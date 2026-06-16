using BMedia.Application.Common.Models;
using MediatR;

namespace BMedia.Application.Features.Users.Queries.GetUserById;

public record GetUserByIdQuery(Guid Id) : IRequest<Result<UserDetailDto>>;

public record UserDetailDto(
    Guid Id,
    string Username,
    string Email,
    string FirstName,
    string LastName,
    string? PhoneNumber,
    bool IsActive,
    bool IsEmailVerified,
    DateTime CreatedAt,
    DateTime? LastLoginAt,
    string PreferredLanguage,
    IEnumerable<RoleSimpleDto> Roles
);

public record RoleSimpleDto(Guid Id, string Name, string? Description);
