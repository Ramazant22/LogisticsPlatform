using System;
namespace LogisticsPlatform.BuildingBlocks.Events;
public class VehicleMaintenanceDueEvent
{
    public Guid VehicleId { get; set; }
    public string PlateNumber { get; set; } = string.Empty;
}
