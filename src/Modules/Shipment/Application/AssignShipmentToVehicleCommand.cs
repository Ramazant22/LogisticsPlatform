using LogisticsPlatform.BuildingBlocks.CQRS;
using LogisticsPlatform.BuildingBlocks.Domain;
using LogisticsPlatform.Modules.Shipment.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPlatform.Modules.Shipment.Application;

public record AssignShipmentToVehicleCommand(Guid ShipmentId, Guid VehicleId) : ICommand<bool>;

public class AssignShipmentToVehicleCommandHandler : ICommandHandler<AssignShipmentToVehicleCommand, bool>
{
    private readonly ShipmentDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public AssignShipmentToVehicleCommandHandler(ShipmentDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(AssignShipmentToVehicleCommand request, CancellationToken cancellationToken)
    {
        // Ýlgili kargoyu bul. HasQueryFilter zaten baþka þirketin kargosunu görmemizi engelliyor.
        var shipment = await _context.Shipments
            .FirstOrDefaultAsync(s => s.Id == request.ShipmentId, cancellationToken);

        if (shipment == null)
        {
            throw new Exception("Kargo bulunamadý veya bu iþlem için yetkiniz yok.");
        }

        if (shipment.Status != "Pending")
        {
            throw new Exception("Sadece 'Beklemede' (Pending) olan kargolar bir araca atanabilir.");
        }

        // Entity üzerindeki iþ kuralý metodunu çaðýr
        shipment.AssignToVehicle(request.VehicleId);

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
