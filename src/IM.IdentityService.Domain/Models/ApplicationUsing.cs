namespace IM.IdentityService.Domain.Models;

public class ApplicationUsing
{
    public Guid ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
    
    public int ApplicationId { get; set; }
    public Application Application { get; set; }
}
