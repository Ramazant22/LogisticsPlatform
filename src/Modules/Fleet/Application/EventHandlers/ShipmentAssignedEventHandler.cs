using MediatR;
using LogisticsPlatform.BuildingBlocks.Events;
using LogisticsPlatform.Modules.Fleet.Infrastructure;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace LogisticsPlatform.Modules.Fleet.Application.EventHandlers;

public class ShipmentAssignedEventHandler : INotificationHandler<ShipmentAssignedEvent>
{
    private readonly FleetDbContext _context;

    public ShipmentAssignedEventHandler(FleetDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ShipmentAssignedEvent notification, CancellationToken cancellationToken)
    {
        var vehicle = await _context.Vehicles.FindAsync(new object[] { notification.VehicleId }, cancellationToken);
        
        if (vehicle != null)
        {
            vehicle.Status = "InUse"; 
            await _context.SaveChangesAsync(cancellationToken);
            
            Console.WriteLine($"\n========================================================");
            Console.WriteLine($"[FLEET MODULE] EVENT YAKALANDI: Araç {vehicle.Id} durumu 'InUse' olarak güncellendi!");
            Console.WriteLine($"========================================================\n");
        }
    }
}
