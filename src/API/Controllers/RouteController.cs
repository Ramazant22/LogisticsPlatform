using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using LogisticsPlatform.Modules.Routes.Application;
using LogisticsPlatform.Modules.Routes.Infrastructure;

namespace LogisticsPlatform.API.Controllers;

public class CreateRouteRequest
{
    public string RouteName { get; set; } = string.Empty;
    public string StartLocation { get; set; } = string.Empty;
    public string EndLocation { get; set; } = string.Empty;
    public decimal DistanceKm { get; set; }
}

[ApiController]
[Route("api/[controller]")]
public class RouteController : TenantControllerBase
{
    private readonly IMediator _mediator;
    private readonly RoutesDbContext _context;

    public RouteController(IMediator mediator, RoutesDbContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        if (!TryGetTenantId(out var tenantId)) return Forbid();
        var routes = await _context.Routes.Where(item => item.TenantId == tenantId).ToListAsync();
        return Ok(routes);
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateRouteRequest req)
    {
        if (!TryGetTenantId(out var tenantId)) return Forbid();
        var command = new CreateRouteCommand
        {
            RouteName = req.RouteName,
            StartLocation = req.StartLocation,
            EndLocation = req.EndLocation,
            DistanceKm = req.DistanceKm,
            TenantId = tenantId
        };

        var resultId = await _mediator.Send(command);
        return Ok(new { Message = "Rota başarıyla oluşturuldu!", Id = resultId });
    }
}
