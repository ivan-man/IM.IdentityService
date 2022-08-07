using System.Runtime.Serialization;
using IM.IdentityService.Common.Enums;

namespace IM.IdentityService.Common.Models;

[DataContract]
public class LoginModel
{
    [DataMember(Order = 1)] public string? UserName { get; set; }
    [DataMember(Order = 2)] public string? Email { get; set; }
    [DataMember(Order = 3)] public string? PhoneNumber { get; set; }

    [DataMember(Order = 4)] public string Password { get; set; }

    [DataMember(Order = 5)] public ConfirmationType? ConfirmationType { get; set; }
}
