using BMedia.Application.Common.Models;
using BMedia.Domain.Entities;
using BMedia.Infrastructure.Persistence;
using BMedia.Infrastructure.Services.Auth;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BMedia.Application.Features.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<UserCreatedResult>>
{
    private readonly ApplicationDbContext _db;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserCommandHandler(ApplicationDbContext db, IPasswordHasher passwordHasher)
    {
        _db = db;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<UserCreatedResult>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var emailExists = await _db.Users.AnyAsync(u => u.Email == request.Email.ToLowerInvariant(), cancellationToken);
        if (emailExists)
            return Result<UserCreatedResult>.Conflict("Email already registered");

        var usernameExists = await _db.Users.AnyAsync(u => u.Username == request.Username.ToLowerInvariant(), cancellationToken);
        if (usernameExists)
            return Result<UserCreatedResult>.Conflict("Username already taken");

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

        if (request.RoleIds?.Any() == true)
        {
            var validRoleIds = await _db.Roles
                .Where(r => request.RoleIds.Contains(r.Id))
                .Select(r => r.Id)
                .ToListAsync(cancellationToken);

            foreach (var roleId in validRoleIds)
            {
                user.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = roleId });
            }
        }

        _db.Users.Add(user);
        await _db.SaveChangesAsync(cancellationToken);

        return Result<UserCreatedResult>.Created(new UserCreatedResult(user.Id, user.Email, user.Username));
    }
}
