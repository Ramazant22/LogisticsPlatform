using Microsoft.EntityFrameworkCore;
using LogisticsPlatform.Modules.Drivers.Domain.Entities;

namespace LogisticsPlatform.Modules.Drivers.Infrastructure;

public class DriversDbContext : DbContext
{
    public DriversDbContext(DbContextOptions<DriversDbContext> options) : base(options)
    {
    }

    public DbSet<Driver> Drivers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("drivers");
        base.OnModelCreating(modelBuilder);
    }
}
