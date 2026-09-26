using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using LogisticsPlatform.Modules.Drivers.Application;
using LogisticsPlatform.Modules.Drivers.Infrastructure;

namespace LogisticsPlatform.API.Controllers;

public class CreateDriverRequest
{
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string LicenseType { get; set; } = string.Empty;
}

[ApiController]
[Route("api/[controller]")]
public class DriverController : TenantControllerBase
{
    private readonly IMediator _mediator;
    private readonly DriversDbContext _context;

    public DriverController(IMediator mediator, DriversDbContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        if (!TryGetTenantId(out var tenantId)) return Forbid();
        var drivers = await _context.Drivers.Where(item => item.TenantId == tenantId).ToListAsync();
        return Ok(drivers);
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateDriverRequest req)
    {
        if (!TryGetTenantId(out var tenantId)) return Forbid();
        var names = req.FullName.Split(' ');
        var firstName = names[0];
        var lastName = names.Length > 1 ? string.Join(" ", names, 1, names.Length - 1) : "";

        var command = new CreateDriverCommand
        {
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = req.PhoneNumber, // API'den gelen telefon numarasını Command'e veriyoruz
            LicenseClass = req.LicenseType,
            TenantId = tenantId
        };

        var resultId = await _mediator.Send(command);
        return Ok(new { Message = "Sürücü başarıyla eklendi!", Id = resultId });
    }
}
