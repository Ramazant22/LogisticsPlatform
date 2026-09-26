using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using LogisticsPlatform.BuildingBlocks.Events;

namespace LogisticsPlatform.Modules.Notifications.Application.Consumers;

public class ShipmentDeliveredNotificationConsumer : IConsumer<ShipmentDeliveredEvent>
{
    private readonly ILogger<ShipmentDeliveredNotificationConsumer> _logger;

    public ShipmentDeliveredNotificationConsumer(ILogger<ShipmentDeliveredNotificationConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<ShipmentDeliveredEvent> context)
    {
        _logger.LogInformation($"---> [BÝLDÝRÝM] Kargo {context.Message.ShipmentId} teslim edildi! Müþteriye bilgi mesajý gönderildi.");
        return Task.CompletedTask;
    }
}
