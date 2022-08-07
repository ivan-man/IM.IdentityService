using IM.IdentityService.Business.Models;

namespace IM.IdentityService.Business.InternalServices.Users;

public interface ICurrentUserService
{
    ICurrentUser? GetCurrentUser();
}
