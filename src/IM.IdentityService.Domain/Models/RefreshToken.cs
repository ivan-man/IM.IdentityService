using System.ComponentModel.DataAnnotations;

namespace IM.IdentityService.Domain.Models;

public class RefreshToken
{
    [Key]
    public int Id { get; set; }
    public string Token { get; set; }
    public string JwtId { get; set; }
    public bool Revoked { get; set; }
    public DateTime AddedDate { get; set; }
    public DateTime ExpiryDate { get; set; }

    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; }
}
