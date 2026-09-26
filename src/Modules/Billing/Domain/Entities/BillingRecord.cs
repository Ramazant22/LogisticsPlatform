using System;

namespace LogisticsPlatform.Modules.Billing.Domain.Entities;

public class BillingRecord
{
    public Guid Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty; // Fatura No
    public string CustomerName { get; set; } = string.Empty; // Müşteri Adı
    public decimal Amount { get; set; } // Tutar
    public string Status { get; set; } = "Ödenmedi"; // Fatura Durumu
    public Guid TenantId { get; set; }
}
