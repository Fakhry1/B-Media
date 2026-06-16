using BMedia.Application.Common.Models;
using BMedia.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BMedia.Application.Features.Users.Queries.GetUsers;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, Result<PaginatedResult<UserListDto>>>
{
    private readonly ApplicationDbContext _db;

    public GetUsersQueryHandler(ApplicationDbContext db) => _db = db;

    public async Task<Result<PaginatedResult<UserListDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var query = _db.Users
            .AsNoTracking()
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();
            query = query.Where(u =>
                u.Email.Contains(search) ||
                u.Username.Contains(search) ||
                u.FirstName.Contains(search) ||
                u.LastName.Contains(search));
        }

        if (request.IsActive.HasValue)
            query = query.Where(u => u.IsActive == request.IsActive.Value);

        query = query.OrderByDescending(u => u.CreatedAt);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(u => new UserListDto(
                u.Id,
                u.Username,
                u.Email,
                u.FirstName,
                u.LastName,
                u.IsActive,
                u.IsEmailVerified,
                u.CreatedAt,
                u.LastLoginAt,
                u.UserRoles.Select(ur => ur.Role.Name)
            ))
            .ToListAsync(cancellationToken);

        return Result<PaginatedResult<UserListDto>>.Success(
            new PaginatedResult<UserListDto>(items, totalCount, request.Page, request.PageSize));
    }
}
