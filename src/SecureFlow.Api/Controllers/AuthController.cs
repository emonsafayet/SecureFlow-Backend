using MediatR;
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc;
using SecureFlow.Application.Auth.DTOs;
using SecureFlow.Application.Auth.Login;
using SecureFlow.Application.Auth.Users.Queries;
using SecureFlow.Application.Authorization;
using SecureFlow.Application.Users;
using SecureFlow.Shared.Authorization;
using SecureFlow.Shared.Models;

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
    public async Task<ApiResponse<LoginResponseDto>> Login(LoginCommand command)
    {
        var result = await _mediator.Send(command);
        return new ApiResponse<LoginResponseDto>(result);
    }
     
    [AuthorizePermission(Actions.View, Resources.Users)]
    [HttpGet("users")]
    public async Task<ApiResponse<PaginationResponse<UserDto>>> GetUsers([FromQuery] PaginationFilter filter)
    {
        var result = await _mediator.Send(new GetUsersQuery(filter));
        return new ApiResponse<PaginationResponse<UserDto>>(result);
    }

    [AuthorizePermission(Actions.View, Resources.Users)]
    [HttpGet("{id:int}")]
    public async Task<ApiResponse<UserDto>> GetUser(int id)
    {
        var result = await _mediator.Send(new GetUserByIdQuery(id));
        return new ApiResponse<UserDto>(result);
    }
}
