using System.ComponentModel.DataAnnotations;
using IM.Common.Models.Domain;

namespace IM.IdentityService.Domain.Models;

public class Application : BaseEntity<int>
{
    [MaxLength(256)] public string Name { get; set; }
    [MaxLength(128)] public string AppKey { get; set; }
    [MaxLength(256)] public string AppName { get; set; }
    public List<ApplicationUser> Users { get; set; }
}
