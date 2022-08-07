using IM.Common.Models;
using IM.IdentityService.Client.Models;

namespace IM.IdentityService.Client;

public interface IIdentityService
{
    ValueTask<Result> ValidateToken(ValidateTokenRequest request, CancellationToken token = default);
    ValueTask<Result> CreateUser(CreateUserRequest request, CancellationToken token = default);
}
