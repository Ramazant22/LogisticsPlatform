using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MassTransit;
using LogisticsPlatform.BuildingBlocks.Events;

namespace LogisticsPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MaintenanceController : ControllerBase
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MaintenanceController(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    [HttpPost("trigger")]
    public async Task<IActionResult> TriggerMaintenanceEvent([FromBody] VehicleMaintenanceDueEvent maintenanceEvent)
    {
        // Event doðrudan post edildiði için ekstra atamaya gerek yok, RabbitMQ'ya fýrlatýyoruz
        await _publishEndpoint.Publish(maintenanceEvent);
        return Ok(new { Message = $"Bakým eventi RabbitMQ'ya fýrlatýldý! Plaka: {maintenanceEvent.PlateNumber}" });
    }
}
