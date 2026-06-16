using BMedia.Application.Common.Models;
using BMedia.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BMedia.Application.Features.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserDetailDto>>
{
    private readonly ApplicationDbContext _db;

    public GetUserByIdQueryHandler(ApplicationDbContext db) => _db = db;

    public async Task<Result<UserDetailDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _db.Users
            .AsNoTracking()
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        if (user is null)
            return Result<UserDetailDto>.NotFound("User not found");

        var dto = new UserDetailDto(
            user.Id,
            user.Username,
            user.Email,
            user.FirstName,
            user.LastName,
            user.PhoneNumber,
            user.IsActive,
            user.IsEmailVerified,
            user.CreatedAt,
            user.LastLoginAt,
            user.PreferredLanguage,
            user.UserRoles.Select(ur => new RoleSimpleDto(ur.Role.Id, ur.Role.Name, ur.Role.Description))
        );

        return Result<UserDetailDto>.Success(dto);
    }
}
