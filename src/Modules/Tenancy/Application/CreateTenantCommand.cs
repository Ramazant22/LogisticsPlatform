using System;
using MediatR;

namespace LogisticsPlatform.Modules.Tenancy.Application;

public class CreateTenantCommand : IRequest<Guid>
{
    public string CompanyName { get; set; } = string.Empty;
    public string SubscriptionPlan { get; set; } = "Standart";
}
