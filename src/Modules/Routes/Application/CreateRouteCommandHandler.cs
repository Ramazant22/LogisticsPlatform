using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using LogisticsPlatform.Modules.Routes.Domain.Entities;
using LogisticsPlatform.Modules.Routes.Infrastructure;

namespace LogisticsPlatform.Modules.Routes.Application;

public class CreateRouteCommandHandler : IRequestHandler<CreateRouteCommand, Guid>
{
    private readonly RoutesDbContext _context;

    public CreateRouteCommandHandler(RoutesDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateRouteCommand request, CancellationToken cancellationToken)
    {
        var route = new DeliveryRoute
        {
            Id = Guid.NewGuid(),
            RouteName = request.RouteName,
            StartLocation = request.StartLocation,
            EndLocation = request.EndLocation,
            DistanceKm = request.DistanceKm,
            Status = "Aktif",
            TenantId = request.TenantId
        };

        _context.Routes.Add(route);
        await _context.SaveChangesAsync(cancellationToken);

        return route.Id;
    }
}
