using IM.IdentityService.Business.Models;
using IM.IdentityService.DataAccess;
using IM.IdentityService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using OtpNet;

namespace IM.IdentityService.Business.InternalServices.Totp;

public class TotpGenerator : ITotpGenerator
{
    private readonly ServiceDbContext _appDbContext;
    private const OtpHashMode HashMode = OtpHashMode.Sha256;

    public TotpGenerator(ServiceDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<TotpDto> GenerateToken(
        ApplicationUser user,
        int timeRange = ITotpGenerator.DefaultTimeRange,
        int tokenSize = ITotpGenerator.DefaultTokenSize,
        CancellationToken cancellationToken = default)
    {
        var seedBytes = CreateSeed();
        var totpEntity = await _appDbContext.Totp.FirstOrDefaultAsync(x => x.UserId == user.Id, cancellationToken);

        if (totpEntity != null)
        {
            var isExpired = totpEntity.Expired.HasValue && totpEntity.Created > totpEntity.Expired.Value;
            return new TotpDto
            {
                Token = isExpired || !totpEntity.IsActive ? null : totpEntity.Token,
                Hash = isExpired || !totpEntity.IsActive ? null : totpEntity.HashTopt,
                Expired = totpEntity.Expired,
                TimeToLive = totpEntity.Expired.HasValue
                    ? (long)(totpEntity.Expired.Value - DateTime.UtcNow).TotalSeconds
                    : null,
            };
        }

        var totp = GetTotp(seedBytes, timeRange, tokenSize);

        var result = new TotpDto
        {
            Token = totp.ComputeTotp(),
            Hash = Base32Encoding.ToString(seedBytes),
            TimeToLive = totp.RemainingSeconds(),
            Expired = totpEntity?.Expired ?? null,
        };

        await _appDbContext.AddAsync(
            new IM.IdentityService.Domain.Models.Totp
            {
                UserId = user.Id,
                HashTopt = result.Hash,
                Token = result.Token,
                Expired = DateTime.UtcNow.AddSeconds((double)result.TimeToLive),
                Created = DateTime.UtcNow,
                IsActive = true
            }, cancellationToken);

        await _appDbContext.SaveChangesAsync(cancellationToken);
        return result;
    }

    public async Task<ValidateToptResponse> ValidateToken(
        Guid userId,
        string code,
        CancellationToken cancellationToken = default)
    {
        var totpInfo = await _appDbContext.Totp.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (totpInfo == null)
            return new ValidateToptResponse { IsValid = false, IsActive = false };

        var totp = GetTotp(
            Base32Encoding.ToBytes(totpInfo.HashTopt),
            ITotpGenerator.DefaultTimeRange,
            ITotpGenerator.DefaultTokenSize);
        
        var result = totp.VerifyTotp(code, out _);

        return new ValidateToptResponse { IsValid = result, IsActive = totpInfo.IsActive };
    }

    public async Task<bool> RevokeTotp(Guid userId, CancellationToken cancellationToken = default)
    {
        var totpInfo = await _appDbContext.Totp.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        if (totpInfo is null)
            return false;

        _appDbContext.Totp.Remove(totpInfo);
        return await _appDbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeactivateTotp(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var totpInfo = await _appDbContext.Totp.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (totpInfo is null)
            return false;

        totpInfo.IsActive = false;
        _appDbContext.Update(totpInfo);
        return await _appDbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    private static byte[] CreateSeed()
    {
        var bytes = KeyGeneration.GenerateRandomKey();
        return bytes;
    }

    private static OtpNet.Totp GetTotp(byte[] seed, int timeRange, int tokenSize)
    {
        return new OtpNet.Totp(seed, timeRange, HashMode, tokenSize);
    }
}
