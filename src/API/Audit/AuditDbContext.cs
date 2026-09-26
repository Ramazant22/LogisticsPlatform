using Microsoft.EntityFrameworkCore;

namespace LogisticsPlatform.API.Audit;

public class AuditDbContext(DbContextOptions<AuditDbContext> options) : DbContext(options)
{
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("audit");
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(item => item.Id);
            entity.Property(item => item.Method).HasMaxLength(10).IsRequired();
            entity.Property(item => item.Path).HasMaxLength(500).IsRequired();
            entity.HasIndex(item => new { item.TenantId, item.CreatedAt });
        });
    }
}
