using BMedia.Application.Common.Models;
using BMedia.Domain.Entities;
using BMedia.Domain.Enums;
using BMedia.Domain.Interfaces;
using BMedia.Infrastructure.Persistence;
using MediatR;

namespace BMedia.Application.Features.MediaAssets.Commands.UploadAsset;

public class UploadAssetCommandHandler : IRequestHandler<UploadAssetCommand, Result<UploadAssetResult>>
{
    private static readonly long MaxFileSizeBytes = 5L * 1024 * 1024 * 1024; // 5GB

    private static readonly HashSet<string> AllowedMimeTypes =
    [
        "video/mp4", "video/mpeg", "video/quicktime", "video/x-msvideo", "video/webm",
        "image/jpeg", "image/png", "image/webp", "image/gif", "image/svg+xml",
        "audio/mpeg", "audio/ogg", "audio/wav", "audio/webm",
        "application/pdf",
        "text/vtt", "text/plain"
    ];

    private readonly ApplicationDbContext _db;
    private readonly IStorageService _storage;
    private readonly IMediaProcessingService _mediaProcessing;

    public UploadAssetCommandHandler(ApplicationDbContext db, IStorageService storage, IMediaProcessingService mediaProcessing)
    {
        _db = db;
        _storage = storage;
        _mediaProcessing = mediaProcessing;
    }

    public async Task<Result<UploadAssetResult>> Handle(UploadAssetCommand request, CancellationToken cancellationToken)
    {
        if (request.FileSizeBytes > MaxFileSizeBytes)
            return Result<UploadAssetResult>.Failure($"File size exceeds maximum allowed size of {MaxFileSizeBytes / (1024 * 1024 * 1024)}GB");

        if (!AllowedMimeTypes.Contains(request.ContentType.ToLowerInvariant()))
            return Result<UploadAssetResult>.Failure($"File type '{request.ContentType}' is not allowed");

        var folder = request.ContentId.HasValue ? $"content/{request.ContentId}" : "assets";
        var uploadResult = await _storage.UploadAsync(request.FileStream, request.OriginalFileName, request.ContentType, folder, cancellationToken);

        var asset = new MediaAsset
        {
            OriginalFileName = request.OriginalFileName,
            StorageKey = uploadResult.StorageKey,
            PublicUrl = uploadResult.PublicUrl,
            ContentType = request.ContentType,
            FileSizeBytes = uploadResult.FileSizeBytes,
            MediaType = request.MediaType,
            Language = request.Language,
            Title = request.Title,
            Description = request.Description,
            AltText = request.AltText,
            IsPrimary = request.IsPrimary,
            SortOrder = request.SortOrder,
            ContentId = request.ContentId,
            Status = MediaAssetStatus.Pending
        };

        _db.MediaAssets.Add(asset);
        await _db.SaveChangesAsync(cancellationToken);

        // Enqueue async processing pipeline — fire and forget (non-blocking)
        await _mediaProcessing.EnqueueAntivirusScanAsync(asset.Id, cancellationToken);
        await _mediaProcessing.EnqueueMetadataExtractionAsync(asset.Id, cancellationToken);

        if (request.MediaType == MediaType.Video)
        {
            await _mediaProcessing.EnqueueTranscodingAsync(asset.Id, cancellationToken);
            await _mediaProcessing.EnqueueThumbnailGenerationAsync(asset.Id, cancellationToken);
        }

        if (request.MediaType is MediaType.Image)
            await _mediaProcessing.EnqueueThumbnailGenerationAsync(asset.Id, cancellationToken);

        if (request.MediaType is MediaType.PDF or MediaType.Document)
            await _mediaProcessing.EnqueueOcrAsync(asset.Id, cancellationToken);

        return Result<UploadAssetResult>.Created(new UploadAssetResult(asset.Id, asset.StorageKey, asset.PublicUrl));
    }
}
