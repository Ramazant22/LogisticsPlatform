using System;

namespace LogisticsPlatform.Modules.Fleet.Domain.Entities;

public class Vehicle
{
    public Guid Id { get; set; }
    public string LicensePlate { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public int ModelYear { get; set; } 
    public int CapacityInKg { get; set; } // int yapıldı
    public int CurrentMileage { get; set; } // int yapıldı
    public string FuelType { get; set; } = "Dizel"; 
    public string Status { get; set; } = "Aktif";
    public Guid TenantId { get; set; }
}
