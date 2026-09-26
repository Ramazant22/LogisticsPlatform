using LogisticsPlatform.Modules.Shipment.Domain;
using Xunit;

namespace LogisticsPlatform.API.Tests;

public class ShipmentTests
{
    [Fact]
    public void AssignToVehicle_SetsVehicleAndAssignmentStatus()
    {
        var shipment = new Shipment { Id = Guid.NewGuid() };
        var vehicleId = Guid.NewGuid();

        shipment.AssignToVehicle(vehicleId);

        Assert.Equal(vehicleId, shipment.AssignedVehicleId);
        Assert.Equal("Araca Atandı", shipment.Status);
    }
}
