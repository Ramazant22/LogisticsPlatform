using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LogisticsPlatform.Modules.Fleet.Application.Queries;

public class VehicleDto {
    public Guid Id { get; set; }
    public string PlateNumber { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class GetVehiclesQuery : IRequest<List<VehicleDto>> { }

public class GetVehiclesQueryHandler : IRequestHandler<GetVehiclesQuery, List<VehicleDto>>
{
    public Task<List<VehicleDto>> Handle(GetVehiclesQuery request, CancellationToken cancellationToken)
    {
        // Şimdilik arayüzün patlamaması için güvenli boş liste dönüyoruz.
        return Task.FromResult(new List<VehicleDto>());
    }
}
