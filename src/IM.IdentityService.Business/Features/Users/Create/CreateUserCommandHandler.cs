using IM.Common.Models;
using IM.IdentityService.Business.InternalServices;
using IM.IdentityService.Common.Contracts;
using IM.IdentityService.DataAccess;
using IM.IdentityService.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IM.IdentityService.Business.Features.Users.Create;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<UserCreatedResponse>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<CreateUserCommandHandler> _logger;
    private readonly ServiceDbContext _dataContext;

    public CreateUserCommandHandler(
        UserManager<ApplicationUser> userManager,
        ServiceDbContext dataContext,
        ILogger<CreateUserCommandHandler> logger)
    {
        _logger = logger;
        _dataContext = dataContext;
        _userManager = userManager;
    }

    public async Task<Result<UserCreatedResponse>> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var application = await _dataContext.Applications
                .FirstOrDefaultAsync(q => q.AppKey.Equals(request.AppKey), cancellationToken)
                .ConfigureAwait(false);

            if (application == null)
                return Result<UserCreatedResponse>.Bad("Invalid application key");

            var user = new ApplicationUser
            {
                UserName = request.UserName ?? request.Email ?? request.PhoneNumber 
                    ?? throw new InvalidOperationException("Failed to set Username. Check validator."),
                Email = request.Email,
                PhoneNumber = request.PhoneNumber?.NormalizePhone(),
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return Result<UserCreatedResponse>.Failed(result.ToString());

            await _dataContext.ApplicationUsings.AddAsync(
                new ApplicationUsing
                {
                    ApplicationUserId = user.Id, ApplicationId = application.Id
                }, cancellationToken)
                .ConfigureAwait(false);

            await _dataContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            return Result<UserCreatedResponse>.Ok(new UserCreatedResponse
            {
                Id = user.Id,
                UserName = user.UserName,
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to create user");
            return Result<UserCreatedResponse>.Internal("Failed to create user");
        }
    }
}
