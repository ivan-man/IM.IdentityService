using FluentValidation;

namespace IM.IdentityService.Business.Features.TokenValidation;

public class ValidateTokenCommandValidator : AbstractValidator<ValidateTokenCommand>
{
    public ValidateTokenCommandValidator()
    {
        RuleFor(x => x.AppKey).NotEmpty()
            .WithMessage("Invalid Application key");
    }
}
