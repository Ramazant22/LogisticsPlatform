using System;
using MediatR;

namespace LogisticsPlatform.Modules.Billing.Application;

public class CreateBillingCommand : IRequest<Guid>
{
    public string InvoiceNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public Guid TenantId { get; set; }
}
