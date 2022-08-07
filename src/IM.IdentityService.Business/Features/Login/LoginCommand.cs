using IM.Common.Models;
using IM.IdentityService.Business.Models;
using IM.IdentityService.Common.Enums;
using IM.IdentityService.Common.Models;
using MediatR;

namespace IM.IdentityService.Business.Features.Login;

public class LoginCommand : IRequest<Result<ResponseToken>>
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
        
    public string Password { get; set; }
    
    public ConfirmationType? ConfirmationType { get; set; }
}
