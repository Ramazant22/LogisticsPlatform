using MediatR;
using LogisticsPlatform.Modules.Identity.Domain;
using LogisticsPlatform.Modules.Identity.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LogisticsPlatform.Modules.Identity.Application;

public class CreateUserCommand : IRequest<Guid>
{
    public Guid TenantId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IdentityDbContext _context;

    public CreateUserCommandHandler(IdentityDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = User.Create(request.TenantId, request.FirstName, request.LastName, request.Email, "hashed_password", request.Role);
        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);
        return user.Id;
    }
}
