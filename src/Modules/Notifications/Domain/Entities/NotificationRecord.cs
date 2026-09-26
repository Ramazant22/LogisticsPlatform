using System;

namespace LogisticsPlatform.Modules.Notifications.Domain.Entities;

public class NotificationRecord
{
    public Guid Id { get; set; }
    public string Recipient { get; set; } = string.Empty; // Alıcı (Müşteri/Sürücü)
    public string Title { get; set; } = string.Empty; // Başlık
    public string Message { get; set; } = string.Empty; // Mesaj İçeriği
    public bool IsRead { get; set; } = false; // Okundu mu?
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid TenantId { get; set; }
}
