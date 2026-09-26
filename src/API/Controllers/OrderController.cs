using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using LogisticsPlatform.Modules.Orders.Application;
using LogisticsPlatform.Modules.Orders.Infrastructure;

namespace LogisticsPlatform.API.Controllers;

public class CreateOrderRequest
{
    public string OrderNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public decimal WeightKg { get; set; }
}

[ApiController]
[Route("api/[controller]")]
public class OrderController : TenantControllerBase
{
    private readonly IMediator _mediator;
    private readonly OrdersDbContext _context;

    public OrderController(IMediator mediator, OrdersDbContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        if (!TryGetTenantId(out var tenantId)) return Forbid();
        var orders = await _context.Orders.Where(item => item.TenantId == tenantId).ToListAsync();
        return Ok(orders);
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateOrderRequest req)
    {
        if (!TryGetTenantId(out var tenantId)) return Forbid();
        var command = new CreateOrderCommand
        {
            OrderNumber = req.OrderNumber,
            CustomerName = req.CustomerName,
            Destination = req.Destination,
            WeightKg = req.WeightKg,
            TenantId = tenantId
        };

        var resultId = await _mediator.Send(command);
        return Ok(new { Message = "Sipariş başarıyla oluşturuldu!", Id = resultId });
    }
}
