using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsPlatform.API.Controllers;

[Authorize]
public abstract class TenantControllerBase : ControllerBase
{
    protected bool TryGetTenantId(out Guid tenantId)
    {
        return Guid.TryParse(User.FindFirstValue("TenantId"), out tenantId) && tenantId != Guid.Empty;
    }
}
