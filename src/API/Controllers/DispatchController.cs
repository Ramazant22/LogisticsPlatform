using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LogisticsPlatform.Modules.Shipment.Application;
using LogisticsPlatform.Modules.Shipment.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "OperationsManage")]
public class DispatchController : TenantControllerBase
{
    private readonly IMediator _mediator;
    private readonly ShipmentDbContext _shipments;

    public DispatchController(IMediator mediator, ShipmentDbContext shipments)
    {
        _mediator = mediator;
        _shipments = shipments;
    }

    [HttpPost]
    public async Task<IActionResult> StartDispatch([FromBody] StartDispatchCommand command)
    {
        if (!TryGetTenantId(out var tenantId) || command.VehicleId == Guid.Empty || !await _shipments.Shipments.AnyAsync(item => item.Id == command.ShipmentId && item.TenantId == tenantId))
            return NotFound();
        var result = await _mediator.Send(command);
        return result ? Ok(new { Success = true, Message = "Atama yapıldı, araç yola çıktı." }) : BadRequest("Sevkiyat başlatılamadı.");
    }
}
