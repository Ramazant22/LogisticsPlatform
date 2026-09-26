using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using LogisticsPlatform.Modules.Orders.Application;

namespace LogisticsPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
    {
        var orderId = await _mediator.Send(command);
        return Ok(new { OrderId = orderId, Message = "Sipariþ oluþturuldu. (Yakýnda Event fýrlatarak bunu Shipment'a çevireceðiz!)" });
    }
}
