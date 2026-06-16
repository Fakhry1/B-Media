using BMedia.Application.Common.Models;
using MediatR;

namespace BMedia.Application.Features.Roles.Commands.UpdateRole;

public record UpdateRoleCommand(Guid Id, string Name, string? Description) : IRequest<Result<bool>>;
