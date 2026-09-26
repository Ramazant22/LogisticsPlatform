using System;
using LogisticsPlatform.BuildingBlocks.Domain;

namespace LogisticsPlatform.Modules.Orders.Domain;

public class Order : Entity<Guid>
{
    public string TenantId { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public decimal TotalWeight { get; set; }
    public string Status { get; set; } = "Pending";

    public Order() { }

    public Order(Guid id, Guid customerId, string origin, string destination, decimal totalWeight)
    {
        Id = id;
        CustomerId = customerId;
        Origin = origin;
        Destination = destination;
        TotalWeight = totalWeight;
        Status = "Pending";
        CreatedAt = DateTime.UtcNow;
    }
}
