using BMedia.Application.Common.Models;
using BMedia.Domain.Entities;
using BMedia.Domain.Interfaces;
using BMedia.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BMedia.Application.Features.Users.Commands.AssignRoles;

public class AssignUserRolesCommandHandler : IRequestHandler<AssignUserRolesCommand, Result<bool>>
{
    private readonly ApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public AssignUserRolesCommandHandler(ApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<Result<bool>> Handle(AssignUserRolesCommand request, CancellationToken cancellationToken)
    {
        var user = await _db.Users
            .Include(u => u.UserRoles)
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null)
            return Result<bool>.NotFound("User not found");

        var requestedIds = request.RoleIds?.ToHashSet() ?? [];

        // Validate that all requested IDs actually exist.
        var validRoleIds = requestedIds.Count > 0
            ? (await _db.Roles
                .Where(r => requestedIds.Contains(r.Id))
                .Select(r => r.Id)
                .ToListAsync(cancellationToken)).ToHashSet()
            : [];

        var currentIds = user.UserRoles.Select(ur => ur.RoleId).ToHashSet();

        // Remove only roles that are no longer in the requested set.
        var toRemove = user.UserRoles.Where(ur => !validRoleIds.Contains(ur.RoleId)).ToList();
        _db.UserRoles.RemoveRange(toRemove);

        // Add only roles not already assigned.
        foreach (var roleId in validRoleIds.Where(id => !currentIds.Contains(id)))
        {
            _db.UserRoles.Add(new UserRole
            {
                UserId = user.Id,
                RoleId = roleId,
                AssignedAt = DateTime.UtcNow,
                AssignedBy = _currentUser.UserId
            });
        }

        user.UpdatedAt = DateTime.UtcNow;
        user.UpdatedBy = _currentUser.UserId;

        await _db.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
