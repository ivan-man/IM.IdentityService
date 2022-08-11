using FluentValidation;

namespace IM.IdentityService.Business.Features.Users.Create;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.AppKey).NotEmpty()
            .WithMessage("Invalid Application key");
        
        RuleFor(x => x.UserName).NotEmpty()
            .When(x => string.IsNullOrWhiteSpace(x.Email) && string.IsNullOrWhiteSpace(x.PhoneNumber))
            .WithMessage("Set Username, Email or Phone number");

        RuleFor(x => x.Email).NotEmpty()
            .When(x => string.IsNullOrWhiteSpace(x.UserName) && string.IsNullOrWhiteSpace(x.PhoneNumber))
            .WithMessage("Set Username, Email or Phone number");

        RuleFor(x => x.PhoneNumber).NotEmpty()
            .When(x => string.IsNullOrWhiteSpace(x.UserName) && string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("Set Username, Email or Phone number");

        RuleFor(x => x.Password).NotEmpty()
            .WithMessage("Password can't be empty");

        RuleFor(x => x.ConfirmPassword).NotEmpty()
            .WithMessage("Confirm the password");

        RuleFor(x => x.Password).Equal(x => x.ConfirmPassword)
            .WithMessage("Invalid password confirmation");
    }
}
