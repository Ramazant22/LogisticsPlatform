using System;
using MediatR;

namespace LogisticsPlatform.Modules.Shipment.Application;

public class StartDispatchCommand : IRequest<bool>
{
    public Guid ShipmentId { get; set; }
    public Guid VehicleId { get; set; }
    public Guid DriverId { get; set; }
    public Guid RouteId { get; set; }
}
