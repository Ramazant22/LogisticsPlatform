using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using LogisticsPlatform.Modules.Fleet.Domain.Entities;
using LogisticsPlatform.Modules.Fleet.Infrastructure;

namespace LogisticsPlatform.Modules.Fleet.Application;

public class CreateVehicleCommandHandler : IRequestHandler<CreateVehicleCommand, Guid>
{
    private readonly FleetDbContext _context;

    public CreateVehicleCommandHandler(FleetDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            LicensePlate = request.LicensePlate,
            Brand = request.Brand,
            Model = request.Model,
            Year = request.Year,
            ModelYear = request.ModelYear, // Varlığa (Entity) aktarıyoruz
            CapacityInKg = request.CapacityInKg, // Varlığa (Entity) aktarıyoruz
            FuelType = request.FuelType,
            CurrentMileage = 0,
            Status = "Aktif",
            TenantId = request.TenantId
        };

        _context.Vehicles.Add(vehicle);
        await _context.SaveChangesAsync(cancellationToken);

        return vehicle.Id;
    }
}
