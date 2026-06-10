using BMedia.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;

namespace BMedia.Infrastructure.BackgroundJobs;

public class MediaProcessingJobs
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<MediaProcessingJobs> _logger;

    public MediaProcessingJobs(ApplicationDbContext db, ILogger<MediaProcessingJobs> logger)
    {
        _db = db;
        _logger = logger;
    }

    public Task TranscodeVideoAsync(Guid mediaAssetId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Transcoding job stub for asset {AssetId}", mediaAssetId);
        return Task.CompletedTask;
    }

    public Task GenerateThumbnailAsync(Guid mediaAssetId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Thumbnail generation stub for asset {AssetId}", mediaAssetId);
        return Task.CompletedTask;
    }

    public Task ExtractMetadataAsync(Guid mediaAssetId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Metadata extraction stub for asset {AssetId}", mediaAssetId);
        return Task.CompletedTask;
    }

    public Task RunOcrAsync(Guid mediaAssetId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("OCR stub for asset {AssetId}", mediaAssetId);
        return Task.CompletedTask;
    }

    public Task RunAiTaggingAsync(Guid mediaAssetId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("AI tagging stub for asset {AssetId}", mediaAssetId);
        return Task.CompletedTask;
    }

    public Task RunAntivirusScanAsync(Guid mediaAssetId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Antivirus scan stub for asset {AssetId}", mediaAssetId);
        return Task.CompletedTask;
    }
}
