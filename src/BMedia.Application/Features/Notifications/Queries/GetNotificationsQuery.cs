using BMedia.Application.Common.Interfaces;
using BMedia.Application.Common.Models;
using BMedia.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BMedia.Application.Features.Notifications.Queries;

public record GetNotificationsQuery(bool UnreadOnly = false, int Page = 1, int PageSize = 20) : IRequest<Result<PaginatedResult<NotificationDto>>>;

public record NotificationDto(
    Guid Id,
    NotificationType Type,
    string Title,
    string Message,
    bool IsRead,
    DateTime? ReadAt,
    Guid? ReferenceId,
    string? ActionUrl,
    DateTime CreatedAt
);

public class GetNotificationsQueryHandler : IRequestHandler<GetNotificationsQuery, Result<PaginatedResult<NotificationDto>>>
{
    private readonly Infrastructure.Persistence.ApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public GetNotificationsQueryHandler(Infrastructure.Persistence.ApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<Result<PaginatedResult<NotificationDto>>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
    {
        if (!_currentUser.UserId.HasValue) return Result<PaginatedResult<NotificationDto>>.Unauthorized();

        var query = _db.Notifications.AsNoTracking().Where(n => n.UserId == _currentUser.UserId.Value);

        if (request.UnreadOnly) query = query.Where(n => !n.IsRead);

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(n => n.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize).Take(request.PageSize)
            .Select(n => new NotificationDto(n.Id, n.Type, n.Title, n.Message, n.IsRead, n.ReadAt, n.ReferenceId, n.ActionUrl, n.CreatedAt))
            .ToListAsync(cancellationToken);

        return Result<PaginatedResult<NotificationDto>>.Success(new PaginatedResult<NotificationDto>(items, total, request.Page, request.PageSize));
    }
}
