using BMedia.Application.Common.Models;
using BMedia.Domain.Enums;
using MediatR;

namespace BMedia.Application.Features.MediaAssets.Commands.UploadAsset;

public record UploadAssetCommand(
    Stream FileStream,
    string OriginalFileName,
    string ContentType,
    long FileSizeBytes,
    Guid? ContentId,
    MediaType MediaType,
    string Language,
    string? Title,
    string? Description,
    string? AltText,
    bool IsPrimary,
    int SortOrder
) : IRequest<Result<UploadAssetResult>>;

public record UploadAssetResult(Guid AssetId, string StorageKey, string? PublicUrl);
