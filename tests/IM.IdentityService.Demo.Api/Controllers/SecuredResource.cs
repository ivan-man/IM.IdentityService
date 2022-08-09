using IM.IdentityService.Common.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IM.IdentityService.Demo.Api.Controllers;

[ApiController, Route("[controller]")]
[Authorize(Policy = AuthorizationSchemes.General, Roles = Roles.User)]
public class SecuredResourceController : ControllerBase
{
    private readonly ILogger<SecuredResourceController> _logger;

    public SecuredResourceController(ILogger<SecuredResourceController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get() => Ok("Success");
}
