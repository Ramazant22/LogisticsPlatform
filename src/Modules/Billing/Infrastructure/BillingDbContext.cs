using Microsoft.EntityFrameworkCore;
using LogisticsPlatform.Modules.Billing.Domain.Entities;

namespace LogisticsPlatform.Modules.Billing.Infrastructure;

public class BillingDbContext : DbContext
{
    public BillingDbContext(DbContextOptions<BillingDbContext> options) : base(options)
    {
    }

    public DbSet<BillingRecord> BillingRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("billing");
        
        modelBuilder.Entity<BillingRecord>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.InvoiceNumber).IsRequired();
            entity.Property(e => e.CustomerName).IsRequired();
            entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
        });
    }
}
