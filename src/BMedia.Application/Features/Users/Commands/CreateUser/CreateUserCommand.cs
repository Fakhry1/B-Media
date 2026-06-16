using BMedia.Application.Common.Models;
using MediatR;

namespace BMedia.Application.Features.Users.Commands.CreateUser;

public record CreateUserCommand(
    string Username,
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string? PhoneNumber,
    string PreferredLanguage,
    IEnumerable<Guid>? RoleIds
) : IRequest<Result<UserCreatedResult>>;

public record UserCreatedResult(Guid Id, string Email, string Username);
