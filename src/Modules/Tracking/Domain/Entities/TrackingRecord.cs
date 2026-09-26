using System;

namespace LogisticsPlatform.Modules.Tracking.Domain.Entities;

public class TrackingRecord
{
    public Guid Id { get; set; }
    public string ShipmentNumber { get; set; } = string.Empty; // Gönderi veya Araç No
    public string CurrentLocation { get; set; } = string.Empty; // Bulunduğu Konum
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow; // Güncelleme Zamanı
    public string Status { get; set; } = "Yolda"; 
    public Guid TenantId { get; set; }
}
