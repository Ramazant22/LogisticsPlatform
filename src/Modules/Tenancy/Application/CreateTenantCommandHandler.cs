using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using LogisticsPlatform.Modules.Tenancy.Domain.Entities;
using LogisticsPlatform.Modules.Tenancy.Infrastructure;

namespace LogisticsPlatform.Modules.Tenancy.Application;

public class CreateTenantCommandHandler : IRequestHandler<CreateTenantCommand, Guid>
{
    private readonly TenancyDbContext _context;

    public CreateTenantCommandHandler(TenancyDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        var tenant = new TenantRecord
        {
            Id = Guid.NewGuid(),
            CompanyName = request.CompanyName,
            SubscriptionPlan = request.SubscriptionPlan,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.Tenants.Add(tenant);
        await _context.SaveChangesAsync(cancellationToken);

        return tenant.Id;
    }
}
