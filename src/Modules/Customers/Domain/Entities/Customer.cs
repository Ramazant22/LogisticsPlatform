using System;

namespace LogisticsPlatform.Modules.Customers.Domain.Entities;

public class Customer
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty; // Şirket / Müşteri Adı
    public string Email { get; set; } = string.Empty; // E-posta
    public string Phone { get; set; } = string.Empty; // Telefon
    public string Address { get; set; } = string.Empty; // Adres
    public Guid TenantId { get; set; }
}
