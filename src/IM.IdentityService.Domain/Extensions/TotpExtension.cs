using IM.IdentityService.Domain.Models;

namespace IM.IdentityService.Domain.Extensions;

public static class TotpExtension
{
    public static bool IsExpired(this Totp totp)
    {
        var expiration = totp.Created.ToUniversalTime().AddSeconds(totp.TimeToLive);

        //ToDo dateTimeService
        var now = DateTime.UtcNow;

        if (expiration.Date.CompareTo(now.Date) != 0)
            return true;

        return expiration.TimeOfDay.TotalSeconds - now.TimeOfDay.TotalSeconds < 0;
    }
}
