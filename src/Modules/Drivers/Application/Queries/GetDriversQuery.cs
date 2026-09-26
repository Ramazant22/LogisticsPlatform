using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LogisticsPlatform.Modules.Drivers.Application.Queries;

public class DriverDto {
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string LicenseType { get; set; } = string.Empty;
}

public class GetDriversQuery : IRequest<List<DriverDto>> { }

public class GetDriversQueryHandler : IRequestHandler<GetDriversQuery, List<DriverDto>>
{
    public Task<List<DriverDto>> Handle(GetDriversQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new List<DriverDto>());
    }
}
