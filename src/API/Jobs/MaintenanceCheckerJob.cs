using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using LogisticsPlatform.Modules.Fleet.Infrastructure;
using LogisticsPlatform.BuildingBlocks.Events;

namespace LogisticsPlatform.API.Jobs;

public class MaintenanceCheckerJob
{
    private readonly FleetDbContext _fleetContext;
    private readonly IPublishEndpoint _publishEndpoint;

    public MaintenanceCheckerJob(FleetDbContext fleetContext, IPublishEndpoint publishEndpoint)
    {
        _fleetContext = fleetContext;
        _publishEndpoint = publishEndpoint;
    }

    public async Task CheckUpcomingMaintenances()
    {
        Console.WriteLine("\n[HANGFIRE ??] Otomatik araç bakım taraması başladı...");

        var vehicles = await _fleetContext.Vehicles.ToListAsync();
        int count = 0;

        foreach (var vehicle in vehicles)
        {
            // Gerçekte burada tarih filtresi yapılır, biz test için hepsine uyarı fırlatıyoruz
            await _publishEndpoint.Publish(new VehicleMaintenanceDueEvent
            {
                VehicleId = vehicle.Id,
                PlateNumber = vehicle.LicensePlate
            });
            count++;
        }

        Console.WriteLine("[HANGFIRE ?] Tarama bitti. {count} araç için bakım bildirimi RabbitMQ'ya iletildi.\n");
    }
}


