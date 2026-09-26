using FluentValidation;
using Hangfire;
using LogisticsPlatform.API.Middleware;
using LogisticsPlatform.API.Services;
using LogisticsPlatform.API.Audit;
using LogisticsPlatform.BuildingBlocks.CQRS;
using LogisticsPlatform.BuildingBlocks.Domain;
using LogisticsPlatform.Modules.Billing.Infrastructure;
using LogisticsPlatform.Modules.Customers.Infrastructure;
using LogisticsPlatform.Modules.Drivers.Infrastructure;
using LogisticsPlatform.Modules.Fleet.Infrastructure;
using LogisticsPlatform.Modules.Identity.Infrastructure;
using LogisticsPlatform.Modules.Notifications.Infrastructure;
using LogisticsPlatform.Modules.Orders.Infrastructure;
using LogisticsPlatform.Modules.Reporting.Infrastructure; // YENİ EKLENDİ
using LogisticsPlatform.Modules.Routes.Infrastructure;
using LogisticsPlatform.Modules.Shipment.Infrastructure;
using LogisticsPlatform.Modules.Tenancy.Infrastructure;
using LogisticsPlatform.Modules.Tracking.Application.Hubs;
using LogisticsPlatform.Modules.Tracking.Infrastructure;
using LogisticsPlatform.Modules.Notifications.Application.Consumers;
using LogisticsPlatform.Modules.Billing.Application.Consumers;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT anahtarı için Jwt:Key yapılandırılmalıdır.");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "LogisticsPlatform";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "LogisticsPlatformUsers";

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddStackExchangeRedisCache(options => options.Configuration = builder.Configuration["Redis:Connection"] ?? "localhost:6379");

// --- VERİTABANI BAĞLAMLARI (DbContexts) ---
builder.Services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly("LogisticsPlatform.API")));
builder.Services.AddDbContext<AuditDbContext>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly("LogisticsPlatform.API")));
builder.Services.AddDbContext<TenancyDbContext>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly("LogisticsPlatform.API")));
builder.Services.AddDbContext<FleetDbContext>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly("LogisticsPlatform.API")));
builder.Services.AddDbContext<ShipmentDbContext>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly("LogisticsPlatform.API")));
builder.Services.AddDbContext<DriversDbContext>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly("LogisticsPlatform.API")));
builder.Services.AddDbContext<CustomersDbContext>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly("LogisticsPlatform.API")));
builder.Services.AddDbContext<OrdersDbContext>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly("LogisticsPlatform.API")));
builder.Services.AddDbContext<TrackingDbContext>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly("LogisticsPlatform.API")));
builder.Services.AddDbContext<BillingDbContext>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly("LogisticsPlatform.API")));
builder.Services.AddDbContext<NotificationsDbContext>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly("LogisticsPlatform.API")));
builder.Services.AddDbContext<RoutesDbContext>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly("LogisticsPlatform.API")));
builder.Services.AddDbContext<ReportingDbContext>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly("LogisticsPlatform.API"))); // YENİ EKLENDİ

// --- MEDIATR & FLUENT VALIDATION AYARLARI ---
// Bütün modüller eksiksiz taranıyor!
var assemblies = new[]
{
    typeof(IdentityDbContext).Assembly,
    typeof(TenancyDbContext).Assembly,
    typeof(FleetDbContext).Assembly,
    typeof(ShipmentDbContext).Assembly,
    typeof(DriversDbContext).Assembly,
    typeof(CustomersDbContext).Assembly,
    typeof(OrdersDbContext).Assembly,
    typeof(TrackingDbContext).Assembly,
    typeof(BillingDbContext).Assembly,
    typeof(NotificationsDbContext).Assembly,
    typeof(RoutesDbContext).Assembly,
    typeof(ReportingDbContext).Assembly, // YENİ EKLENDİ
    typeof(LogisticsPlatform.Modules.Orders.Application.Queries.GetOrdersQuery).Assembly,
    typeof(LogisticsPlatform.Modules.Identity.Application.Queries.LoginQuery).Assembly,
    typeof(LogisticsPlatform.Modules.Fleet.Application.Queries.GetVehiclesQuery).Assembly,
    typeof(LogisticsPlatform.Modules.Drivers.Application.Queries.GetDriversQuery).Assembly
};

builder.Services.AddValidatorsFromAssemblies(assemblies);
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssemblies(assemblies);
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
});

// --- KİMLİK DOĞRULAMA (JWT Authentication) ---
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                if (!string.IsNullOrWhiteSpace(accessToken) && context.HttpContext.Request.Path.StartsWithSegments("/trackingHub"))
                    context.Token = accessToken;
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("OperationsManage", policy => policy.RequireRole("Company Admin", "Operations Manager", "Super Admin"));
    options.AddPolicy("BillingManage", policy => policy.RequireRole("Company Admin", "Finance", "Super Admin"));
    options.AddPolicy("ReportRead", policy => policy.RequireRole("Company Admin", "Operations Manager", "Finance", "Super Admin"));
});

// --- MASSTRANSIT / RabbitMQ ---
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<VehicleMaintenanceDueEventConsumer>();
    x.AddConsumer<ShipmentDeliveredNotificationConsumer>();
    x.AddConsumer<ShipmentDeliveredEventConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMq:Host"] ?? "localhost", "/", host =>
        {
            host.Username(builder.Configuration["RabbitMq:Username"] ?? "logistics");
            host.Password(builder.Configuration["RabbitMq:Password"] ?? "logistics-dev-password");
        });
        cfg.UseMessageRetry(retry => retry.Exponential(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(2)));
        cfg.UseDelayedRedelivery(redelivery => redelivery.Intervals(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(15)));
        cfg.ConfigureEndpoints(context);
    });
});

// --- HANGFIRE (Background Jobs) ---
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(connectionString));

builder.Services.AddHangfireServer();
builder.Services.AddScoped<LogisticsPlatform.API.Jobs.MaintenanceCheckerJob>();

// --- SIGNALR & CORS (Frontend İletişimi) ---
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod().AllowCredentials());
});

builder.Services.AddHttpClient();
builder.Services.AddHealthChecks();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
await DatabaseInitializer.ApplyMigrationsAndSeedAsync(app);

// --- UYGULAMA MİMARİSİ (Middlewares) ---
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapHealthChecks("/health");
app.MapControllers();
app.MapHub<TrackingHub>("/trackingHub");
app.UseHangfireDashboard("/hangfire");
RecurringJob.AddOrUpdate<LogisticsPlatform.API.Jobs.MaintenanceCheckerJob>("maintenance-check", job => job.CheckUpcomingMaintenances(), Cron.Daily);

app.Run();
