using IM.Common.Models;
using MediatR;

namespace IM.IdentityService.Business.Features.TokenValidation;

public class ValidateTokenCommand : IRequest<Result>
{
    public bool Temp { get; set; }
    public string Token { get; set; }
    public string AppKey { get; set; }
}
