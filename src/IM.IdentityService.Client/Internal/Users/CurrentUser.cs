namespace IM.IdentityService.Client.Internal.Users;

public class CurrentUser : ICurrentUser
{
    public Guid Id { get; init; }
    public string Email { get; init; }
    public string Phone { get; init; }
    public string Jti { get; init; }
    public string[] Roles { get; init; }
    
    public CurrentUser(Guid id, string email, string phone, string jti, string[] roles)
    {
        Id = id;
        Email = email;
        Phone = phone;
        Jti = jti;
        Roles = roles;
    }
}
