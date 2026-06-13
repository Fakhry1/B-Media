using BMedia.Application.Common.Models;
using BMedia.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BMedia.Application.Features.Contents.Queries.GetContents;

public class GetContentsQueryHandler : IRequestHandler<GetContentsQuery, Result<PaginatedResult<ContentListDto>>>
{
    private readonly ApplicationDbContext _db;

    public GetContentsQueryHandler(ApplicationDbContext db) => _db = db;

    public async Task<Result<PaginatedResult<ContentListDto>>> Handle(GetContentsQuery request, CancellationToken cancellationToken)
    {
        var query = _db.Contents
            .AsNoTracking()
            .Include(c => c.Category)
            .Include(c => c.ContentTags).ThenInclude(ct => ct.Tag)
            .Include(c => c.MediaAssets.Where(m => m.IsPrimary && !m.IsDeleted))
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(c => c.Title.Contains(request.Search) || (c.Summary != null && c.Summary.Contains(request.Search)));

        if (request.Status.HasValue)
            query = query.Where(c => c.Status == request.Status.Value);

        if (request.CategoryId.HasValue)
            query = query.Where(c => c.CategoryId == request.CategoryId.Value);

        if (!string.IsNullOrWhiteSpace(request.Language))
            query = query.Where(c => c.Language == request.Language);

        if (request.IsFeatured.HasValue)
            query = query.Where(c => c.IsFeatured == request.IsFeatured.Value);

        if (request.SubcategoryId.HasValue)
            query = query.Where(c => c.SubcategoryId == request.SubcategoryId.Value);

        if (request.MediaType.HasValue)
            query = query.Where(c => c.MediaAssets.Any(m => m.MediaType == request.MediaType.Value && !m.IsDeleted));

        query = request.SortBy?.ToLower() switch
        {
            "title" => request.SortDescending ? query.OrderByDescending(c => c.Title) : query.OrderBy(c => c.Title),
            "publishedat" => request.SortDescending ? query.OrderByDescending(c => c.PublishedAt) : query.OrderBy(c => c.PublishedAt),
            "viewcount" => request.SortDescending ? query.OrderByDescending(c => c.ViewCount) : query.OrderBy(c => c.ViewCount),
            _ => request.SortDescending ? query.OrderByDescending(c => c.CreatedAt) : query.OrderBy(c => c.CreatedAt)
        };

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(c => new ContentListDto(
                c.Id,
                c.Title,
                c.Slug,
                c.Summary,
                c.Language,
                c.Status,
                c.IsFeatured,
                c.CreatedAt,
                c.PublishedAt,
                c.Category != null ? c.Category.Name : null,
                c.MediaAssets.Where(m => m.IsPrimary).Select(m => m.ThumbnailUrl).FirstOrDefault(),
                c.ContentTags.Select(ct => ct.Tag.Name)
            ))
            .ToListAsync(cancellationToken);

        return Result<PaginatedResult<ContentListDto>>.Success(new PaginatedResult<ContentListDto>(items, totalCount, request.Page, request.PageSize));
    }
}
