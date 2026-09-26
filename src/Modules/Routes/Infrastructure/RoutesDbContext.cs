using Microsoft.EntityFrameworkCore;
using LogisticsPlatform.Modules.Routes.Domain.Entities;

namespace LogisticsPlatform.Modules.Routes.Infrastructure;

public class RoutesDbContext : DbContext
{
    public RoutesDbContext(DbContextOptions<RoutesDbContext> options) : base(options)
    {
    }

    public DbSet<DeliveryRoute> Routes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("routes");
        
        modelBuilder.Entity<DeliveryRoute>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RouteName).IsRequired();
            entity.Property(e => e.StartLocation).IsRequired();
            entity.Property(e => e.EndLocation).IsRequired();
            entity.Property(e => e.DistanceKm).HasColumnType("decimal(18,2)");
        });
    }
}
