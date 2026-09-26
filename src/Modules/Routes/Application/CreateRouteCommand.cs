using System;
using MediatR;

namespace LogisticsPlatform.Modules.Routes.Application;

public class CreateRouteCommand : IRequest<Guid>
{
    public string RouteName { get; set; } = string.Empty;
    public string StartLocation { get; set; } = string.Empty;
    public string EndLocation { get; set; } = string.Empty;
    public decimal DistanceKm { get; set; }
    public Guid TenantId { get; set; }
}
