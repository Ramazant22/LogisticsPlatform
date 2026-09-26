using System;
using MediatR;

namespace LogisticsPlatform.Modules.Drivers.Application;

public class CreateDriverCommand : IRequest<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string LicenseClass { get; set; } = string.Empty;
    public Guid TenantId { get; set; }
}
