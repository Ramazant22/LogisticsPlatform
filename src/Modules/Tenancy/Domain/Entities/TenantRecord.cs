using System;

namespace LogisticsPlatform.Modules.Tenancy.Domain.Entities;

public class TenantRecord
{
    public Guid Id { get; set; }
    public string CompanyName { get; set; } = string.Empty; // Lojistik Firmasının Adı
    public string SubscriptionPlan { get; set; } = "Standart"; // Standart, Premium, Enterprise
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
