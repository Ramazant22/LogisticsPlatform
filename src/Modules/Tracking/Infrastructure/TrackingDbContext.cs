using Microsoft.EntityFrameworkCore;
using LogisticsPlatform.Modules.Tracking.Domain.Entities;

namespace LogisticsPlatform.Modules.Tracking.Infrastructure;

public class TrackingDbContext : DbContext
{
    public TrackingDbContext(DbContextOptions<TrackingDbContext> options) : base(options)
    {
    }

    public DbSet<TrackingRecord> TrackingRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("tracking");
        
        modelBuilder.Entity<TrackingRecord>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ShipmentNumber).IsRequired();
            entity.Property(e => e.CurrentLocation).IsRequired();
            entity.Property(e => e.Latitude).HasColumnType("decimal(10,6)");
            entity.Property(e => e.Longitude).HasColumnType("decimal(10,6)");
        });
    }
}
