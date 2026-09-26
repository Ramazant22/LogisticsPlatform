using LogisticsPlatform.BuildingBlocks.CQRS;
using LogisticsPlatform.Modules.Fleet.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPlatform.Modules.Fleet.Application;

public record GetAllVehiclesQuery() : IQuery<List<VehicleDto>>;

// API'den dýþarý çýkacak olan veri transfer objesi (Domain Entity'i gizliyoruz)
public record VehicleDto(Guid Id, string LicensePlate, string Brand, string Model, int ModelYear, decimal CapacityInKg, string Status, int CurrentMileage);

public class GetAllVehiclesQueryHandler : IQueryHandler<GetAllVehiclesQuery, List<VehicleDto>>
{
    private readonly FleetDbContext _context;

    public GetAllVehiclesQueryHandler(FleetDbContext context)
    {
        _context = context;
    }

    public async Task<List<VehicleDto>> Handle(GetAllVehiclesQuery request, CancellationToken cancellationToken)
    {
        // MÝMARÝNÝN GÜCÜ BURADA:
        // Sorguda hiçbir TenantId filtrelemesi YOK!
        // Fakat FleetDbContext'teki HasQueryFilter sayesinde arka planda sadece 
        // Token'ý gönderen kullanýcýnýn þirketine (TenantId) ait araçlar çekilecek.
        return await _context.Vehicles
            .AsNoTracking()
            .Select(v => new VehicleDto(
                v.Id, 
                v.LicensePlate, 
                v.Brand, 
                v.Model, 
                v.ModelYear, 
                v.CapacityInKg, 
                v.Status, 
                v.CurrentMileage))
            .ToListAsync(cancellationToken);
    }
}
