using MediatR;
using System;

namespace LogisticsPlatform.BuildingBlocks.Events;

public record ShipmentAssignedEvent(Guid ShipmentId, Guid VehicleId) : INotification;
