using System;
namespace LogisticsPlatform.Modules.Tenancy.Domain;
public class Tenant {
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Identifier { get; set; } = string.Empty;
    public string SubscriptionPlan { get; set; } = "Starter";
    public bool IsActive { get; set; } = true;

    public static Tenant Create(string name, string identifier, string subscriptionPlan) {
        return new Tenant { Id = Guid.NewGuid(), Name = name, Identifier = identifier, SubscriptionPlan = subscriptionPlan, IsActive = true };
    }
}
