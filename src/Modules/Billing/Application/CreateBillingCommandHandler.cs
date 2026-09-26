using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using LogisticsPlatform.Modules.Billing.Domain.Entities;
using LogisticsPlatform.Modules.Billing.Infrastructure;

namespace LogisticsPlatform.Modules.Billing.Application;

public class CreateBillingCommandHandler : IRequestHandler<CreateBillingCommand, Guid>
{
    private readonly BillingDbContext _context;

    public CreateBillingCommandHandler(BillingDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateBillingCommand request, CancellationToken cancellationToken)
    {
        var billing = new BillingRecord
        {
            Id = Guid.NewGuid(),
            InvoiceNumber = request.InvoiceNumber,
            CustomerName = request.CustomerName,
            Amount = request.Amount,
            Status = "Ödenmedi",
            TenantId = request.TenantId
        };

        _context.BillingRecords.Add(billing);
        await _context.SaveChangesAsync(cancellationToken);

        return billing.Id;
    }
}
