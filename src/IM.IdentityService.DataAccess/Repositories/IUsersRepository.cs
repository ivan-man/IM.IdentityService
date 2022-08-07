using System.Linq.Expressions;
using IM.Common.EF.Repositories;
using IM.IdentityService.Domain.Models;

namespace IM.IdentityService.DataAccess.Repositories;

public interface IUsersRepository : IBaseRepositoryAdvanced<ApplicationUser, Guid>
{
}
