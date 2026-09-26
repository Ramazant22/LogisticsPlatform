using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using LogisticsPlatform.Modules.Billing.Infrastructure;
using LogisticsPlatform.Modules.Billing.Domain;

namespace LogisticsPlatform.Modules.Billing.Application;

public class GenerateInvoiceCommand : IRequest<Guid>
{
    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
    public decimal Amount { get; set; }
}

public class GenerateInvoiceCommandHandler : IRequestHandler<GenerateInvoiceCommand, Guid>
{
    private readonly BillingDbContext _context;

    public GenerateInvoiceCommandHandler(BillingDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(GenerateInvoiceCommand request, CancellationToken cancellationToken)
    {
        var invoice = new Invoice(
            Guid.NewGuid(),
            request.OrderId,
            request.CustomerId,
            request.Amount
        );

        _context.Set<Invoice>().Add(invoice);
        await _context.SaveChangesAsync(cancellationToken);

        // Terminalde faturanýn kesildiðini renkli görmek için
        Console.WriteLine($"\n[BILLING MODÜLÜ] ?? Fatura Kesildi! Sipariþ ID: {request.OrderId} | Tutar: {request.Amount} TL\n");

        return invoice.Id;
    }
}
