using IM.Common.Models;
using IM.IdentityService.Business.InternalServices;
using IM.IdentityService.Business.InternalServices.Tokens;
using IM.IdentityService.Common.Contracts;
using IM.IdentityService.DataAccess;
using IM.IdentityService.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IM.IdentityService.Business.Features.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<TokenResponse>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ServiceDbContext _dbContext;
    private readonly SignInManager<ApplicationUser> _signInManager;

    private readonly IJwtTokenGenerator _tokenGenerator;

    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtTokenGenerator tokenGenerator,
        ServiceDbContext dbContext,
        ILogger<LoginCommandHandler> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenGenerator = tokenGenerator;
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<Result<TokenResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        try
        {
            ApplicationUser? user = null;

            var usersQuery = _userManager.Users
                .Include(q => q.ApplicationUsings);

            if (!string.IsNullOrWhiteSpace(request.UserName))
                user = await _userManager.FindByNameAsync(request.UserName)
                    .ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(request.Email))
                user = await _userManager.FindByEmailAsync(request.Email)
                    .ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
                user = await usersQuery.FirstOrDefaultAsync(
                    q => q.PhoneNumber == request.PhoneNumber.NormalizePhone(),
                    cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

            if (request.UserId.HasValue && request.UserId != Guid.Empty)
                user = await _userManager.FindByIdAsync(request.UserId.ToString())
                    .ConfigureAwait(false);

            if (user == null)
            {
                request.Password = string.Empty;
                _logger.LogWarning("User not found {@Request}", request);
                return Result<TokenResponse>.NotFound("User not found");
            }

            if (user.LockoutEnd != null)
            {
                return Result<TokenResponse>.Forbidden("Account locked");
            }

            if (!await _userManager.CheckPasswordAsync(user, request.Password).ConfigureAwait(false))
            {
                _logger.LogWarning("{UserId} provided invalid password", user.Id);
                return Result<TokenResponse>.Bad("Invalid password");
            }

            if (user.TwoFactorEnabled)
            {
                //ToDo send email/sms 
                throw new NotImplementedException();
            }

            var isUserRegisteredInApp = await _dbContext.ApplicationUsings.AnyAsync(q =>
                    q.ApplicationUserId == user.Id && q.Application.AppKey == request.AppKey,
                cancellationToken: cancellationToken);

            if (!isUserRegisteredInApp)
                return Result<TokenResponse>.UnAuthorized("User does not have access to this application");

            var access = await _tokenGenerator.Generate(user, request.AppKey, cancellationToken: cancellationToken);


            return Result<TokenResponse>.Ok(new TokenResponse
            {
                IsNeed2FA = user.TwoFactorEnabled,
                AccessToken = access.Token,
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to login");
            return Result<TokenResponse>.Internal("Failed to login");
        }
    }
}
