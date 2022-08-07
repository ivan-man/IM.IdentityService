using Microsoft.AspNetCore.Mvc;

namespace IM.IdentityService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class EchoController : ControllerBase
{
    [HttpGet]
    public IActionResult Echo() => Ok();
}
