using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using BCrypt.Net;
using LogisticsPlatform.Modules.Identity.Domain;
using LogisticsPlatform.Modules.Identity.Infrastructure;
using LogisticsPlatform.Modules.Tenancy.Domain.Entities;
using LogisticsPlatform.Modules.Tenancy.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LogisticsPlatform.API.Controllers;

public sealed class LoginReq { public string Email { get; init; } = string.Empty; public string Password { get; init; } = string.Empty; }
public sealed class RegisterReq { public string CompanyName { get; init; } = string.Empty; public string FirstName { get; init; } = string.Empty; public string LastName { get; init; } = string.Empty; public string Email { get; init; } = string.Empty; public string Password { get; init; } = string.Empty; }
public sealed class RefreshReq { public string RefreshToken { get; init; } = string.Empty; }
public sealed class LogoutReq { public string RefreshToken { get; init; } = string.Empty; }

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IdentityDbContext _identityDb;
    private readonly TenancyDbContext _tenancyDb;
    private readonly IConfiguration _configuration;

    public AuthController(IdentityDbContext identityDb, TenancyDbContext tenancyDb, IConfiguration configuration) => (_identityDb, _tenancyDb, _configuration) = (identityDb, tenancyDb, configuration);

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginReq request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(request.Password)) return BadRequest(new { Message = "E-posta ve şifre zorunludur." });
        var user = await _identityDb.Users.SingleOrDefaultAsync(item => item.Email == email, cancellationToken);
        if (user is null) return Unauthorized(new { Message = "E-posta veya şifre hatalı." });
        if (!user.IsActive || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash)) return Unauthorized(new { Message = "E-posta veya şifre hatalı." });
        var session = await CreateSessionAsync(user, cancellationToken);
        return Ok(new { Token = session.AccessToken, session.RefreshToken, UserFullName = $"{user.FirstName} {user.LastName}".Trim(), TenantId = user.TenantId, Role = user.Role, AccessTokenExpiresInSeconds = 900 });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterReq request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(request.CompanyName) || string.IsNullOrWhiteSpace(request.FirstName) || string.IsNullOrWhiteSpace(request.LastName) || string.IsNullOrWhiteSpace(email)) return BadRequest(new { Message = "Firma adı, ad, soyad ve e-posta zorunludur." });
        if (request.Password.Length < 8) return BadRequest(new { Message = "Şifre en az 8 karakter olmalıdır." });
        if (await _identityDb.Users.AnyAsync(item => item.Email == email, cancellationToken)) return Conflict(new { Message = "Bu e-posta adresi zaten kullanılıyor." });

        var tenant = new TenantRecord { Id = Guid.NewGuid(), CompanyName = request.CompanyName.Trim(), SubscriptionPlan = "Starter", IsActive = true, CreatedAt = DateTime.UtcNow };
        var user = LogisticsPlatform.Modules.Identity.Domain.User.Create(tenant.Id, request.FirstName.Trim(), request.LastName.Trim(), email, BCrypt.Net.BCrypt.HashPassword(request.Password), "Company Admin");
        _tenancyDb.Tenants.Add(tenant);
        _identityDb.Users.Add(user);
        await _tenancyDb.SaveChangesAsync(cancellationToken);
        await _identityDb.SaveChangesAsync(cancellationToken);
        return Created(string.Empty, new { Success = true, TenantId = tenant.Id, UserId = user.Id, Message = "Firma ve yönetici hesabı oluşturuldu." });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshReq request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken)) return BadRequest(new { Message = "Refresh token zorunludur." });

        var tokenHash = HashToken(request.RefreshToken);
        var existing = await _identityDb.RefreshTokens.SingleOrDefaultAsync(item => item.TokenHash == tokenHash, cancellationToken);
        if (existing is null || existing.RevokedAt is not null || existing.ExpiresAt <= DateTime.UtcNow)
            return Unauthorized(new { Message = "Oturum yenilenemedi. Lütfen tekrar giriş yapın." });

        var user = await _identityDb.Users.SingleOrDefaultAsync(item => item.Id == existing.UserId, cancellationToken);
        if (user is null || !user.IsActive) return Unauthorized(new { Message = "Oturum yenilenemedi. Lütfen tekrar giriş yapın." });

        existing.RevokedAt = DateTime.UtcNow;
        var session = await CreateSessionAsync(user, cancellationToken);
        existing.ReplacedByTokenHash = HashToken(session.RefreshToken);
        await _identityDb.SaveChangesAsync(cancellationToken);
        return Ok(new { Token = session.AccessToken, session.RefreshToken, AccessTokenExpiresInSeconds = 900 });
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutReq request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken)) return NoContent();
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var token = await _identityDb.RefreshTokens.SingleOrDefaultAsync(item => item.TokenHash == HashToken(request.RefreshToken), cancellationToken);
        if (token is not null && token.UserId.ToString() == userId && token.RevokedAt is null)
        {
            token.RevokedAt = DateTime.UtcNow;
            await _identityDb.SaveChangesAsync(cancellationToken);
        }
        return NoContent();
    }

    private string CreateToken(User user)
    {
        var key = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT anahtarı yapılandırılmamış.");
        var issuer = _configuration["Jwt:Issuer"] ?? "LogisticsPlatform";
        var audience = _configuration["Jwt:Audience"] ?? "LogisticsPlatformUsers";
        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}".Trim()), new Claim(ClaimTypes.Email, user.Email), new Claim(ClaimTypes.Role, user.Role), new Claim("TenantId", user.TenantId.ToString()) };
        var descriptor = new SecurityTokenDescriptor { Subject = new ClaimsIdentity(claims), Expires = DateTime.UtcNow.AddMinutes(15), Issuer = issuer, Audience = audience, SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256) };
        var handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(handler.CreateToken(descriptor));
    }

    private async Task<(string AccessToken, string RefreshToken)> CreateSessionAsync(User user, CancellationToken cancellationToken)
    {
        var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        _identityDb.RefreshTokens.Add(new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TokenHash = HashToken(refreshToken),
            ExpiresAt = DateTime.UtcNow.AddDays(14)
        });
        await _identityDb.SaveChangesAsync(cancellationToken);
        return (CreateToken(user), refreshToken);
    }

    private static string HashToken(string token) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
}
