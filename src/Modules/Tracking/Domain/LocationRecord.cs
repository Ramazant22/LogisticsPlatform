using System;
namespace LogisticsPlatform.Modules.Tracking.Domain;
public class LocationRecord {
    public Guid Id { get; set; }
    public Guid ShipmentId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime Timestamp { get; set; }
}
