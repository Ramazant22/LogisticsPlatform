using System;

namespace LogisticsPlatform.BuildingBlocks.Events;

public class ShipmentDeliveredEvent
{
    public Guid ShipmentId { get; set; }
    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
    public decimal FinalAmount { get; set; }

    // Boþ Constructor (MassTransit / RabbitMQ serileþtirmesi için þart)
    public ShipmentDeliveredEvent() { }

    // Shipment Modülünün kullandýðý 4 parametreli Constructor
    public ShipmentDeliveredEvent(Guid shipmentId, Guid orderId, Guid customerId, decimal finalAmount)
    {
        ShipmentId = shipmentId;
        OrderId = orderId;
        CustomerId = customerId;
        FinalAmount = finalAmount;
    }
}
