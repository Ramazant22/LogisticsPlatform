using System;
using System.Threading.Tasks;
using MassTransit;
using LogisticsPlatform.BuildingBlocks.Events;

namespace LogisticsPlatform.Modules.Notifications.Application.Consumers;

public class VehicleMaintenanceDueEventConsumer : IConsumer<VehicleMaintenanceDueEvent>
{
    public Task Consume(ConsumeContext<VehicleMaintenanceDueEvent> context)
    {
        var message = context.Message;
        
        // Terminalde bildirimi net bir þekilde görebilmemiz için
        Console.WriteLine($"\n[BÝLDÝRÝM MODÜLÜ] ?? DÝKKAT: {message.PlateNumber} plakalý aracýn periyodik bakým zamaný gelmiþtir!\n");
        
        return Task.CompletedTask;
    }
}
