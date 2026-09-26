using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShipmentEntity = LogisticsPlatform.Modules.Shipment.Domain.Shipment;

namespace LogisticsPlatform.Modules.Shipment.Infrastructure.Configurations;

public class ShipmentConfiguration : IEntityTypeConfiguration<ShipmentEntity>
{
    public void Configure(EntityTypeBuilder<ShipmentEntity> builder)
    {
        builder.HasKey(s => s.Id);
        
        builder.Property(s => s.Description).IsRequired().HasMaxLength(200);
        builder.Property(s => s.Origin).IsRequired().HasMaxLength(100);
        builder.Property(s => s.Destination).IsRequired().HasMaxLength(100);
        builder.Property(s => s.WeightInKg).HasPrecision(18, 2).IsRequired();
        builder.Property(s => s.Status).HasMaxLength(50);
    }
}
