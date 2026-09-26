using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using LogisticsPlatform.Modules.Tenancy.Application;
using LogisticsPlatform.Modules.Tenancy.Infrastructure;

namespace LogisticsPlatform.API.Controllers;

public class CreateTenantRequest
{
    public string CompanyName { get; set; } = string.Empty;
    public string SubscriptionPlan { get; set; } = "Standart";
}

[ApiController]
[Route("api/[controller]")]
public class TenantController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly TenancyDbContext _context;

    public TenantController(IMediator mediator, TenancyDbContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var tenants = await _context.Tenants.OrderByDescending(x => x.CreatedAt).ToListAsync();
        return Ok(tenants);
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateTenantRequest req)
    {
        var command = new CreateTenantCommand
        {
            CompanyName = req.CompanyName,
            SubscriptionPlan = req.SubscriptionPlan
        };

        var resultId = await _mediator.Send(command);
        return Ok(new { Message = "SaaS Kiracı firması başarıyla eklendi!", Id = resultId });
    }
}
