using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using LogisticsPlatform.Modules.Shipment.Application;
using LogisticsPlatform.Modules.Shipment.Infrastructure;

namespace LogisticsPlatform.API.Controllers;

public class CreateShipmentRequest
{
    public string TrackingNumber { get; set; } = string.Empty;
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
}

[ApiController]
[Route("api/[controller]")]
public class ShipmentController : TenantControllerBase
{
    private readonly IMediator _mediator;
    private readonly ShipmentDbContext _context;

    public ShipmentController(IMediator mediator, ShipmentDbContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        if (!TryGetTenantId(out var tenantId)) return Forbid();
        var shipments = await _context.Shipments.Where(item => item.TenantId == tenantId).ToListAsync();
        return Ok(shipments);
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateShipmentRequest req)
    {
        if (!TryGetTenantId(out var tenantId)) return Forbid();
        var command = new CreateShipmentCommand
        {
            TrackingNumber = req.TrackingNumber,
            Origin = req.Origin,
            Destination = req.Destination,
            TenantId = tenantId
        };

        var resultId = await _mediator.Send(command);
        return Ok(new { Message = "Gönderi başarıyla oluşturuldu!", Id = resultId });
    }
}
