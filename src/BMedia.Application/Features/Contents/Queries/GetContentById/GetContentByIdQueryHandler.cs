using BMedia.Application.Common.Models;
using BMedia.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BMedia.Application.Features.Contents.Queries.GetContentById;

public class GetContentByIdQueryHandler : IRequestHandler<GetContentByIdQuery, Result<ContentDetailDto>>
{
    private readonly ApplicationDbContext _db;

    public GetContentByIdQueryHandler(ApplicationDbContext db) => _db = db;

    public async Task<Result<ContentDetailDto>> Handle(GetContentByIdQuery request, CancellationToken cancellationToken)
    {
        var content = await _db.Contents
            .AsNoTracking()
            .Include(c => c.Category)
            .Include(c => c.Subcategory)
            .Include(c => c.CurrentWorkflowStep)
            .Include(c => c.ContentTags).ThenInclude(ct => ct.Tag)
            .Include(c => c.MediaAssets.Where(m => !m.IsDeleted)).ThenInclude(m => m.Versions)
            .Include(c => c.Localizations.Where(l => !l.IsDeleted))
            .FirstOrDefaultAsync(c => c.Id == request.ContentId, cancellationToken);

        if (content is null) return Result<ContentDetailDto>.NotFound("Content not found");

        var dto = new ContentDetailDto(
            content.Id,
            content.Title,
            content.Slug,
            content.Summary,
            content.Language,
            content.Status,
            content.IsFeatured,
            content.AllowComments,
            content.ViewCount,
            content.CreatedAt,
            content.PublishedAt,
            content.ScheduledPublishAt,
            content.CategoryId,
            content.Category?.Name,
            content.SubcategoryId,
            content.Subcategory?.Name,
            content.CurrentWorkflowStep?.Name,
            content.MediaAssets.OrderBy(m => m.SortOrder).Select(m => new MediaAssetDto(
                m.Id, m.OriginalFileName, m.ContentType, m.MediaType, m.Status,
                m.PublicUrl, m.ThumbnailUrl, m.IsPrimary, m.SortOrder, m.FileSizeBytes)),
            content.ContentTags.Select(ct => ct.Tag.Name),
            content.Localizations.Select(l => new LocalizationDto(l.Id, l.Language, l.Title, l.Summary, l.IsApproved))
        );

        return Result<ContentDetailDto>.Success(dto);
    }
}
