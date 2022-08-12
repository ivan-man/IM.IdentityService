using IM.IdentityService.Client;
using IM.IdentityService.Client.Models;
using IM.IdentityService.Client.Settings;
using IM.IdentityService.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IM.IdentityService.Demo.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IIdentityService _identityService;
    private readonly IdentitySettings _identitySettings;
    
    public AuthController(
        IIdentityService identityService, 
        IOptions<IdentitySettings> identitySettings, 
        ILogger<AuthController> logger)
    {
        _logger = logger;
        _identityService = identityService;
        _identitySettings = identitySettings.Value;
    }

    /// <summary>
    /// Register
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserRequest request)
    {
        try
        {
            request.AppKey = _identitySettings.AppKey;
            var registerResult = await _identityService.CreateUser(request, HttpContext.RequestAborted);
            if (registerResult.Success)
                return Ok();
            
            return BadRequest(registerResult.Message);
        }
        catch (Exception e)
        {
            return  StatusCode(500, e);
        }
    } 

    /// <summary>
    /// Login
    /// </summary>
    /// <returns>Access Token</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel request)
    {
        try
        {
            var registerResult = await _identityService.Login(request, HttpContext.RequestAborted);
            if (registerResult.Success)
                return Ok(registerResult.Data);
            
            return BadRequest(registerResult.Message);
        }
        catch (Exception e)
        {
            return  StatusCode(500, e);
        }
    } 

    /// <summary>
    /// Validate
    /// </summary>
    /// <returns>Access Token</returns>
    [HttpPost("validate")]
    public async Task<IActionResult> Validate([FromBody] ValidateTokenRequest request)
    {
        try
        {
            var registerResult = await _identityService.ValidateToken(request, HttpContext.RequestAborted);
            if (registerResult.Success)
                return Ok();
            
            return BadRequest(registerResult.Message);
        }
        catch (Exception e)
        {
            return  StatusCode(500, e);
        }
    } 
}
