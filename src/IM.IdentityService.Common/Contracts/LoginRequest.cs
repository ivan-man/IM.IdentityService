using System.Runtime.Serialization;
using IM.IdentityService.Common.Enums;

namespace IM.IdentityService.Common.Contracts;

[DataContract]
public class LoginRequest
{
    [DataMember(Order = 1)] public Guid? UserId { get; set; }
    [DataMember(Order = 2)] public string? UserName { get; set; }
    [DataMember(Order = 3)] public string? Email { get; set; }
    [DataMember(Order = 4)] public string? PhoneNumber { get; set; }

    [DataMember(Order = 5)] public string Password { get; set; }
    [DataMember(Order = 6)] public string AppKey { get; set; }

    [DataMember(Order = 7)] public ConfirmationType? ConfirmationType { get; set; }
}
