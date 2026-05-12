using BMedia.Application.Common.Models;
using MediatR;

namespace BMedia.Application.Features.Auth.Commands.Login;

public record LoginCommand(string Email, string Password, string? IpAddress) : IRequest<Result<LoginResult>>;

public record LoginResult(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt,
    UserInfo User
);

public record UserInfo(
    Guid Id,
    string Email,
    string Username,
    string FirstName,
    string LastName,
    IEnumerable<string> Roles,
    IEnumerable<string> Permissions
);
