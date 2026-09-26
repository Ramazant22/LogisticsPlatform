using Microsoft.EntityFrameworkCore;
using LogisticsPlatform.Modules.Orders.Domain.Entities;

namespace LogisticsPlatform.Modules.Orders.Infrastructure;

public class OrdersDbContext : DbContext
{
    public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options) { }
    
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("orders");
        modelBuilder.Entity<Order>().HasKey(x => x.Id);
        base.OnModelCreating(modelBuilder);
    }
}
