namespace LogisticsPlatform.Modules.Identity.Domain;

public class UserRole
{
    public Guid UserId { get; private set; }
    public Guid RoleId { get; private set; }

    // Navigation Properties
    public User User { get; private set; }
    public Role Role { get; private set; }

    private UserRole() { } // EF Core için

    public static UserRole Create(Guid userId, Guid roleId)
    {
        return new UserRole
        {
            UserId = userId,
            RoleId = roleId
        };
    }
}
