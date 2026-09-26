using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LogisticsPlatform.BuildingBlocks.CQRS;
using LogisticsPlatform.Modules.Identity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LogisticsPlatform.Modules.Identity.Application;

public record LoginQuery(string Email, string Password) : IQuery<string>;

public class LoginQueryHandler : IQueryHandler<LoginQuery, string>
{
    private readonly IdentityDbContext _context;

    public LoginQueryHandler(IdentityDbContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        // BCrypt.Verify: Kullanıcının girdiği düz şifre ile DB'deki hash'i karşılaştırır
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash) || !user.IsActive)
        {
            throw new Exception("Geçersiz e-posta veya şifre!");
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("TenantId", user.TenantId.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SuperSecretKeyForLogisticsPlatform_12345!"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = creds,
            Issuer = "LogisticsPlatform",
            Audience = "LogisticsPlatformUsers"
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}


