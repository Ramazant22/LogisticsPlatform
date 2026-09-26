using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using LogisticsPlatform.Modules.Notifications.Domain.Entities;
using LogisticsPlatform.Modules.Notifications.Infrastructure;

namespace LogisticsPlatform.Modules.Notifications.Application;

public class SendNotificationCommandHandler : IRequestHandler<SendNotificationCommand, Guid>
{
    private readonly NotificationsDbContext _context;

    public SendNotificationCommandHandler(NotificationsDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(SendNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = new NotificationRecord
        {
            Id = Guid.NewGuid(),
            Recipient = request.Recipient,
            Title = request.Title,
            Message = request.Message,
            IsRead = false,
            CreatedAt = DateTime.UtcNow,
            TenantId = request.TenantId
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync(cancellationToken);

        return notification.Id;
    }
}
