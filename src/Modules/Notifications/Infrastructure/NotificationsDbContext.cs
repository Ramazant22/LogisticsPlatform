using Microsoft.EntityFrameworkCore;
using LogisticsPlatform.Modules.Notifications.Domain.Entities;

namespace LogisticsPlatform.Modules.Notifications.Infrastructure;

public class NotificationsDbContext : DbContext
{
    public NotificationsDbContext(DbContextOptions<NotificationsDbContext> options) : base(options)
    {
    }

    public DbSet<NotificationRecord> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("notifications");
        
        modelBuilder.Entity<NotificationRecord>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Recipient).IsRequired();
            entity.Property(e => e.Title).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Message).IsRequired();
        });
    }
}
