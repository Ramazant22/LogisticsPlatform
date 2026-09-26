using System;

namespace LogisticsPlatform.Modules.Orders.Domain.Entities;

public class Order
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string Customer { get; set; } = string.Empty; // Query'nin beklediği alan
    public string Destination { get; set; } = string.Empty;
    public string PickupLocation { get; set; } = string.Empty; // Query'nin beklediği alan
    public string DropoffLocation { get; set; } = string.Empty; // Query'nin beklediği alan
    public decimal WeightKg { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow; // Query'nin beklediği alan
    public string Status { get; set; } = "Beklemede";
    public Guid TenantId { get; set; }
}
