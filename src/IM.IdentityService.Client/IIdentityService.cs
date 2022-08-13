using IM.Common.Models;
using System.ServiceModel;
using IM.IdentityService.Common.Contracts;

namespace IM.IdentityService.Client;

[ServiceContract]
public interface IIdentityService
{
    [OperationContract]
    ValueTask<Result> ValidateToken(ValidateTokenRequest request, CancellationToken token = default);

    [OperationContract]
    ValueTask<Result<UserCreatedResponse>> CreateUser(CreateUserRequest request, CancellationToken token = default);

    [OperationContract]
    ValueTask<Result<TokenResponse>> Login(LoginRequest request, CancellationToken token = default);
}
