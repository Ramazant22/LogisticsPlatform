using System;
using MediatR;

namespace LogisticsPlatform.Modules.Fleet.Application;

public class CreateVehicleCommand : IRequest<Guid>
{
    public string LicensePlate { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public int ModelYear { get; set; } 
    public int CapacityInKg { get; set; } // int yapıldı
    public string FuelType { get; set; } = "Dizel";
    public Guid TenantId { get; set; }
}
