using IM.Common.Models;
using IM.IdentityService.Business.InternalServices.Tokens;
using MediatR;

namespace IM.IdentityService.Business.Features.TokenValidation;

public class ValidateTokenCommandHandler : IRequestHandler<ValidateTokenCommand, Result>
{
    private readonly IJwtTokenGenerator _jwtGenerator;

    public ValidateTokenCommandHandler(
        IJwtTokenGenerator jwtGenerator)
    {
        _jwtGenerator = jwtGenerator;
    }

    public async Task<Result> Handle(ValidateTokenCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _jwtGenerator.Validate(request.Token, request.Temp, cancellationToken);
        return validationResult;
    }
}
