using BMedia.Application.Common.Models;
using MediatR;

namespace BMedia.Application.Features.Users.Commands.AssignRoles;

public record AssignUserRolesCommand(Guid UserId, IEnumerable<Guid> RoleIds) : IRequest<Result<bool>>;
