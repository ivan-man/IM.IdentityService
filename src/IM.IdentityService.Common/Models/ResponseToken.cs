using System.Runtime.Serialization;

namespace IM.IdentityService.Common.Models;

[DataContract]
public class ResponseToken
{
    [DataMember(Order = 1)] public string AccessToken { get; set; }
    [DataMember(Order = 2)] public string Refresh { get; set; }
    [DataMember(Order = 3)] public bool IsNeed2FA { get; set; }
    [DataMember(Order = 4)] public string Code { get; set; }
    [DataMember(Order = 5)] public bool? EmailConfirmed { get; set; }
    // [DataMember(Order = 6)] public string Hash { get; set; }
}
