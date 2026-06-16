using BMedia.Application.Common.Models;
using MediatR;

namespace BMedia.Application.Features.Users.Commands.ResetPassword;

public record ResetUserPasswordCommand(Guid UserId, string NewPassword) : IRequest<Result<bool>>;
