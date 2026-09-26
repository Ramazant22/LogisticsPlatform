using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using LogisticsPlatform.Modules.Orders.Domain.Entities;
using LogisticsPlatform.Modules.Orders.Infrastructure;

namespace LogisticsPlatform.Modules.Orders.Application;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly OrdersDbContext _context;

    public CreateOrderCommandHandler(OrdersDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            OrderNumber = request.OrderNumber,
            CustomerName = request.CustomerName,
            Customer = request.CustomerName, // Eşitleme
            Destination = request.Destination,
            PickupLocation = "Merkez Depo", // Varsayılan
            DropoffLocation = request.Destination, // Eşitleme
            WeightKg = request.WeightKg,
            OrderDate = DateTime.UtcNow,
            Status = "Beklemede",
            TenantId = request.TenantId
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);

        return order.Id;
    }
}
