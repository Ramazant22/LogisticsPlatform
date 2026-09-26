using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using LogisticsPlatform.Modules.Billing.Application;
using LogisticsPlatform.Modules.Billing.Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace LogisticsPlatform.API.Controllers;

public class CreateBillingRequest
{
    public string InvoiceNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "BillingManage")]
public class BillingController : TenantControllerBase
{
    private readonly IMediator _mediator;
    private readonly BillingDbContext _context;

    public BillingController(IMediator mediator, BillingDbContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        if (!TryGetTenantId(out var tenantId)) return Forbid();
        var records = await _context.BillingRecords.Where(item => item.TenantId == tenantId).ToListAsync();
        return Ok(records);
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateBillingRequest req)
    {
        if (!TryGetTenantId(out var tenantId)) return Forbid();
        var command = new CreateBillingCommand
        {
            InvoiceNumber = req.InvoiceNumber,
            CustomerName = req.CustomerName,
            Amount = req.Amount,
            TenantId = tenantId
        };

        var resultId = await _mediator.Send(command);
        return Ok(new { Message = "Fatura başarıyla oluşturuldu!", Id = resultId });
    }
}
