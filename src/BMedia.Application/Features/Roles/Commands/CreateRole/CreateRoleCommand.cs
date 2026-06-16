using BMedia.Application.Common.Models;
using MediatR;

namespace BMedia.Application.Features.Roles.Commands.CreateRole;

public record CreateRoleCommand(string Name, string? Description) : IRequest<Result<RoleCreatedResult>>;

public record RoleCreatedResult(Guid Id, string Name);
