using System;
using LogisticsPlatform.BuildingBlocks.Domain;

namespace LogisticsPlatform.Modules.Orders.Domain;

public class OrderItem : Entity<Guid>
{
    public Guid OrderId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal WeightKg { get; set; }
    public decimal VolumeM3 { get; set; }
    public int Quantity { get; set; }
}
