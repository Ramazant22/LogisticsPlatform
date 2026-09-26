using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LogisticsPlatform.Modules.Drivers.Application;

namespace LogisticsPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DriversController : ControllerBase
{
    private readonly IMediator _mediator;

    public DriversController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateDriver([FromBody] CreateDriverCommand command)
    {
        var driverId = await _mediator.Send(command);
        return Ok(new { DriverId = driverId, Message = "Sürücü baþarýyla sisteme kaydedildi!" });
    }
}
