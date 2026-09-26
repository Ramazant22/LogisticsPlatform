using LogisticsPlatform.BuildingBlocks.CQRS;
using LogisticsPlatform.Modules.Shipment.Infrastructure;
using FluentValidation;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using LogisticsPlatform.BuildingBlocks.Events;

namespace LogisticsPlatform.Modules.Shipment.Application;

public record AssignShipmentCommand(Guid ShipmentId, Guid VehicleId) : ICommand<bool>;

public class AssignShipmentCommandValidator : AbstractValidator<AssignShipmentCommand>
{
    public AssignShipmentCommandValidator()
    {
        RuleFor(x => x.ShipmentId).NotEmpty();
        RuleFor(x => x.VehicleId).NotEmpty();
    }
}

public class AssignShipmentCommandHandler : ICommandHandler<AssignShipmentCommand, bool>
{
    private readonly ShipmentDbContext _context;
    private readonly IPublisher _publisher;

    public AssignShipmentCommandHandler(ShipmentDbContext context, IPublisher publisher)
    {
        _context = context;
        _publisher = publisher;
    }

    public async Task<bool> Handle(AssignShipmentCommand request, CancellationToken cancellationToken)
    {
        var shipment = await _context.Shipments.FindAsync(new object[] { request.ShipmentId }, cancellationToken);
        
        if (shipment == null)
            throw new Exception("Shipment not found.");

        shipment.AssignToVehicle(request.VehicleId);
        await _context.SaveChangesAsync(cancellationToken);

        // ÝÞLEM BÝTTÝ. SÝSTEME DUYURU (EVENT) YAPIYORUZ!
        await _publisher.Publish(new ShipmentAssignedEvent(request.ShipmentId, request.VehicleId), cancellationToken);

        return true;
    }
}
