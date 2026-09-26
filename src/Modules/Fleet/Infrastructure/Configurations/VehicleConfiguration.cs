using LogisticsPlatform.Modules.Fleet.Domain;
using LogisticsPlatform.Modules.Fleet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LogisticsPlatform.Modules.Fleet.Infrastructure.Configurations;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.HasKey(v => v.Id);
        
        builder.Property(v => v.LicensePlate)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(v => v.Brand)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(v => v.Model)
            .IsRequired()
            .HasMaxLength(50);

        // Ýþte o uyarý veren decimal alaný burada çözüyoruz (Toplam 18 hane, 2'si ondalýk)
        builder.Property(v => v.CapacityInKg)
            .HasPrecision(18, 2)
            .IsRequired();
            
        builder.Property(v => v.FuelType)
            .HasMaxLength(20);

        builder.Property(v => v.Status)
            .HasMaxLength(20);
    }
}

