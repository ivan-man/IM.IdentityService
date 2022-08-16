namespace IM.IdentityService.Demo.Api.Models;

public class CreateUserViewModel
{
    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string Password { get; set; }

    public string ConfirmPassword { get; set; }
}
