using BMedia.Application.Common.Models;
using BMedia.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BMedia.Application.Features.AuditLogs.Queries;

public record GetAuditLogsQuery(
    Guid? UserId = null,
    string? EntityType = null,
    AuditAction? Action = null,
    DateTime? From = null,
    DateTime? To = null,
    int Page = 1,
    int PageSize = 50
) : IRequest<Result<PaginatedResult<AuditLogDto>>>;

public record AuditLogDto(
    Guid Id,
    Guid? UserId,
    string? UserEmail,
    AuditAction Action,
    string EntityType,
    string? EntityId,
    bool IsSuccessful,
    string? IpAddress,
    DateTime CreatedAt
);

public class GetAuditLogsQueryHandler : IRequestHandler<GetAuditLogsQuery, Result<PaginatedResult<AuditLogDto>>>
{
    private readonly Infrastructure.Persistence.ApplicationDbContext _db;

    public GetAuditLogsQueryHandler(Infrastructure.Persistence.ApplicationDbContext db) => _db = db;

    public async Task<Result<PaginatedResult<AuditLogDto>>> Handle(GetAuditLogsQuery request, CancellationToken cancellationToken)
    {
        var query = _db.AuditLogs.AsNoTracking().IgnoreQueryFilters().AsQueryable();

        if (request.UserId.HasValue) query = query.Where(a => a.UserId == request.UserId.Value);
        if (!string.IsNullOrWhiteSpace(request.EntityType)) query = query.Where(a => a.EntityType == request.EntityType);
        if (request.Action.HasValue) query = query.Where(a => a.Action == request.Action.Value);
        if (request.From.HasValue) query = query.Where(a => a.CreatedAt >= request.From.Value);
        if (request.To.HasValue) query = query.Where(a => a.CreatedAt <= request.To.Value);

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(a => a.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize).Take(request.PageSize)
            .Select(a => new AuditLogDto(a.Id, a.UserId, a.UserEmail, a.Action, a.EntityType, a.EntityId, a.IsSuccessful, a.IpAddress, a.CreatedAt))
            .ToListAsync(cancellationToken);

        return Result<PaginatedResult<AuditLogDto>>.Success(new PaginatedResult<AuditLogDto>(items, total, request.Page, request.PageSize));
    }
}
