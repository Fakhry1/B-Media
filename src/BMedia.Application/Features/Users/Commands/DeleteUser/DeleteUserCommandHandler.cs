using BMedia.Application.Common.Models;
using BMedia.Domain.Interfaces;
using BMedia.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BMedia.Application.Features.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result<bool>>
{
    private readonly ApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public DeleteUserCommandHandler(ApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<Result<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        if (_currentUser.UserId == request.Id)
            return Result<bool>.Failure("You cannot delete your own account", 400);

        var user = await _db.Users
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        if (user is null)
            return Result<bool>.NotFound("User not found");

        var hasOnlySystemRoles = user.UserRoles.Any() && user.UserRoles.All(ur => ur.Role.IsSystem);
        if (hasOnlySystemRoles)
            return Result<bool>.Failure("Cannot delete a user assigned exclusively to system roles", 400);

        user.IsDeleted = true;
        user.DeletedAt = DateTime.UtcNow;
        user.DeletedBy = _currentUser.UserId;
        user.UpdatedAt = DateTime.UtcNow;
        user.UpdatedBy = _currentUser.UserId;

        await _db.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
