using IM.Common.Models;
using IM.IdentityService.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace IM.IdentityService.Business.Features.Users.Create;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<CreateUserCommandHandler> _logger;

    public CreateUserCommandHandler(UserManager<ApplicationUser> userManager, ILogger<CreateUserCommandHandler> logger)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new ApplicationUser
        {
            UserName = request.UserName ?? string.Empty,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
        };
        
        var result = await _userManager.CreateAsync(user, request.Password);

        return result.Succeeded ? Result.Ok() : Result.Failed(result.ToString()); 
    }
}
