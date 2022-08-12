using IM.Common.Models;
using IM.IdentityService.Business.Features.Login;
using IM.IdentityService.Business.Features.TokenValidation;
using IM.IdentityService.Business.Features.Users.Create;
using IM.IdentityService.Client;
using IM.IdentityService.Common.Contracts;
using Mapster;
using MediatR;

namespace IM.IdentityService.Api.Services;

internal class IdentityService : IIdentityService
{
    private readonly IMediator _mediator;

    public IdentityService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async ValueTask<Result> ValidateToken(ValidateTokenRequest request, CancellationToken token = default)
    {
        var result = await _mediator.Send(request.Adapt<ValidateTokenCommand>(), token);
        return result;
    }

    public async ValueTask<Result<TokenResponse>> Login(
        LoginRequest request,
        CancellationToken token = default)
    {
        var result = await _mediator.Send(request.Adapt<LoginCommand>(), token);
        return result;
    }

    public async ValueTask<Result> CreateUser(CreateUserRequest request, CancellationToken token = default)
    {
        var result = await _mediator.Send(new CreateUserCommand
        {
            UserName = request.UserName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Password = request.Password,
            ConfirmPassword = request.ConfirmPassword,
            AppKey = request.AppKey,
        }, token);

        return result;
    }
}
