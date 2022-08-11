using IM.Common.Models;
using IM.IdentityService.Business.Models;
using MediatR;

namespace IM.IdentityService.Business.Features.Users.Create;

public class CreateUserCommand : IRequest<Result>
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string AppKey { get; set; }
}
