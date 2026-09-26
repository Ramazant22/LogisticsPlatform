using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using LogisticsPlatform.Modules.Reporting.Application;
using LogisticsPlatform.Modules.Reporting.Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace LogisticsPlatform.API.Controllers;

public class GenerateReportRequest
{
    public int TotalShipments { get; set; }
    public int DeliveredShipments { get; set; }
    public int ActiveVehicles { get; set; }
    public decimal TotalRevenue { get; set; }
}

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "ReportRead")]
public class ReportingController : TenantControllerBase
{
    private readonly IMediator _mediator;
    private readonly ReportingDbContext _context;

    public ReportingController(IMediator mediator, ReportingDbContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        // En son oluşturulan raporları getir
        if (!TryGetTenantId(out var tenantId)) return Forbid();
        var reports = await _context.DailyReports.Where(item => item.TenantId == tenantId).OrderByDescending(x => x.ReportDate).Take(10).ToListAsync();
        return Ok(reports);
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] GenerateReportRequest req)
    {
        if (!TryGetTenantId(out var tenantId)) return Forbid();
        var command = new GenerateReportCommand
        {
            TotalShipments = req.TotalShipments,
            DeliveredShipments = req.DeliveredShipments,
            ActiveVehicles = req.ActiveVehicles,
            TotalRevenue = req.TotalRevenue,
            TenantId = tenantId
        };

        var resultId = await _mediator.Send(command);
        return Ok(new { Message = "Günlük rapor başarıyla oluşturuldu!", Id = resultId });
    }
}
