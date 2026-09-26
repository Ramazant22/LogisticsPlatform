using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LogisticsPlatform.Modules.Identity.Domain.Entities;
using LogisticsPlatform.Modules.Identity.Infrastructure;
using System.Security.Cryptography;
using System.Text;

namespace LogisticsPlatform.Modules.Identity.Application.Commands;

public class RegisterResponse {
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class RegisterCommand : IRequest<RegisterResponse> {
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponse>
{
    private readonly IdentityDbContext _context;

    public RegisterCommandHandler(IdentityDbContext context)
    {
        _context = context;
    }

    public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _context.Set<User>().AnyAsync(u => u.Email == request.Email, cancellationToken))
        {
            throw new Exception("Bu e-posta adresi ile zaten kayıtlı bir kullanıcı var.");
        }

        string passwordHash;
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(request.Password));
            passwordHash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }

        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = passwordHash,
            Role = "User"
        };

        _context.Set<User>().Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        return new RegisterResponse { Success = true, Message = "Kayıt başarıyla oluşturuldu." };
    }
}
