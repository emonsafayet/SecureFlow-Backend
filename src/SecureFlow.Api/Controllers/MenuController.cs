using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureFlow.Application.Authorization;
using SecureFlow.Application.Menus.DTOs;
using SecureFlow.Application.Menus.Queries.GetMenus;
using SecureFlow.Shared.Authorization;
using SecureFlow.Shared.Models;

namespace SecureFlow.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/menus")]
public class MenuController : ControllerBase
{
    private readonly IMediator _mediator;

    public MenuController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AuthorizePermission(Actions.View, Resources.Users)]
    [HttpGet]
    public async Task<ApiResponse<PaginationResponse<MenuDto>>> GetMenus([FromQuery] PaginationFilter filter)
    {
        var result = await _mediator.Send(new GetMenusQuery(filter));
        return new ApiResponse<PaginationResponse<MenuDto>>(result);
    }
}