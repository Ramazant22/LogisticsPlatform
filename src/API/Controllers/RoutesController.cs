using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LogisticsPlatform.Modules.Routes.Application;

namespace LogisticsPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RoutesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RoutesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoute([FromBody] CreateRouteCommand command)
    {
        var routeId = await _mediator.Send(command);
        return Ok(new { RouteId = routeId, Message = "Rota baþarýyla planlandý!" });
    }
}
