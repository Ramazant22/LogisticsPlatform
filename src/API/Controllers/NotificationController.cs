using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using LogisticsPlatform.Modules.Notifications.Application;
using LogisticsPlatform.Modules.Notifications.Infrastructure;

namespace LogisticsPlatform.API.Controllers;

public class SendNotificationRequest
{
    public string Recipient { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

[ApiController]
[Route("api/[controller]")]
public class NotificationController : TenantControllerBase
{
    private readonly IMediator _mediator;
    private readonly NotificationsDbContext _context;

    public NotificationController(IMediator mediator, NotificationsDbContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        if (!TryGetTenantId(out var tenantId)) return Forbid();
        var notifications = await _context.Notifications.Where(item => item.TenantId == tenantId).OrderByDescending(x => x.CreatedAt).ToListAsync();
        return Ok(notifications);
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] SendNotificationRequest req)
    {
        if (!TryGetTenantId(out var tenantId)) return Forbid();
        var command = new SendNotificationCommand
        {
            Recipient = req.Recipient,
            Title = req.Title,
            Message = req.Message,
            TenantId = tenantId
        };

        var resultId = await _mediator.Send(command);
        return Ok(new { Message = "Bildirim başarıyla gönderildi!", Id = resultId });
    }
}
