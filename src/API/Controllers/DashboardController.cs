using LogisticsPlatform.Modules.Billing.Infrastructure;
using LogisticsPlatform.Modules.Fleet.Infrastructure;
using LogisticsPlatform.Modules.Orders.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace LogisticsPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : TenantControllerBase
{
    private readonly FleetDbContext _fleetDb;
    private readonly OrdersDbContext _ordersDb;
    private readonly BillingDbContext _billingDb;
    private readonly IDistributedCache _cache;

    public DashboardController(FleetDbContext fleetDb, OrdersDbContext ordersDb, BillingDbContext billingDb, IDistributedCache cache) => (_fleetDb, _ordersDb, _billingDb, _cache) = (fleetDb, ordersDb, billingDb, cache);

    [HttpGet("summary")]
    public async Task<IActionResult> GetDashboardSummary(CancellationToken cancellationToken)
    {
        if (!TryGetTenantId(out var tenantId)) return Forbid();
        var cacheKey = $"dashboard:summary:{tenantId}";
        var cached = await _cache.GetStringAsync(cacheKey, cancellationToken);
        if (cached is not null) return Content(cached, "application/json");

        var totalVehicles = await _fleetDb.Vehicles.CountAsync(item => item.TenantId == tenantId, cancellationToken);
        var activeVehicles = await _fleetDb.Vehicles.CountAsync(item => item.TenantId == tenantId && item.Status == "Aktif", cancellationToken);
        var pendingOrders = await _ordersDb.Orders.CountAsync(item => item.TenantId == tenantId && item.Status == "Beklemede", cancellationToken);
        var totalRevenue = await _billingDb.BillingRecords.Where(item => item.TenantId == tenantId && item.Status == "Ödendi").SumAsync(item => (decimal?)item.Amount, cancellationToken) ?? 0m;
        var recentOrders = await _ordersDb.Orders.Where(item => item.TenantId == tenantId).OrderByDescending(item => item.OrderDate).Take(5).Select(item => new { item.Id, item.OrderNumber, item.CustomerName, item.OrderDate, item.Status }).ToListAsync(cancellationToken);
        var activities = recentOrders.Select((order, index) => new { Id = index + 1, Action = $"Sipariş: {order.Status}", Detail = $"{order.OrderNumber} ({order.CustomerName})", Time = order.OrderDate.ToLocalTime().ToString("dd.MM.yyyy HH:mm"), Type = order.Status == "Teslim Edildi" ? "success" : "info" });

        var summary = new { TotalVehicles = totalVehicles, ActiveVehicles = activeVehicles, PendingOrders = pendingOrders, TotalRevenue = totalRevenue, RecentActivities = activities };
        var json = JsonSerializer.Serialize(summary);
        await _cache.SetStringAsync(cacheKey, json, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2) }, cancellationToken);
        return Content(json, "application/json");
    }
}
