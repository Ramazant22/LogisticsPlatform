using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using LogisticsPlatform.Modules.Shipment.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPlatform.Modules.Shipment.Application;

public class StartDispatchCommandHandler : IRequestHandler<StartDispatchCommand, bool>
{
    private readonly ShipmentDbContext _context;

    public StartDispatchCommandHandler(ShipmentDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(StartDispatchCommand request, CancellationToken cancellationToken)
    {
        var shipment = await _context.Shipments
            .FirstOrDefaultAsync(item => item.Id == request.ShipmentId, cancellationToken);

        if (shipment is null || shipment.Status is "Teslim Edildi" or "Cancelled")
            return false;

        shipment.AssignToVehicle(request.VehicleId);
        shipment.AssignedDriverId = request.DriverId;
        shipment.RouteId = request.RouteId;
        shipment.Status = "Yolda";
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
