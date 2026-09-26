using System;
using MediatR;

namespace LogisticsPlatform.Modules.Tracking.Application;

public class UpdateTrackingCommand : IRequest<Guid>
{
    public string ShipmentNumber { get; set; } = string.Empty;
    public string CurrentLocation { get; set; } = string.Empty;
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public string Status { get; set; } = "Yolda";
    public Guid TenantId { get; set; }
}
