using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using LogisticsPlatform.Modules.Tracking.Domain.Entities;
using LogisticsPlatform.Modules.Tracking.Infrastructure;

namespace LogisticsPlatform.Modules.Tracking.Application;

public class UpdateTrackingCommandHandler : IRequestHandler<UpdateTrackingCommand, Guid>
{
    private readonly TrackingDbContext _context;

    public UpdateTrackingCommandHandler(TrackingDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(UpdateTrackingCommand request, CancellationToken cancellationToken)
    {
        var record = new TrackingRecord
        {
            Id = Guid.NewGuid(),
            ShipmentNumber = request.ShipmentNumber,
            CurrentLocation = request.CurrentLocation,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            Status = request.Status,
            Timestamp = DateTime.UtcNow,
            TenantId = request.TenantId
        };

        _context.TrackingRecords.Add(record);
        await _context.SaveChangesAsync(cancellationToken);

        return record.Id;
    }
}
