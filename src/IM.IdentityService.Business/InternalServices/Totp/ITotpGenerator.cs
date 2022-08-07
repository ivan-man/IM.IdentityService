using IM.IdentityService.Business.Models;
using IM.IdentityService.Domain.Models;

namespace IM.IdentityService.Business.InternalServices.Totp;

public interface ITotpGenerator
{
    public const int DefaultTimeRange = 30;
    public const int DefaultTokenSize = 6;
    
    public Task<TotpDto> GenerateToken(
        ApplicationUser user,
        int timeRange = DefaultTimeRange,
        int tokenSize = DefaultTokenSize,
        CancellationToken cancellationToken = default);

    public Task<ValidateToptResponse> ValidateToken(Guid userId, string code, CancellationToken cancellationToken = default);
    public Task<bool> RevokeTotp(Guid userId, CancellationToken cancellationToken = default);
    public Task<bool> DeactivateTotp(Guid userId, CancellationToken cancellationToken = default);
}
