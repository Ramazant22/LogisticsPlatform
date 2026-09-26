using System;
using LogisticsPlatform.BuildingBlocks.Domain;

namespace LogisticsPlatform.Modules.Billing.Domain;

public class Invoice : Entity<Guid>
{
    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; } = "Unpaid";

    public Invoice() { }

    public Invoice(Guid id, Guid orderId, Guid customerId, decimal amount)
    {
        Id = id;
        OrderId = orderId;
        CustomerId = customerId;
        Amount = amount;
        Status = "Unpaid";
        CreatedAt = DateTime.UtcNow;
    }
}
