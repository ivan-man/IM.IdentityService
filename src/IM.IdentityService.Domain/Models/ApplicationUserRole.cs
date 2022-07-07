using Microsoft.AspNetCore.Identity;

namespace IM.IdentityService.Domain.Models;

public class ApplicationUserRole : IdentityRole<int>, IEntity<int>
{
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
}
