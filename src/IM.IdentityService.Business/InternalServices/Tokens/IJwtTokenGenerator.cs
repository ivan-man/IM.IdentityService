using IM.Common.Models;
using IM.IdentityService.Domain.Models;

namespace IM.IdentityService.Business.InternalServices.Tokens;

public interface IJwtTokenGenerator
{
    Task<TokenModel> Generate(ApplicationUser user, string appKey, bool? temp = false, CancellationToken cancellationToken = default);
    Task<Result> Validate(string token, string appKey, bool temp, CancellationToken cancellationToken = default);
}
