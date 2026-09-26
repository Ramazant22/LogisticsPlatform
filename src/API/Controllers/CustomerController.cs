using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using LogisticsPlatform.Modules.Customers.Application;
using LogisticsPlatform.Modules.Customers.Infrastructure;

namespace LogisticsPlatform.API.Controllers;

public class CreateCustomerRequest
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}

[ApiController]
[Route("api/[controller]")]
public class CustomerController : TenantControllerBase
{
    private readonly IMediator _mediator;
    private readonly CustomersDbContext _context;

    public CustomerController(IMediator mediator, CustomersDbContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        if (!TryGetTenantId(out var tenantId)) return Forbid();
        var customers = await _context.Customers.Where(item => item.TenantId == tenantId).ToListAsync();
        return Ok(customers);
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateCustomerRequest req)
    {
        if (!TryGetTenantId(out var tenantId)) return Forbid();
        var command = new CreateCustomerCommand
        {
            Name = req.Name,
            Email = req.Email,
            Phone = req.Phone,
            Address = req.Address,
            TenantId = tenantId
        };

        var resultId = await _mediator.Send(command);
        return Ok(new { Message = "Müşteri başarıyla eklendi!", Id = resultId });
    }
}
