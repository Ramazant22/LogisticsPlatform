using System;

namespace LogisticsPlatform.Modules.Identity.Domain.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    
    // Şifreyi asla düz metin olarak tutmayız, Hash (karma) olarak saklarız.
    public string PasswordHash { get; set; } = string.Empty;
    
    public string Role { get; set; } = "User"; // Varsayılan rol
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
