using IM.Common.Models;
using IM.IdentityService.Client.Models;
using IM.IdentityService.Common.Models;
using System.ServiceModel;

namespace IM.IdentityService.Client;

[ServiceContract]
public interface IIdentityService
{
    [OperationContract]
    ValueTask<Result> ValidateToken(ValidateTokenRequest request, CancellationToken token = default);

    [OperationContract]
    ValueTask<Result> CreateUser(CreateUserRequest request, CancellationToken token = default);

    [OperationContract]
    ValueTask<Result<ResponseToken>> Login(LoginModel request, CancellationToken token = default);
}
