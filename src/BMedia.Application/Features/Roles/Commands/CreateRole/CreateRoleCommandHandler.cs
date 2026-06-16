using BMedia.Application.Common.Models;
using BMedia.Domain.Entities;
using BMedia.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BMedia.Application.Features.Roles.Commands.CreateRole;

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Result<RoleCreatedResult>>
{
    private readonly ApplicationDbContext _db;

    public CreateRoleCommandHandler(ApplicationDbContext db) => _db = db;

    public async Task<Result<RoleCreatedResult>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var normalizedName = request.Name.ToUpperInvariant();

        var nameExists = await _db.Roles.AnyAsync(r => r.NormalizedName == normalizedName, cancellationToken);
        if (nameExists)
            return Result<RoleCreatedResult>.Conflict("A role with this name already exists");

        var role = new Role
        {
            Name = request.Name,
            NormalizedName = normalizedName,
            Description = request.Description,
            IsSystem = false
        };

        _db.Roles.Add(role);
        await _db.SaveChangesAsync(cancellationToken);

        return Result<RoleCreatedResult>.Created(new RoleCreatedResult(role.Id, role.Name));
    }
}
