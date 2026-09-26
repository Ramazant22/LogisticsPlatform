using Microsoft.EntityFrameworkCore;
using LogisticsPlatform.Modules.Reporting.Domain.Entities;

namespace LogisticsPlatform.Modules.Reporting.Infrastructure;

public class ReportingDbContext : DbContext
{
    public ReportingDbContext(DbContextOptions<ReportingDbContext> options) : base(options)
    {
    }

    public DbSet<DailyReport> DailyReports { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("reporting");
        
        modelBuilder.Entity<DailyReport>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TotalRevenue).HasColumnType("decimal(18,2)");
        });
    }
}
