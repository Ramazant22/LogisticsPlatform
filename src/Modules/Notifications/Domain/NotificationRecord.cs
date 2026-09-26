using System;
namespace LogisticsPlatform.Modules.Notifications.Domain;
public class NotificationRecord {
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public bool IsSent { get; set; }

    public NotificationRecord() {}
    public NotificationRecord(string title, string message, Guid userId) {
        Id = Guid.NewGuid(); Title = title; Message = message; UserId = userId;
    }
    public NotificationRecord(Guid userId, string title, string message) {
        Id = Guid.NewGuid(); Title = title; Message = message; UserId = userId;
    }
    public void MarkAsSent() { IsSent = true; }
}
