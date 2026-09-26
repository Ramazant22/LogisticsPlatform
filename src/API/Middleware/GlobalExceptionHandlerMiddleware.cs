using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using LogisticsPlatform.API.Audit;
using System.Security.Claims;

namespace LogisticsPlatform.API.Middleware;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
            if (!HttpMethods.IsGet(context.Request.Method) && !context.Request.Path.StartsWithSegments("/health"))
            {
                var audit = context.RequestServices.GetRequiredService<AuditDbContext>();
                Guid? tenantId = Guid.TryParse(context.User.FindFirstValue("TenantId"), out var id) ? id : null;
                audit.AuditLogs.Add(new AuditLog { Id = Guid.NewGuid(), TenantId = tenantId, UserId = context.User.FindFirstValue(ClaimTypes.NameIdentifier), Method = context.Request.Method, Path = context.Request.Path, StatusCode = context.Response.StatusCode });
                await audit.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Beklenmeyen bir hata oluştu: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        switch (exception)
        {
            case ValidationException validationEx:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest; // 400
                
                // Hataları property adına göre gruplayıp düzenli bir sözlük (dictionary) haline getiriyoruz
                var errors = validationEx.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

                var validationResponse = new
                {
                    Status = 400,
                    Title = "Doğrulama Hatası (Validation Failed)",
                    Errors = errors
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(validationResponse));
                break;

            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // 500
                
                var genericResponse = new
                {
                    Status = 500,
                    Title = "Sunucu Hatası",
                    Message = exception.Message
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(genericResponse));
                break;
        }
    }
}
