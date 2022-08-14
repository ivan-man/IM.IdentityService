using FluentValidation;

namespace IM.IdentityService.Business.Features.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty()
            .When(x => string.IsNullOrWhiteSpace(x.Email) 
                       && string.IsNullOrWhiteSpace(x.PhoneNumber) 
                       && string.IsNullOrWhiteSpace(x.UserName));
        
        RuleFor(x => x.UserName).NotEmpty()
            .When(x => (!x.UserId.HasValue || x.UserId == Guid.Empty) 
                       && string.IsNullOrWhiteSpace(x.Email) 
                       && string.IsNullOrWhiteSpace(x.PhoneNumber));
        
        RuleFor(x => x.Email).NotEmpty()
            .When(x => (!x.UserId.HasValue || x.UserId == Guid.Empty) 
                       && string.IsNullOrWhiteSpace(x.UserName) 
                       && string.IsNullOrWhiteSpace(x.PhoneNumber));
        
        RuleFor(x => x.PhoneNumber).NotEmpty()
            .When(x => (!x.UserId.HasValue || x.UserId == Guid.Empty) 
                       && string.IsNullOrWhiteSpace(x.UserName)
                       && string.IsNullOrWhiteSpace(x.Email));
        
        RuleFor(x => x.Password).NotEmpty();
    }
}
