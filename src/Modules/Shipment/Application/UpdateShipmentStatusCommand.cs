using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using LogisticsPlatform.Modules.Shipment.Infrastructure;
using LogisticsPlatform.BuildingBlocks.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPlatform.Modules.Shipment.Application;

public class UpdateShipmentStatusCommand : IRequest<bool>
{
    public Guid ShipmentId { get; set; }
    public string NewStatus { get; set; } = string.Empty;
}

public class UpdateShipmentStatusCommandHandler : IRequestHandler<UpdateShipmentStatusCommand, bool>
{
    private readonly ShipmentDbContext _context;
    private readonly IPublishEndpoint _publishEndpoint;

    public UpdateShipmentStatusCommandHandler(ShipmentDbContext context, IPublishEndpoint publishEndpoint)
    {
        _context = context;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<bool> Handle(UpdateShipmentStatusCommand request, CancellationToken cancellationToken)
    {
        var shipment = await _context.Shipments.FirstOrDefaultAsync(s => s.Id == request.ShipmentId, cancellationToken);
        
        if (shipment == null)
            return false;

        shipment.Status = request.NewStatus;
        await _context.SaveChangesAsync(cancellationToken);

        if (request.NewStatus is "Delivered" or "Teslim Edildi")
        {
            var deliveredEvent = new ShipmentDeliveredEvent(
                request.ShipmentId, 
                shipment.OrderId,
                shipment.CustomerId,
                shipment.FinalAmount
            );

            await _publishEndpoint.Publish(deliveredEvent, cancellationToken);
        }

        return true;
    }
}
