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

    public UploadAssetCommandHandler(ApplicationDbContext db, IStorageService storage)
    {
        _db = db;
        _storage = storage;
    }

    public async Task<Result<UploadAssetResult>> Handle(UploadAssetCommand request, CancellationToken cancellationToken)
    {
        if (request.FileSizeBytes > MaxFileSizeBytes)
            return Result<UploadAssetResult>.Failure($"File size exceeds maximum allowed size of {MaxFileSizeBytes / (1024 * 1024 * 1024)}GB");

        if (!AllowedMimeTypes.Contains(request.ContentType.ToLowerInvariant()))
            return Result<UploadAssetResult>.Failure($"File type '{request.ContentType}' is not allowed");

        var folder = request.ContentId.HasValue ? $"content/{request.ContentId}" : "assets";
        var uploadResult = await _storage.UploadAsync(request.FileStream, request.OriginalFileName, request.ContentType, folder, cancellationToken);

        try
        {
            var asset = new MediaAsset
            {
                OriginalFileName = request.OriginalFileName,
                StorageKey = uploadResult.StorageKey,
                PublicUrl = uploadResult.PublicUrl,
                ContentType = request.ContentType,
                FileSizeBytes = uploadResult.FileSizeBytes,
                MediaType = request.MediaType,
                StorageProvider = StorageProvider.AzureBlob,
                Language = request.Language,
                Title = request.Title,
                Description = request.Description,
                AltText = request.AltText,
                IsPrimary = request.IsPrimary,
                SortOrder = request.SortOrder,
                ContentId = request.ContentId,
                Status = MediaAssetStatus.Ready
            };

            _db.MediaAssets.Add(asset);
            await _db.SaveChangesAsync(cancellationToken);

            return Result<UploadAssetResult>.Created(new UploadAssetResult(asset.Id, asset.StorageKey, asset.PublicUrl));
        }
        catch
        {
            // DB save failed — roll back the blob to keep storage consistent
            await _storage.DeleteAsync(uploadResult.StorageKey, CancellationToken.None);
            throw;
        }
    }
}
