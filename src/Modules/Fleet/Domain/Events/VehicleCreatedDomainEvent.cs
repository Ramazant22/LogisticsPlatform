using LogisticsPlatform.BuildingBlocks.Domain;

namespace LogisticsPlatform.Modules.Fleet.Domain.Events;

public record VehicleCreatedDomainEvent(Guid EventId, DateTime OccurredOn, Guid VehicleId, string LicensePlate) : IDomainEvent
{
    public VehicleCreatedDomainEvent(Guid vehicleId, string licensePlate)
        : this(Guid.NewGuid(), DateTime.UtcNow, vehicleId, licensePlate) { }
}
