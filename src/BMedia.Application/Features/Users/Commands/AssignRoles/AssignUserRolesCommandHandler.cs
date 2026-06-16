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

        _db.UserRoles.RemoveRange(user.UserRoles);

        if (request.RoleIds?.Any() == true)
        {
            var validRoleIds = await _db.Roles
                .Where(r => request.RoleIds.Contains(r.Id))
                .Select(r => r.Id)
                .ToListAsync(cancellationToken);

            foreach (var roleId in validRoleIds)
            {
                _db.UserRoles.Add(new UserRole
                {
                    UserId = user.Id,
                    RoleId = roleId,
                    AssignedAt = DateTime.UtcNow,
                    AssignedBy = _currentUser.UserId
                });
            }
        }

        user.UpdatedAt = DateTime.UtcNow;
        user.UpdatedBy = _currentUser.UserId;

        await _db.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
