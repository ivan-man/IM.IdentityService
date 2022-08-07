using IM.Common.Models;
using IM.IdentityService.Business.InternalServices.Tokens;

namespace IM.IdentityService.Business.Features.TokenValidation;

public class ValidateTokenCommandHandler
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
