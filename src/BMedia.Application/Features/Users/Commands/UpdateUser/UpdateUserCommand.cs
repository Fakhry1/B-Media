using BMedia.Application.Common.Models;
using MediatR;

namespace BMedia.Application.Features.Users.Commands.UpdateUser;

public record UpdateUserCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string? PhoneNumber,
    string PreferredLanguage,
    bool IsActive
) : IRequest<Result<bool>>;
