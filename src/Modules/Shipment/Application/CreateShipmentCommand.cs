using System;
using MediatR;

namespace LogisticsPlatform.Modules.Shipment.Application;

public class CreateShipmentCommand : IRequest<Guid>
{
    public string TrackingNumber { get; set; } = string.Empty;
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public Guid TenantId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid OrderId { get; set; }
    public decimal FinalAmount { get; set; }
}
