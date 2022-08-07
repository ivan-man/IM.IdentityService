using IM.Common.Models;
using IM.IdentityService.Domain.Models;

namespace IM.IdentityService.Business.InternalServices.Tokens;

public interface IJwtTokenGenerator
{
    Task<TokenModel> Generate(ApplicationUser user, bool? temp, CancellationToken cancellationToken = default);
    Task<Result> Validate(string token, bool temp, CancellationToken cancellationToken = default);
}
