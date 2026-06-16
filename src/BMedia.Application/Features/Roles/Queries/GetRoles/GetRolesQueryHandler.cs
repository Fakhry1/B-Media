using BMedia.Application.Common.Models;
using BMedia.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BMedia.Application.Features.Roles.Queries.GetRoles;

public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, Result<IEnumerable<RoleDto>>>
{
    private readonly ApplicationDbContext _db;

    public GetRolesQueryHandler(ApplicationDbContext db) => _db = db;

    public async Task<Result<IEnumerable<RoleDto>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var query = _db.Roles
            .AsNoTracking()
            .Include(r => r.UserRoles)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();
            query = query.Where(r => r.Name.Contains(search) || (r.Description != null && r.Description.Contains(search)));
        }

        query = query.OrderBy(r => r.Name);

        var roles = await query
            .Select(r => new RoleDto(
                r.Id,
                r.Name,
                r.Description,
                r.IsSystem,
                r.UserRoles.Count,
                r.CreatedAt
            ))
            .ToListAsync(cancellationToken);

        return Result<IEnumerable<RoleDto>>.Success(roles);
    }
}
