using System;
using LogisticsPlatform.BuildingBlocks.Domain;

namespace LogisticsPlatform.Modules.Customers.Domain;

public class Customer : Entity<Guid>
{
    public string TenantId { get; set; } = string.Empty; // DbContext hatasýný çözen alan
    public string CompanyName { get; set; } = string.Empty;
    public string TaxNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;

    public Customer() { } // EF Core için boþ yapýcý

    public Customer(Guid id, string companyName, string taxNumber, string email, string phone)
    {
        Id = id;
        CompanyName = companyName;
        TaxNumber = taxNumber;
        Email = email;
        Phone = phone;
        CreatedAt = DateTime.UtcNow;
    }
}
