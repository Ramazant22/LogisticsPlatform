using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using LogisticsPlatform.Modules.Shipment.Infrastructure;

namespace LogisticsPlatform.Modules.Shipment.Application;

public class CreateShipmentCommandHandler : IRequestHandler<CreateShipmentCommand, Guid>
{
    private readonly ShipmentDbContext _context;

    public CreateShipmentCommandHandler(ShipmentDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateShipmentCommand request, CancellationToken cancellationToken)
    {
        var shipment = new LogisticsPlatform.Modules.Shipment.Domain.Shipment
        {
            Id = Guid.NewGuid(),
            TrackingNumber = request.TrackingNumber,
            Origin = request.Origin,
            Destination = request.Destination,
            Status = "Hazırlanıyor",
            TenantId = request.TenantId,
            CustomerId = request.CustomerId,
            OrderId = request.OrderId,
            FinalAmount = request.FinalAmount
        };

        _context.Shipments.Add(shipment);
        await _context.SaveChangesAsync(cancellationToken);

        return shipment.Id;
    }
}
