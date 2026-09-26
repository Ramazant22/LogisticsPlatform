using System;

namespace LogisticsPlatform.Modules.Shipment.Domain;

public class Shipment
{
    public Guid Id { get; set; }
    public string TrackingNumber { get; set; } = string.Empty;
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public string Status { get; set; } = "Hazırlanıyor";
    
    // Sistem konfigürasyonunun beklediği ek alanlar
    public string Description { get; set; } = string.Empty;
    public decimal WeightInKg { get; set; }
    public Guid? AssignedVehicleId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid OrderId { get; set; }
    public Guid? AssignedDriverId { get; set; }
    public Guid? RouteId { get; set; }
    public decimal FinalAmount { get; set; }
    
    public Guid TenantId { get; set; }

    // Komutların (AssignShipmentCommand) çağırdığı metot
    public void AssignToVehicle(Guid vehicleId)
    {
        AssignedVehicleId = vehicleId;
        Status = "Araca Atandı";
    }
}
