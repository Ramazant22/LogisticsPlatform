using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LogisticsPlatform.Modules.Fleet.Application;

namespace LogisticsPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Endpoint güvenliði devrede
public class VehiclesController : ControllerBase
{
    private readonly IMediator _mediator;

    public VehiclesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleCommand command)
    {
        var vehicleId = await _mediator.Send(command);
        return Ok(new { VehicleId = vehicleId });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllVehicles()
    {
        var query = new GetAllVehiclesQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
