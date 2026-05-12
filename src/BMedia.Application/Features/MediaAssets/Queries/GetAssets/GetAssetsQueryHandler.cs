using BMedia.Application.Common.Models;
using BMedia.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BMedia.Application.Features.MediaAssets.Queries.GetAssets;

public class GetAssetsQueryHandler : IRequestHandler<GetAssetsQuery, Result<PaginatedResult<MediaAssetListDto>>>
{
    private readonly ApplicationDbContext _db;

    public GetAssetsQueryHandler(ApplicationDbContext db) => _db = db;

    public async Task<Result<PaginatedResult<MediaAssetListDto>>> Handle(GetAssetsQuery request, CancellationToken cancellationToken)
    {
        var query = _db.MediaAssets.AsNoTracking().AsQueryable();

        if (request.ContentId.HasValue) query = query.Where(m => m.ContentId == request.ContentId.Value);
        if (request.MediaType.HasValue) query = query.Where(m => m.MediaType == request.MediaType.Value);
        if (request.Status.HasValue) query = query.Where(m => m.Status == request.Status.Value);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(m => m.SortOrder).ThenByDescending(m => m.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(m => new MediaAssetListDto(
                m.Id, m.OriginalFileName, m.ContentType, m.MediaType, m.Status,
                m.PublicUrl, m.CdnUrl, m.ThumbnailUrl, m.HlsManifestUrl,
                m.IsPrimary, m.FileSizeBytes, m.DurationSeconds,
                m.Width, m.Height, m.CreatedAt, m.IsTranscodingComplete))
            .ToListAsync(cancellationToken);

        return Result<PaginatedResult<MediaAssetListDto>>.Success(
            new PaginatedResult<MediaAssetListDto>(items, totalCount, request.Page, request.PageSize));
    }
}
