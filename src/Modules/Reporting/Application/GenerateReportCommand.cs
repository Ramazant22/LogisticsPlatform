using System;
using MediatR;

namespace LogisticsPlatform.Modules.Reporting.Application;

public class GenerateReportCommand : IRequest<Guid>
{
    public int TotalShipments { get; set; }
    public int DeliveredShipments { get; set; }
    public int ActiveVehicles { get; set; }
    public decimal TotalRevenue { get; set; }
    public Guid TenantId { get; set; }
}
