using IM.Common.Models;
using IM.IdentityService.Business.InternalServices;
using IM.IdentityService.Business.InternalServices.Tokens;
using IM.IdentityService.Business.InternalServices.Totp;
using IM.IdentityService.Business.Models;
using IM.IdentityService.Common.Models;
using IM.IdentityService.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IM.IdentityService.Business.Features.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<ResponseToken>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtTokenGenerator _tokenGenerator;
    // private readonly ITotpGenerator _totpGenerator;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtTokenGenerator tokenGenerator,
        // ITotpGenerator totpGenerator,
        ILogger<LoginCommandHandler> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenGenerator = tokenGenerator;
        // _totpGenerator = totpGenerator;
        _logger = logger;
    }

    public async Task<Result<ResponseToken>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        ApplicationUser? user = null;

        if (!string.IsNullOrWhiteSpace(request.UserName))
            user = await _userManager.FindByNameAsync(request.UserName);

        if (!string.IsNullOrWhiteSpace(request.Email))
            user = await _userManager.FindByEmailAsync(request.UserName);

        if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
            user = await _userManager.Users.FirstOrDefaultAsync(
                q => q.PhoneNumber == request.PhoneNumber.NormalizePhone(), 
                cancellationToken: cancellationToken);

        if (user == null)
        {
            _logger.LogWarning("User not found {@Request}", request);
            return Result<ResponseToken>.NotFound("User not found");
        }

        if (user.LockoutEnd != null)
        {
            return Result<ResponseToken>.Forbidden("Account locked");
        }
            
        if (!await _userManager.CheckPasswordAsync(user, request.Password))
        {
            _logger.LogWarning("{UserId} provided invalid password", user.Id);
            return Result<ResponseToken>.Bad("Invalid password");
        }

        if (user.TwoFactorEnabled)
        {
            //ToDo send email/sms 
        }
        
        var access = await _tokenGenerator.Generate(user, true, cancellationToken);
        // var totp = await _totpGenerator.GenerateToken(user, cancellationToken: cancellationToken);
        
        return Result<ResponseToken>.Ok(new ResponseToken
        {
            IsNeed2FA = true,
            AccessToken = access.Token,
            // Hash = totp.Hash
        });
    }
}
