namespace IM.IdentityService.Domain.Models;

public class Totp
{
    public Guid UserId { get; set; }
    public int Id { get; set; }
    public string HashTopt { get; set; }
    public string Token { get; set; }
    public DateTime? Expired { get; set; }
    public DateTime Created { get; set; }
    public bool IsActive { get; set; }
}
