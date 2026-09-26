using Microsoft.EntityFrameworkCore;
using LogisticsPlatform.Modules.Tenancy.Domain.Entities;

namespace LogisticsPlatform.Modules.Tenancy.Infrastructure;

public class TenancyDbContext : DbContext
{
    public TenancyDbContext(DbContextOptions<TenancyDbContext> options) : base(options)
    {
    }

    public DbSet<TenantRecord> Tenants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("tenancy");
        
        modelBuilder.Entity<TenantRecord>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CompanyName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.SubscriptionPlan).IsRequired();
        });
    }
}
