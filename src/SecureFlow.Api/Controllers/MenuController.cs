using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureFlow.Application.Authorization;
using SecureFlow.Shared.Authorization;

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
    public async Task<IActionResult> GetMenus()
    {
        var result = await _mediator.Send(new GetMenusQuery());
        return Ok(result);
    }
}