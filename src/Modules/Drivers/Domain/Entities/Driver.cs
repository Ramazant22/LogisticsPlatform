using System;

namespace LogisticsPlatform.Modules.Drivers.Domain.Entities;

public class Driver
{
    public Guid Id { get; set; }
    
    // Frontend tarafı için kullandığımız alanlar
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string LicenseType { get; set; } = string.Empty;

    // Arka plan CQRS (CreateDriverCommand) yapısının beklediği zorunlu alanlar
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string LicenseClass { get; set; } = string.Empty;
    
    // Hata buradan kaynaklanıyordu, string yerine Guid olmalı!
    public Guid TenantId { get; set; } 
}
