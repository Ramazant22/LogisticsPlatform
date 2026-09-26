using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using LogisticsPlatform.Modules.Fleet.Application;
using LogisticsPlatform.Modules.Fleet.Infrastructure;

namespace LogisticsPlatform.API.Controllers;

public class CreateVehicleRequest
{
    public string LicensePlate { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
}

[ApiController]
[Route("api/[controller]")]
public class FleetController : TenantControllerBase
{
    private readonly IMediator _mediator;
    private readonly FleetDbContext _context;

    public FleetController(IMediator mediator, FleetDbContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        if (!TryGetTenantId(out var tenantId)) return Forbid();
        var vehicles = await _context.Vehicles.Where(item => item.TenantId == tenantId).ToListAsync();
        return Ok(vehicles);
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateVehicleRequest req)
    {
        if (!TryGetTenantId(out var tenantId)) return Forbid();
        var command = new CreateVehicleCommand
        {
            LicensePlate = req.LicensePlate,
            Brand = req.Brand,
            Model = req.Model,
            Year = req.Year,
            ModelYear = req.Year, // Basitlik için Year değerini kullanıyoruz
            CapacityInKg = 1500, // Şimdilik varsayılan bir değer
            FuelType = "Dizel", // Şimdilik varsayılan bir değer
            TenantId = tenantId
        };

        var resultId = await _mediator.Send(command);
        return Ok(new { Message = "Araç başarıyla eklendi!", Id = resultId });
    }
}

