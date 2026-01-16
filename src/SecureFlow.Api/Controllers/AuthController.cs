using MediatR;
using Microsoft.AspNetCore.Mvc;
using SecureFlow.Application.Auth.Login;
using SecureFlow.Application.Auth.Users.Queries;
using SecureFlow.Application.Users;

namespace SecureFlow.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("{userId:int}")]
    public async Task<ActionResult<UserDto>> GetUser(int userId)
    {
        var result = await _mediator.Send(new GetUserByIdQuery(userId));
        return Ok(result);
    }
}
