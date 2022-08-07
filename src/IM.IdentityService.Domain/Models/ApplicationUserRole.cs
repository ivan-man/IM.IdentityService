using IM.Common.Models.Domain;
using Microsoft.AspNetCore.Identity;

namespace IM.IdentityService.Domain.Models;

public class ApplicationUserRole : IdentityRole<Guid>, IBaseEntity<Guid>
{
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
}
