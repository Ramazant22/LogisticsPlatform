using System;

namespace LogisticsPlatform.Modules.Reporting.Domain.Entities;

public class DailyReport
{
    public Guid Id { get; set; }
    public DateTime ReportDate { get; set; } = DateTime.UtcNow.Date;
    public int TotalShipments { get; set; } // Toplam Gönderi
    public int DeliveredShipments { get; set; } // Teslim Edilen
    public int ActiveVehicles { get; set; } // Aktif Araç Sayısı
    public decimal TotalRevenue { get; set; } // Toplam Gelir
    public Guid TenantId { get; set; }
}
