using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using LogisticsPlatform.Modules.Customers.Domain.Entities;
using LogisticsPlatform.Modules.Customers.Infrastructure;

namespace LogisticsPlatform.Modules.Customers.Application;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Guid>
{
    private readonly CustomersDbContext _context;

    public CreateCustomerCommandHandler(CustomersDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            Address = request.Address,
            TenantId = request.TenantId
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync(cancellationToken);

        return customer.Id;
    }
}
