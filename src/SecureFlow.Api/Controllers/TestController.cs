using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureFlow.Application.Common.Interfaces;

namespace SecureFlow.Api.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    [HttpGet("public")]
    public IActionResult Public()
    {
        return Ok("This is public");
    }

    [Authorize]
    [HttpGet("secure")]
    public IActionResult Secure()
    {
        return Ok(new
        {
            Message = "This is secured",
            User = User.Identity?.Name,
            Claims = User.Claims.Select(c => new { c.Type, c.Value })
        });
    }
    [Authorize]
    [HttpGet("me")]
    public IActionResult Me(
    [FromServices] ICurrentUserService currentUser)
    {
        return Ok(new
        {
            currentUser.UserId,
            currentUser.Email,
            currentUser.IsAuthenticated
        });
    }
}
