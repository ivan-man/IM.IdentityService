namespace IM.IdentityService.Business.Configuration;

public class JwtConfig
{
    public string SecretTemp { get; set; }
    public int AccessTtl { get; set; } = 5;
    public int TempTtl { get; set; } = 1;
    public int RefreshTtl { get; set; } = 60 * 24 * 7;
}
