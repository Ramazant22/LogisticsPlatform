using LogisticsPlatform.Modules.Identity.Domain;
using LogisticsPlatform.Modules.Identity.Infrastructure;
using LogisticsPlatform.Modules.Tenancy.Domain.Entities;
using LogisticsPlatform.Modules.Tenancy.Infrastructure;
using LogisticsPlatform.Modules.Shipment.Infrastructure;
using LogisticsPlatform.API.Audit;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPlatform.API.Services;

public static class DatabaseInitializer
{
    public static async Task ApplyMigrationsAndSeedAsync(WebApplication app)
    {
        if (!app.Configuration.GetValue<bool>("Database:ApplyMigrationsOnStartup"))
            return;

        const int maxAttempts = 10;
        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                await using var scope = app.Services.CreateAsyncScope();
                var tenancyDb = scope.ServiceProvider.GetRequiredService<TenancyDbContext>();
                var identityDb = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
                var shipmentDb = scope.ServiceProvider.GetRequiredService<ShipmentDbContext>();
                var auditDb = scope.ServiceProvider.GetRequiredService<AuditDbContext>();

                await tenancyDb.Database.MigrateAsync();
                await identityDb.Database.MigrateAsync();
                await shipmentDb.Database.MigrateAsync();
                await auditDb.Database.MigrateAsync();
                await SeedDemoDataAsync(app.Configuration, tenancyDb, identityDb);
                return;
            }
            catch (Exception) when (attempt < maxAttempts)
            {
                app.Logger.LogWarning("Veritabanı henüz hazır değil. Migration {Attempt}/{MaxAttempts} yeniden denenecek.", attempt, maxAttempts);
                await Task.Delay(TimeSpan.FromSeconds(3));
            }
        }

        throw new InvalidOperationException("Veritabanı migration işlemi başlatılamadı.");
    }

    private static async Task SeedDemoDataAsync(IConfiguration configuration, TenancyDbContext tenancyDb, IdentityDbContext identityDb)
    {
        if (!configuration.GetValue<bool>("Database:SeedDemoData") || await tenancyDb.Tenants.AnyAsync())
            return;

        var tenant = new TenantRecord { Id = Guid.NewGuid(), CompanyName = "Demo Lojistik", SubscriptionPlan = "Enterprise" };
        tenancyDb.Tenants.Add(tenant);
        identityDb.Users.Add(User.Create(
            tenant.Id,
            configuration["Seed:AdminFirstName"] ?? "Demo",
            configuration["Seed:AdminLastName"] ?? "Yönetici",
            configuration["Seed:AdminEmail"] ?? "admin@demo.local",
            BCrypt.Net.BCrypt.HashPassword(configuration["Seed:AdminPassword"] ?? "ChangeMe123!"),
            "Company Admin"));

        await tenancyDb.SaveChangesAsync();
        await identityDb.SaveChangesAsync();
    }
}
