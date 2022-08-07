using System.Runtime.Serialization;

namespace IM.IdentityService.Client.Models;

[DataContract]
public class ValidateTokenRequest
{
    [DataMember(Order = 1)] public bool Temp { get; set; }
    [DataMember(Order = 2)] public string Token { get; set; }
}
