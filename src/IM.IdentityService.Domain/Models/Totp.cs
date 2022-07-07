namespace IM.IdentityService.Domain.Models;

public class Totp
{
    public int UserId { get; set; }
    public long Id { get; set; }
    public string HashTopt { get; set; }
    public string Token { get; set; }
    public long TimeToLive { get; set; }
    public DateTime Created { get; set; }
    public bool IsActive { get; set; }
}
