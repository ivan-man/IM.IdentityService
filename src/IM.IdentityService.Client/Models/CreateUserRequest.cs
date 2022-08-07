using System.Runtime.Serialization;

namespace IM.IdentityService.Client.Models;

[DataContract]
public class CreateUserRequest
{
    [DataMember(Order = 1)] public string? UserName { get; set; }
    [DataMember(Order = 2)] public string? Email { get; set; }
    [DataMember(Order = 3)] public string? PhoneNumber { get; set; }
    [DataMember(Order = 4)] public string Password { get; set; }
    [DataMember(Order = 5)] public string ConfirmPassword { get; set; }
    [DataMember(Order = 6)] public int ApplicationId { get; set; }
}
