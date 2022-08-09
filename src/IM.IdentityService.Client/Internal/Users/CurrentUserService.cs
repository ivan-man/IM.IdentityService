using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IM.IdentityService.Common.Consts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace IM.IdentityService.Client.Internal.Users;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _contextAccessor;

    public CurrentUserService(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
    }

    public virtual ICurrentUser? GetCurrentUser()
    {
        var jwt = GetJwtFromHttpContext();
        var jwtData = ReadJwtToken(jwt);

        return jwtData;
    }

    private string? GetJwtFromHttpContext()
    {
        StringValues authHeader = string.Empty;
        
        if (!_contextAccessor.HttpContext?.Request.Headers.TryGetValue("Authorization", out authHeader) ?? false)
            _contextAccessor.HttpContext?.Request.Query.TryGetValue("access_token", out authHeader);

        return string.IsNullOrWhiteSpace(authHeader.ToString())
            ? null
            : authHeader.ToString().Split(' ').LastOrDefault();
    }

    private static ICurrentUser? ReadJwtToken(string jwt)
    {
        if (string.IsNullOrWhiteSpace(jwt))
            throw new ArgumentNullException(nameof(jwt));

        var token = new JwtSecurityTokenHandler().ReadJwtToken(jwt);

        var id = token.Claims.FirstOrDefault(x => x.Type.Equals(ApplicationClaims.UserId))?.Value;
        var email = token.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Email))?.Value;
        var phone = token.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.PhoneNumber))?.Value;
        var jti = token.Claims.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Jti))?.Value;
        //ToDo check
        var roles = token.Claims.Where(x => x.Type.Equals(ClaimTypes.Role));

        return !Guid.TryParse(id, out var userId)
            ? null
            : new CurrentUser(userId, email, phone, jti, roles.Select(x => x.Value).ToArray());
    }
}
