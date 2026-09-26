using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using LogisticsPlatform.Modules.Drivers.Domain.Entities;
using LogisticsPlatform.Modules.Drivers.Infrastructure;

namespace LogisticsPlatform.Modules.Drivers.Application;

public class CreateDriverCommandHandler : IRequestHandler<CreateDriverCommand, Guid>
{
    private readonly DriversDbContext _context;

    public CreateDriverCommandHandler(DriversDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateDriverCommand request, CancellationToken cancellationToken)
    {
        var driver = new Driver
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            FullName = request.FirstName + " " + request.LastName,
            PhoneNumber = request.PhoneNumber, // Eksik olan parça buydu!
            LicenseClass = request.LicenseClass,
            LicenseType = request.LicenseClass,
            TenantId = request.TenantId
        };

        _context.Drivers.Add(driver);
        await _context.SaveChangesAsync(cancellationToken);

        return driver.Id;
    }
}
