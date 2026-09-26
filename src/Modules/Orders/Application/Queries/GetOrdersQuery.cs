using MediatR;
using Microsoft.EntityFrameworkCore;
using LogisticsPlatform.Modules.Orders.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LogisticsPlatform.Modules.Orders.Application.Queries;

// DTO (Arayüze dönecek format)
public class OrderDto
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string Customer { get; set; } = string.Empty;
    public string Pickup { get; set; } = string.Empty;
    public string Dropoff { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class GetOrdersQuery : IRequest<List<OrderDto>> { }

public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, List<OrderDto>>
{
    private readonly OrdersDbContext _context;

    public GetOrdersQueryHandler(OrdersDbContext context)
    {
        _context = context;
    }

    public async Task<List<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        return await _context.Orders
            .Select(o => new OrderDto
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                Customer = o.Customer,
                Pickup = o.PickupLocation,
                Dropoff = o.DropoffLocation,
                Date = o.OrderDate.ToString("dd.MM.yyyy"),
                Status = o.Status
            })
            .ToListAsync(cancellationToken);
    }
}
