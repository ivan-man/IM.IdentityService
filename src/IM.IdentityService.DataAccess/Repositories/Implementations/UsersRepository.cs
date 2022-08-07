using System.Linq.Expressions;
using IM.Common.EF.Repositories.Implementations;
using IM.Common.Models.Domain;
using IM.IdentityService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace IM.IdentityService.DataAccess.Repositories.Implementations;

public class UsersRepository : BaseRepositoryAdvanced<ApplicationUser, Guid>, IUsersRepository
{
    public UsersRepository(DbContext context) : base(context)
    {
    }
}
