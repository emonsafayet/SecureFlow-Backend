using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    [Authorize(Policy = nameof(Permissions.ViewUsers))]
    [HttpGet]
    public async Task<IActionResult> GetMenus()
    {
        var result = await _mediator.Send(new GetMenusQuery());
        return Ok(result);
    }
}