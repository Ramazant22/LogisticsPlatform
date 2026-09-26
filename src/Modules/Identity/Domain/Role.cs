using LogisticsPlatform.BuildingBlocks.Domain;

namespace LogisticsPlatform.Modules.Identity.Domain;

public class Role : Entity<Guid>, IAggregateRoot
{
    public string Name { get; private set; }
    public string Description { get; private set; }

    private readonly List<UserRole> _userRoles = new();
    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();

    private Role() { } // EF Core için

    public static Role Create(string name, string description)
    {
        return new Role
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            CreatedAt = DateTime.UtcNow
        };
    }
}
