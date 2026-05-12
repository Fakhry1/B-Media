using BMedia.Application.Common.Models;
using BMedia.Domain.Entities;
using BMedia.Infrastructure.Persistence;
using BMedia.Infrastructure.Services.Auth;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BMedia.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<RegisterResult>>
{
    private readonly ApplicationDbContext _db;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterCommandHandler(ApplicationDbContext db, IPasswordHasher passwordHasher)
    {
        _db = db;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<RegisterResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var emailExists = await _db.Users.AnyAsync(u => u.Email == request.Email.ToLowerInvariant(), cancellationToken);
        if (emailExists) return Result<RegisterResult>.Conflict("Email already registered");

        var usernameExists = await _db.Users.AnyAsync(u => u.Username == request.Username.ToLowerInvariant(), cancellationToken);
        if (usernameExists) return Result<RegisterResult>.Conflict("Username already taken");

        var user = new User
        {
            Username = request.Username.ToLowerInvariant(),
            Email = request.Email.ToLowerInvariant(),
            PasswordHash = _passwordHasher.Hash(request.Password),
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            PreferredLanguage = request.PreferredLanguage
        };

        // Assign ContentCreator role by default
        var contentCreatorRole = await _db.Roles
            .FirstOrDefaultAsync(r => r.NormalizedName == "CONTENTCREATOR", cancellationToken);

        if (contentCreatorRole is not null)
        {
            user.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = contentCreatorRole.Id });
        }

        _db.Users.Add(user);
        await _db.SaveChangesAsync(cancellationToken);

        return Result<RegisterResult>.Created(new RegisterResult(user.Id, user.Email, user.Username));
    }
}
