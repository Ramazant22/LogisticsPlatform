using System;
using MediatR;

namespace LogisticsPlatform.Modules.Notifications.Application;

public class SendNotificationCommand : IRequest<Guid>
{
    public string Recipient { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public Guid TenantId { get; set; }
}
