using BMedia.Application.Common.Models;
using MediatR;

namespace BMedia.Application.Features.Roles.Commands.DeleteRole;

public record DeleteRoleCommand(Guid Id) : IRequest<Result<bool>>;
