using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace LogisticsPlatform.Modules.Identity.Application.Queries;

public class LoginResponse {
    public string Token { get; set; } = string.Empty;
    public string UserFullName { get; set; } = string.Empty;
}

public class LoginQuery : IRequest<LoginResponse> {
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginQueryHandler : IRequestHandler<LoginQuery, LoginResponse>
{
    public Task<LoginResponse> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        // İleride veritabanından kullanıcı şifre doğrulaması yapılacak.
        // Şimdilik sahte ve başarılı bir giriş (Token) üretiyoruz.
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes("SuperSecretKeyForLogisticsPlatform_12345!");
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Email, request.Email),
                new Claim(ClaimTypes.Name, "Admin Kullanıcı"),
                new Claim(ClaimTypes.Role, "SuperAdmin")
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = "LogisticsPlatform",
            Audience = "LogisticsPlatformUsers"
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return Task.FromResult(new LoginResponse {
            Token = tokenHandler.WriteToken(token),
            UserFullName = "Admin Kullanıcı"
        });
    }
}
