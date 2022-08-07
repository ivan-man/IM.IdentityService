namespace IM.IdentityService.Business.Models;

public class TotpDto
{
    public string? Hash { get; set; }
    public string? Token { get; set; }
    public DateTime? Expired { get; set; }
    public long? TimeToLive { get; set; }
}
