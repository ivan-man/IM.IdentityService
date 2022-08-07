using System.Runtime.Serialization;

namespace IM.IdentityService.Client.Models;

[DataContract]
public class TokenResponse
{
    [DataMember(Order = 1)] public string Access { get; set; }
    [DataMember(Order = 2)] public string Refresh { get; set; }
    [DataMember(Order = 3)] public string Hash { get; set; }
    [DataMember(Order = 4)] public bool IsNeed2FA { get; set; }
    [DataMember(Order = 5)] public string Code { get; set; }
}
