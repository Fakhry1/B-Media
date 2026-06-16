using BMedia.Application.Common.Models;
using BMedia.Domain.Interfaces;
using BMedia.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BMedia.Application.Features.Roles.Commands.DeleteRole;

public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, Result<bool>>
{
    private readonly ApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public DeleteRoleCommandHandler(ApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<Result<bool>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _db.Roles.FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);
        if (role is null)
            return Result<bool>.NotFound("Role not found");

        if (role.IsSystem)
            return Result<bool>.Failure("Cannot delete a system role", 400);

        role.IsDeleted = true;
        role.DeletedAt = DateTime.UtcNow;
        role.DeletedBy = _currentUser.UserId;
        role.UpdatedAt = DateTime.UtcNow;
        role.UpdatedBy = _currentUser.UserId;

        await _db.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
