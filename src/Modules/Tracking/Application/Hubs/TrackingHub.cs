using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Security.Claims;

namespace LogisticsPlatform.Modules.Tracking.Application.Hubs;

[Authorize]
public class TrackingHub : Hub
{
    // İstemciler (Frontend) bağlandığında çalışır
    public override async Task OnConnectedAsync()
    {
        var tenantId = Context.User?.FindFirstValue("TenantId");
        if (!string.IsNullOrWhiteSpace(tenantId))
            await Groups.AddToGroupAsync(Context.ConnectionId, $"tenant:{tenantId}");
        await base.OnConnectedAsync();
    }
}
