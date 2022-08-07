using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IM.Common.Models;
using IM.IdentityService.Business.Configuration;
using IM.IdentityService.Common.Consts;
using IM.IdentityService.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace IM.IdentityService.Business.InternalServices.Tokens;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtConfig _jwtConfig;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<JwtTokenGenerator> _logger;

    public JwtTokenGenerator(
        IOptions<JwtConfig> options,
        UserManager<ApplicationUser> userManager,
        ILogger<JwtTokenGenerator> logger)
    {
        _jwtConfig = options.Value;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<TokenModel> Generate(ApplicationUser user, bool? temp,
        CancellationToken cancellationToken = default)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(temp ?? false ? _jwtConfig.SecretTemp : _jwtConfig.Secret);
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.PhoneNumber, user.PhoneNumber),
            new Claim(ApplicationClaims.UserId, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString("O")),
        }.Concat(roles.Select(e => new Claim(ClaimTypes.Role, e)));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddMinutes(temp ?? false ? _jwtConfig.TempTtl : _jwtConfig.AccessTtl),
            
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature),
            
            AdditionalHeaderClaims = new Dictionary<string, object>
                { { "subType", temp ?? false ? "temp" : "bearer" } },
            
            Subject = new ClaimsIdentity(claims),
        };

        var token = jwtTokenHandler.CreateToken(tokenDescriptor);

        var jwtToken = jwtTokenHandler.WriteToken(token);

        return new TokenModel
        {
            Id = token.Id,
            Token = jwtToken
        };
    }

    public Task<Result> Validate(string token, bool temp, CancellationToken cancellationToken = default)
    {
        Claim? userIdClaim = null;

        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(temp ? _jwtConfig.SecretTemp : _jwtConfig.Secret);

        try
        {
            var jsonToken = jwtTokenHandler.ReadToken(token);
            if (jsonToken is not JwtSecurityToken tokenS)
            {
                return Task.FromResult((Result.Bad("Provided token is not JWT")));
            }

            userIdClaim = tokenS.Claims.First(e => e.Type == ApplicationClaims.UserId);

            jwtTokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                RequireSignedTokens = true,
                RequireExpirationTime = true
            }, out var validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;

            var userId = int.Parse(jwtToken.Claims.First(x => x.Type == ApplicationClaims.UserId).Value);

            return Task.FromResult(
                userId >= 0 && (DateTime.UtcNow - jwtToken.IssuedAt).TotalSeconds < _jwtConfig.AccessTtl * 60
                    ? Result.Ok()
                    : Result.UnAuthorized("User id is not valid"));
        }
        catch (ArgumentException e)
        {
            _logger.LogWarning(e, "JWT does not contain 'id' claim");

            return Task.FromResult(Result.Bad("JWT does not contain required claims"));
        }
        catch (SecurityTokenExpiredException e)
        {
            _logger.LogWarning(e, "Provided token Expired. UserId: {Claim}", userIdClaim?.Value);
            return Task.FromResult(Result.Forbidden("Token expired"));
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Provided token is not valid. UserId: {Claim}", userIdClaim?.Value);
            return Task.FromResult(Result.Validation("Token is not valid"));
        }
    }
}
