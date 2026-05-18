using BMedia.Domain.Interfaces;
using BMedia.Infrastructure.BackgroundJobs;
using Hangfire;
using Microsoft.Extensions.Logging;

namespace BMedia.Infrastructure.Services.Media;

public class MediaProcessingService : IMediaProcessingService
{
    private readonly IBackgroundJobClient _jobClient;
    private readonly ILogger<MediaProcessingService> _logger;

    public MediaProcessingService(IBackgroundJobClient jobClient, ILogger<MediaProcessingService> logger)
    {
        _jobClient = jobClient;
        _logger = logger;
    }

    public Task EnqueueTranscodingAsync(Guid mediaAssetId, CancellationToken cancellationToken = default)
    {
        _jobClient.Enqueue<MediaProcessingJobs>(j => j.TranscodeVideoAsync(mediaAssetId, CancellationToken.None));
        _logger.LogInformation("Transcoding job enqueued for asset {AssetId}", mediaAssetId);
        return Task.CompletedTask;
    }

    public Task EnqueueThumbnailGenerationAsync(Guid mediaAssetId, CancellationToken cancellationToken = default)
    {
        _jobClient.Enqueue<MediaProcessingJobs>(j => j.GenerateThumbnailAsync(mediaAssetId, CancellationToken.None));
        return Task.CompletedTask;
    }

    public Task EnqueueMetadataExtractionAsync(Guid mediaAssetId, CancellationToken cancellationToken = default)
    {
        _jobClient.Enqueue<MediaProcessingJobs>(j => j.ExtractMetadataAsync(mediaAssetId, CancellationToken.None));
        return Task.CompletedTask;
    }

    public Task EnqueueOcrAsync(Guid mediaAssetId, CancellationToken cancellationToken = default)
    {
        _jobClient.Enqueue<MediaProcessingJobs>(j => j.RunOcrAsync(mediaAssetId, CancellationToken.None));
        return Task.CompletedTask;
    }

    public Task EnqueueAiTaggingAsync(Guid mediaAssetId, CancellationToken cancellationToken = default)
    {
        _jobClient.Enqueue<MediaProcessingJobs>(j => j.RunAiTaggingAsync(mediaAssetId, CancellationToken.None));
        return Task.CompletedTask;
    }

    public Task EnqueueAntivirusScanAsync(Guid mediaAssetId, CancellationToken cancellationToken = default)
    {
        _jobClient.Enqueue<MediaProcessingJobs>(j => j.RunAntivirusScanAsync(mediaAssetId, CancellationToken.None));
        return Task.CompletedTask;
    }
}
