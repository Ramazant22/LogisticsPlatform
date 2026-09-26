using MediatR;
using Microsoft.Extensions.Logging;
using LogisticsPlatform.Modules.Fleet.Domain.Events;

namespace LogisticsPlatform.Modules.Fleet.Application.EventHandlers;

// INotificationHandler arayüzü sayesinde MediatR bu sýnýfýn bir "Dinleyici" olduðunu anlar
public class VehicleCreatedDomainEventHandler : INotificationHandler<VehicleCreatedDomainEvent>
{
    private readonly ILogger<VehicleCreatedDomainEventHandler> _logger;

    public VehicleCreatedDomainEventHandler(ILogger<VehicleCreatedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(VehicleCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Burada normalde Notification modülüne bir API çaðrýsý yapýlýr veya Kafka/RabbitMQ'ya mesaj atýlýr.
        _logger.LogWarning("=========================================================");
        _logger.LogWarning($"[SÝSTEM BÝLDÝRÝMÝ] YENÝ ARAÇ FÝLOYA KATILDI!");
        _logger.LogWarning($"Araç Plakasý: {notification.LicensePlate} | Araç ID: {notification.VehicleId}");
        _logger.LogWarning("=========================================================");
        
        return Task.CompletedTask;
    }
}
