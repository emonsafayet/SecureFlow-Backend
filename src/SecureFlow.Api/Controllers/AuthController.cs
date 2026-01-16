using Microsoft.AspNetCore.Mvc;
using SecureFlow.Application.Auth.DTOs;
using SecureFlow.Application.Auth.Login;

namespace SecureFlow.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly LoginService _loginService;

    public AuthController(LoginService loginService)
    {
        _loginService = loginService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto request)
    {
        var result = await _loginService.LoginAsync(request);
        return Ok(result);
    }
}
