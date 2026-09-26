using LogisticsPlatform.BuildingBlocks.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LogisticsPlatform.Modules.Shipment.Infrastructure;

public class ShipmentDbContext : DbContext
{
    private readonly ICurrentUserService _currentUserService;

    public ShipmentDbContext(DbContextOptions<ShipmentDbContext> options, ICurrentUserService currentUserService) 
        : base(options)
    {
        _currentUserService = currentUserService;
    }

    public DbSet<Domain.Shipment> Shipments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("shipment");

        modelBuilder.Entity<Domain.Shipment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasQueryFilter(e => e.TenantId == _currentUserService.TenantId);
        });

        base.OnModelCreating(modelBuilder);
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<Domain.Shipment>())
        {
            if (entry.State == EntityState.Added && entry.Entity.TenantId == Guid.Empty)
            {
                var tenantId = _currentUserService.TenantId;
                if (tenantId == Guid.Empty)
                    throw new Exception("TenantId cannot be empty.");
                entry.Entity.TenantId = tenantId;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}

