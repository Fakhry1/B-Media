using BMedia.Application.Common.Models;
using MediatR;

namespace BMedia.Application.Features.Users.Queries.GetUsers;

public record GetUsersQuery(
    int Page = 1,
    int PageSize = 20,
    string? Search = null,
    bool? IsActive = null
) : IRequest<Result<PaginatedResult<UserListDto>>>;

public record UserListDto(
    Guid Id,
    string Username,
    string Email,
    string FirstName,
    string LastName,
    bool IsActive,
    bool IsEmailVerified,
    DateTime CreatedAt,
    DateTime? LastLoginAt,
    IEnumerable<string> Roles
);
