using Microsoft.EntityFrameworkCore;
using LogisticsPlatform.Modules.Fleet.Domain.Entities;

namespace LogisticsPlatform.Modules.Fleet.Infrastructure;

public class FleetDbContext : DbContext
{
    public FleetDbContext(DbContextOptions<FleetDbContext> options) : base(options) { }
    
    public DbSet<Vehicle> Vehicles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("fleet");
        modelBuilder.Entity<Vehicle>().ToTable("Vehicles");
        modelBuilder.Entity<Vehicle>().HasKey(x => x.Id);
    }
}
