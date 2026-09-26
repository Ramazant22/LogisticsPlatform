using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using LogisticsPlatform.Modules.Reporting.Domain.Entities;
using LogisticsPlatform.Modules.Reporting.Infrastructure;

namespace LogisticsPlatform.Modules.Reporting.Application;

public class GenerateReportCommandHandler : IRequestHandler<GenerateReportCommand, Guid>
{
    private readonly ReportingDbContext _context;

    public GenerateReportCommandHandler(ReportingDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(GenerateReportCommand request, CancellationToken cancellationToken)
    {
        var report = new DailyReport
        {
            Id = Guid.NewGuid(),
            ReportDate = DateTime.UtcNow.Date,
            TotalShipments = request.TotalShipments,
            DeliveredShipments = request.DeliveredShipments,
            ActiveVehicles = request.ActiveVehicles,
            TotalRevenue = request.TotalRevenue,
            TenantId = request.TenantId
        };

        _context.DailyReports.Add(report);
        await _context.SaveChangesAsync(cancellationToken);

        return report.Id;
    }
}
