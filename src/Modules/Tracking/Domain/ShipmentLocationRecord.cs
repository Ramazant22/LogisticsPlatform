using System;
namespace LogisticsPlatform.Modules.Tracking.Domain;
public class ShipmentLocationRecord {
    public Guid Id { get; set; }
    public Guid ShipmentId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime Timestamp { get; set; }

    public ShipmentLocationRecord() {}
    public ShipmentLocationRecord(Guid shipmentId, double latitude, double longitude, DateTime timestamp) {
        Id = Guid.NewGuid();
        ShipmentId = shipmentId;
        Latitude = latitude;
        Longitude = longitude;
        Timestamp = timestamp;
    }
}
