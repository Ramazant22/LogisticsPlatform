using System.Threading.Tasks;
using MassTransit;
using MediatR;
using LogisticsPlatform.BuildingBlocks.Events;
using LogisticsPlatform.Modules.Billing.Application;

namespace LogisticsPlatform.Modules.Billing.Application.Consumers;

public class ShipmentDeliveredEventConsumer : IConsumer<ShipmentDeliveredEvent>
{
    private readonly IMediator _mediator;

    public ShipmentDeliveredEventConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<ShipmentDeliveredEvent> context)
    {
        var message = context.Message;
        
        var command = new GenerateInvoiceCommand
        {
            OrderId = message.OrderId,
            CustomerId = message.CustomerId,
            Amount = message.FinalAmount
        };

        // CQRS üzerinden faturayý veritabanýna yaz
        await _mediator.Send(command);
    }
}
