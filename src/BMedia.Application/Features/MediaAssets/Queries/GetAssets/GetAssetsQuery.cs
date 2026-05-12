using BMedia.Application.Common.Models;
using BMedia.Domain.Enums;
using MediatR;

namespace BMedia.Application.Features.MediaAssets.Queries.GetAssets;

public record GetAssetsQuery(
    Guid? ContentId = null,
    MediaType? MediaType = null,
    MediaAssetStatus? Status = null,
    int Page = 1,
    int PageSize = 20
) : IRequest<Result<PaginatedResult<MediaAssetListDto>>>;

public record MediaAssetListDto(
    Guid Id,
    string OriginalFileName,
    string ContentType,
    MediaType MediaType,
    MediaAssetStatus Status,
    string? PublicUrl,
    string? CdnUrl,
    string? ThumbnailUrl,
    string? HlsManifestUrl,
    bool IsPrimary,
    long FileSizeBytes,
    int? DurationSeconds,
    int? Width,
    int? Height,
    DateTime CreatedAt,
    bool IsTranscodingComplete
);
