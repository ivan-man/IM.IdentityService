using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IM.Common.Models;
using IM.IdentityService.Client.Models;
using IM.IdentityService.Common.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace IM.IdentityService.Client.Internal;

public class TokenRequirementHandler : AuthorizationHandler<TokenRequirement>
{
    private readonly IDistributedCache _cache;
    private readonly IHttpContextAccessor _accessor;
    private readonly IIdentityService _identityService;
    private readonly ILogger<TokenRequirementHandler> _logger;

    public TokenRequirementHandler(
        IDistributedCache cache,
        IHttpContextAccessor accessor,
        IIdentityService identityService,
        ILogger<TokenRequirementHandler> logger)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _accessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
        _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        TokenRequirement requirement)
    {
        var tokenViewModel = GetTokenFromRequest();
        if (string.IsNullOrWhiteSpace(tokenViewModel.Token) || string.IsNullOrWhiteSpace(tokenViewModel.Scheme))
        {
            Fail(context, "Missing authorization token");
            return;
        }

        var jwt = ReadToken(tokenViewModel.Token);
        if (jwt is null)
        {
            Fail(context);
            return;
        }

        var existsJtiToken = await ExistsJtiToken(jwt);
        if (existsJtiToken)
        {
            Fail(context);
            return;
        }

        var exists = await _cache.GetAsync(jwt.RawSignature) is not null;
        if (exists)
        {
            SetIdentity(context, jwt);
            context.Succeed(requirement);
            return;
        }

        var validationResult = await CheckHeader(tokenViewModel.Scheme, tokenViewModel.Token);
        if (!validationResult.Success)
        {
            _logger.LogWarning("Header is not valid: {Message}", validationResult.Message);
            try
            {
                Fail(context, validationResult.Message);
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Could not fail auth");
            }

            return;
        }

        await CacheToken(jwt);
        SetIdentity(context, jwt);
        context.Succeed(requirement);
    }

    private void Fail(AuthorizationHandlerContext context, string message = "Authorization failed")
    {
        if (!context.HasFailed && _accessor.HttpContext?.Response.HasStarted == false)
            context.Fail();

        _logger.LogWarning("Authorization failed: {Message}", message);
    }

    private static void SetIdentity(AuthorizationHandlerContext context, JwtSecurityToken jwt)
    {
        if (context.User.Identities.Any() && context.User.Claims.Any())
            return;

        var identity = context.User.Identities.FirstOrDefault();
        var role = jwt.Claims.FirstOrDefault(e => e.Type == ApplicationClaims.Role);

        identity?.AddClaims(jwt.Claims.Where(e => e.Type != role?.Type));

        if (role != null)
            identity?.AddClaim(new Claim(ClaimTypes.Role, role.Value));
    }

    private JwtSecurityToken? ReadToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return null;

        var handler = new JwtSecurityTokenHandler();
        try
        {
            var jsonToken = handler.ReadToken(token);
            return jsonToken as JwtSecurityToken;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to read {Token}", token);
            return null;
        }
    }

    private async Task<bool> ExistsJtiToken(JwtSecurityToken tokenS)
    {
        var jti = tokenS?.Claims?.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Jti)?.Value;

        return !string.IsNullOrWhiteSpace(jti) && await _cache.GetAsync(jti) != null;
    }

    private async Task<Result> CheckHeader(string scheme, string token)
    {
        _logger.LogInformation("Will check {Token} explicitly", token);
        var tokenIsValid = await _identityService.ValidateToken(new ValidateTokenRequest
        {
            Temp = scheme == AuthorizationSchemes.Temp,
            Token = token
        });

        _logger.LogInformation("Explicit token check result: {@Result}", tokenIsValid);

        return new Result
        {
            Success = tokenIsValid.Success,
            Message = tokenIsValid.Message,
        };
    }

    private TokenViewModel GetTokenFromRequest()
    {
        var tokenHeader = _accessor.HttpContext?.Request.Headers["Authorization"].ToString();
        if (!string.IsNullOrWhiteSpace(tokenHeader))
        {
            var data = tokenHeader.Split(' ');
            return new TokenViewModel
            {
                Scheme = data[0],
                Token = data.Length > 1 ? data[1] : null
            };
        }

        StringValues queryToken = string.Empty;
        var tokenGetted = _accessor.HttpContext?.Request.Query.TryGetValue("access_token", out queryToken) ?? false;
        
        if (!tokenGetted)
        {
            return new TokenViewModel
            {
                Scheme = null,
                Token = queryToken,
            };
        }

        return new TokenViewModel
        {
            Scheme = AuthorizationSchemes.General,
            Token = queryToken,
        };
    }

    private async Task CacheToken(JwtSecurityToken tokenS)
    {
        await _cache.SetAsync(
            tokenS.RawSignature,
            new byte[] { 0 },
            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = GetTokenTtl(tokenS), });
    }

    private static TimeSpan GetTokenTtl(JwtSecurityToken tokenS)
    {
        var expires = DateTimeOffset.FromUnixTimeSeconds(
            long.Parse(tokenS.Claims.First(claim => claim.Type == "exp").Value));

        var result = expires - DateTimeOffset.UtcNow;
        return result.Seconds < 0 ? TimeSpan.FromMilliseconds(1) : result;
    }
}

internal class TokenViewModel
{
    public string? Scheme { get; init; }
    public string? Token { get; init; }
}
