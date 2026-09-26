using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using LogisticsPlatform.Modules.Tracking.Application;
using LogisticsPlatform.Modules.Tracking.Infrastructure;
using LogisticsPlatform.Modules.Tracking.Application.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;

namespace LogisticsPlatform.API.Controllers;

public class UpdateTrackingRequest
{
    public string ShipmentNumber { get; set; } = string.Empty;
    public string CurrentLocation { get; set; } = string.Empty;
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public string Status { get; set; } = "Yolda";
}

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "OperationsManage")]
public class TrackingController : TenantControllerBase
{
    private readonly IMediator _mediator;
    private readonly TrackingDbContext _context;
    private readonly IHubContext<TrackingHub> _hub;

    public TrackingController(IMediator mediator, TrackingDbContext context, IHubContext<TrackingHub> hub)
    {
        _mediator = mediator;
        _context = context;
        _hub = hub;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        if (!TryGetTenantId(out var tenantId)) return Forbid();
        var records = await _context.TrackingRecords.Where(item => item.TenantId == tenantId).OrderByDescending(x => x.Timestamp).ToListAsync();
        return Ok(records);
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] UpdateTrackingRequest req)
    {
        if (!TryGetTenantId(out var tenantId)) return Forbid();
        var command = new UpdateTrackingCommand
        {
            ShipmentNumber = req.ShipmentNumber,
            CurrentLocation = req.CurrentLocation,
            Latitude = req.Latitude,
            Longitude = req.Longitude,
            Status = req.Status,
            TenantId = tenantId
        };

        var resultId = await _mediator.Send(command);
        await _hub.Clients.Group($"tenant:{tenantId}").SendAsync("LocationUpdated", new { Id = resultId, req.ShipmentNumber, req.CurrentLocation, req.Latitude, req.Longitude, req.Status, Timestamp = DateTime.UtcNow });
        return Ok(new { Message = "Konum güncellendi!", Id = resultId });
    }
}
