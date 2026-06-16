using BMedia.Application.Common.Models;
using MediatR;

namespace BMedia.Application.Features.Users.Commands.DeleteUser;

public record DeleteUserCommand(Guid Id) : IRequest<Result<bool>>;
