using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using LogisticsPlatform.Modules.Shipment.Application;
using LogisticsPlatform.Modules.Shipment.Infrastructure;
using LogisticsPlatform.Modules.Tracking.Application.Hubs;

namespace LogisticsPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShipmentsController : TenantControllerBase
{
    private readonly IMediator _mediator;
    private readonly ShipmentDbContext _context;
    private readonly IHubContext<TrackingHub> _hub;
    private readonly IDistributedCache _cache;

    public ShipmentsController(IMediator mediator, ShipmentDbContext context, IHubContext<TrackingHub> hub, IDistributedCache cache)
    {
        _mediator = mediator;
        _context = context;
        _hub = hub;
        _cache = cache;
    }

    [HttpPost]
    public async Task<IActionResult> CreateShipment([FromBody] CreateShipmentCommand command)
    {
        if (!TryGetTenantId(out var tenantId)) return Forbid();
        command.TenantId = tenantId;
        var shipmentId = await _mediator.Send(command);
        await InvalidateDashboardAsync(tenantId, cancellationToken);
        return Ok(new { ShipmentId = shipmentId });
    }

    [HttpPost("dispatch")]
    public async Task<IActionResult> StartDispatch([FromBody] StartDispatchCommand command)
    {
        if (!TryGetTenantId(out var tenantId) || !await _context.Shipments.AnyAsync(item => item.Id == command.ShipmentId && item.TenantId == tenantId)) return NotFound();
        var result = await _mediator.Send(command);
        if (!result) return BadRequest("Sevkiyat başlatılamadı.");
        await InvalidateDashboardAsync(tenantId, HttpContext.RequestAborted);
        await _hub.Clients.Group($"tenant:{tenantId}").SendAsync("ShipmentStatusChanged", new { command.ShipmentId, Status = "Yolda", command.VehicleId, command.DriverId, command.RouteId, Timestamp = DateTime.UtcNow });
        return Ok(new { Message = "Sevkiyat başlatıldı." });
    }

    // Sorun çıkaran Action ve VehicleId alanları kaldırılıp, yeni NewStatus yapısına uyarlandı
    [HttpPost("update-status")]
    public async Task<IActionResult> UpdateStatus([FromBody] UpdateShipmentStatusCommand command)
    {
        if (!TryGetTenantId(out var tenantId) || !await _context.Shipments.AnyAsync(item => item.Id == command.ShipmentId && item.TenantId == tenantId)) return NotFound();
        var result = await _mediator.Send(command);
        if (!result) return BadRequest("Sevkiyat durumu güncellenemedi veya sevkiyat bulunamadı.");
        await InvalidateDashboardAsync(tenantId, HttpContext.RequestAborted);
        await _hub.Clients.Group($"tenant:{tenantId}").SendAsync("ShipmentStatusChanged", new { command.ShipmentId, Status = command.NewStatus, Timestamp = DateTime.UtcNow });
        return Ok(new { Message = $"Sevkiyat durumu '{command.NewStatus}' olarak güncellendi." });
    }

    private Task InvalidateDashboardAsync(Guid tenantId, CancellationToken cancellationToken)
        => _cache.RemoveAsync($"dashboard:summary:{tenantId}", cancellationToken);
}
