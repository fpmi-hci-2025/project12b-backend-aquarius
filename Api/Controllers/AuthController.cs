using Application.Dto.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{

    public AuthController()
    {

    }

    [AllowAnonymous]
    [HttpPost("sign-up")]
    public async Task<ActionResult> SignUp([FromBody] RegisterDto registerDto)
    {
        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("sign-in")]
    public async Task<ActionResult> SignIn([FromBody] LoginDto loginDto)
    {
        return Ok();
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<ActionResult> Refresh([FromBody] RefreshDto tokensToRefresh)
    {
        return Ok();
    }
}
