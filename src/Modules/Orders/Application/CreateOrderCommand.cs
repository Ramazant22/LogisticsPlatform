using System;
using MediatR;

namespace LogisticsPlatform.Modules.Orders.Application;

public class CreateOrderCommand : IRequest<Guid>
{
    public string OrderNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public decimal WeightKg { get; set; }
    public Guid TenantId { get; set; }
}
