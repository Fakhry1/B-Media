using BMedia.Application.Common.Models;
using BMedia.Domain.Enums;
using MediatR;

namespace BMedia.Application.Features.Contents.Queries.GetContentById;

public record GetContentByIdQuery(Guid ContentId) : IRequest<Result<ContentDetailDto>>;

public record ContentDetailDto(
    Guid Id,
    string Title,
    string Slug,
    string? Summary,
    string? Body,
    string Language,
    ContentStatus Status,
    bool IsFeatured,
    bool AllowComments,
    int ViewCount,
    string? SeoTitle,
    string? SeoDescription,
    string? SeoKeywords,
    DateTime CreatedAt,
    DateTime? PublishedAt,
    DateTime? ScheduledPublishAt,
    Guid? CategoryId,
    string? CategoryName,
    Guid? SubcategoryId,
    string? SubcategoryName,
    string? CurrentWorkflowStep,
    IEnumerable<MediaAssetDto> MediaAssets,
    IEnumerable<string> Tags,
    IEnumerable<LocalizationDto> Localizations
);

public record MediaAssetDto(
    Guid Id,
    string OriginalFileName,
    string ContentType,
    MediaType MediaType,
    MediaAssetStatus Status,
    string? PublicUrl,
    string? CdnUrl,
    string? ThumbnailUrl,
    bool IsPrimary,
    int SortOrder,
    long FileSizeBytes
);

public record LocalizationDto(
    Guid Id,
    string Language,
    string Title,
    string? Summary,
    bool IsApproved
);
