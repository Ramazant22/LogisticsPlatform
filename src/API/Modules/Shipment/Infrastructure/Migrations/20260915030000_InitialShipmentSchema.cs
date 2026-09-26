using LogisticsPlatform.Modules.Shipment.Infrastructure;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LogisticsPlatform.API.Modules.Shipment.Infrastructure.Migrations;

[DbContext(typeof(ShipmentDbContext))]
[Migration("20260915030000_InitialShipmentSchema")]
public class InitialShipmentSchema : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema("shipment");
        migrationBuilder.CreateTable("Shipments", "shipment", table => new
        {
            Id = table.Column<Guid>(nullable: false),
            TrackingNumber = table.Column<string>(nullable: false),
            Origin = table.Column<string>(nullable: false),
            Destination = table.Column<string>(nullable: false),
            Status = table.Column<string>(nullable: false),
            Description = table.Column<string>(nullable: false),
            WeightInKg = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            AssignedVehicleId = table.Column<Guid>(nullable: true),
            CustomerId = table.Column<Guid>(nullable: false),
            OrderId = table.Column<Guid>(nullable: false),
            AssignedDriverId = table.Column<Guid>(nullable: true),
            RouteId = table.Column<Guid>(nullable: true),
            FinalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            TenantId = table.Column<Guid>(nullable: false)
        }, table => table.PrimaryKey("PK_Shipments", item => item.Id));
        migrationBuilder.CreateIndex("IX_Shipments_TenantId", "shipment", "Shipments", "TenantId");
        migrationBuilder.CreateIndex("IX_Shipments_TrackingNumber", "shipment", "Shipments", "TrackingNumber", unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropTable("Shipments", "shipment");
}
