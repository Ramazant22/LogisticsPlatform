using Microsoft.EntityFrameworkCore;
using LogisticsPlatform.Modules.Drivers.Domain.Entities;

namespace LogisticsPlatform.Modules.Drivers.Infrastructure;

public class DriverDbContext : DbContext
{
    public DriverDbContext(DbContextOptions<DriverDbContext> options) : base(options) { }
    
    public DbSet<Driver> Drivers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("drivers");
        modelBuilder.Entity<Driver>().ToTable("Drivers");
        modelBuilder.Entity<Driver>().HasKey(x => x.Id);
    }
}
