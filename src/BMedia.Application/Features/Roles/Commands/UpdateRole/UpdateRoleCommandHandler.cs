using BMedia.Application.Common.Models;
using BMedia.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BMedia.Application.Features.Roles.Commands.UpdateRole;

public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, Result<bool>>
{
    private readonly ApplicationDbContext _db;

    public UpdateRoleCommandHandler(ApplicationDbContext db) => _db = db;

    public async Task<Result<bool>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _db.Roles.FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);
        if (role is null)
            return Result<bool>.NotFound("Role not found");

        var normalizedName = request.Name.ToUpperInvariant();

        if (role.IsSystem && role.NormalizedName != normalizedName)
            return Result<bool>.Failure("Cannot rename a system role", 400);

        if (role.NormalizedName != normalizedName)
        {
            var nameExists = await _db.Roles.AnyAsync(r => r.NormalizedName == normalizedName && r.Id != request.Id, cancellationToken);
            if (nameExists)
                return Result<bool>.Conflict("A role with this name already exists");
        }

        role.Name = request.Name;
        role.NormalizedName = normalizedName;
        role.Description = request.Description;
        role.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
