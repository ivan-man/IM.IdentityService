using System.Runtime.Serialization;

namespace IM.IdentityService.Common.Contracts;

[DataContract]
public class UserCreatedResponse
{
    [DataMember(Order = 1)] public Guid Id { get; set; }
    [DataMember(Order = 2)] public string? UserName { get; set; }
}
