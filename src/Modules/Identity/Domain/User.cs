using System;
namespace LogisticsPlatform.Modules.Identity.Domain;
public class User {
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "Operasyon Sorumlusu";
    public bool IsActive { get; set; } = true;

    // Handler içindeki farklı parametre sıralamalarına yakalanmamak için
    // TenantId alan bir Create fonksiyonu daha:
    public static User Create(Guid tenantId, string firstName, string lastName, string email, string passwordHash, string role) {
        return new User { Id = Guid.NewGuid(), TenantId = tenantId, FirstName = firstName, LastName = lastName, Email = email, PasswordHash = passwordHash, Role = role, IsActive = true };
    }

    public static User Create(string firstName, string lastName, string email, string passwordHash, string role) {
        return new User { Id = Guid.NewGuid(), FirstName = firstName, LastName = lastName, Email = email, PasswordHash = passwordHash, Role = role, IsActive = true };
    }
}
