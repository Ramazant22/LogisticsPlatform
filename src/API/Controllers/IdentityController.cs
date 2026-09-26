using LogisticsPlatform.Modules.Identity.Infrastructure;
using LogisticsPlatform.Modules.Identity.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LogisticsPlatform.API.Controllers;

public sealed class CreateTenantUserReq
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string Role { get; init; } = "Operations Manager";
}

[ApiController]
[Route("api/[controller]")]
public class IdentityController : TenantControllerBase
{
    private readonly IdentityDbContext _identityDb;

    public IdentityController(IdentityDbContext identityDb) => _identityDb = identityDb;

    [HttpGet]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        if (!TryGetTenantId(out var tenantId)) return Forbid();

        var users = await _identityDb.Users
            .Where(user => user.TenantId == tenantId)
            .OrderBy(user => user.FirstName)
            .Select(user => new
            {
                user.Id,
                FullName = user.FirstName + " " + user.LastName,
                user.Email,
                user.Role,
                Status = user.IsActive ? "Active" : "Passive",
                LastLogin = (string?)null
            })
            .ToListAsync(cancellationToken);

        return Ok(users);
    }

    [Authorize(Policy = "OperationsManage")]
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateTenantUserReq request, CancellationToken cancellationToken)
    {
        if (!TryGetTenantId(out var tenantId)) return Forbid();
        var email = request.Email.Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(request.FirstName) || string.IsNullOrWhiteSpace(request.LastName) || string.IsNullOrWhiteSpace(email) || request.Password.Length < 8)
            return BadRequest(new { Message = "Ad, soyad, e-posta ve en az 8 karakterli şifre zorunludur." });
        if (await _identityDb.Users.AnyAsync(user => user.Email == email, cancellationToken))
            return Conflict(new { Message = "Bu e-posta adresi zaten kullanılıyor." });

        var role = request.Role switch
        {
            "Sistem Yöneticisi" => "Company Admin",
            "Operasyon Sorumlusu" => "Operations Manager",
            "Finans Uzmanı" => "Finance",
            _ => request.Role
        };
        var user = LogisticsPlatform.Modules.Identity.Domain.User.Create(tenantId, request.FirstName.Trim(), request.LastName.Trim(), email, BCrypt.Net.BCrypt.HashPassword(request.Password), role);
        _identityDb.Users.Add(user);
        await _identityDb.SaveChangesAsync(cancellationToken);
        return Created($"api/identity/{user.Id}", new { user.Id, FullName = user.FirstName + " " + user.LastName, user.Email, user.Role, Status = "Active" });
    }
}
