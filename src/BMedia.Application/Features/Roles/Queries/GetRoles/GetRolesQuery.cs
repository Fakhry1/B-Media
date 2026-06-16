using BMedia.Application.Common.Models;
using MediatR;

namespace BMedia.Application.Features.Roles.Queries.GetRoles;

public record GetRolesQuery(string? Search = null) : IRequest<Result<IEnumerable<RoleDto>>>;

public record RoleDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsSystem,
    int UserCount,
    DateTime CreatedAt
);
