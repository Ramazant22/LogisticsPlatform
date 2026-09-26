using System;

namespace LogisticsPlatform.Modules.Routes.Domain.Entities;

public class DeliveryRoute
{
    public Guid Id { get; set; }
    public string RouteName { get; set; } = string.Empty; // Rota Adı
    public string StartLocation { get; set; } = string.Empty; // Başlangıç
    public string EndLocation { get; set; } = string.Empty; // Bitiş
    public decimal DistanceKm { get; set; } // Mesafe (KM)
    public string Status { get; set; } = "Aktif"; // Rota Durumu
    public Guid TenantId { get; set; }
}
