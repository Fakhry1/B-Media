namespace BMedia.Domain.Interfaces;

public interface IMediaProcessingService
{
    Task EnqueueTranscodingAsync(Guid mediaAssetId, CancellationToken cancellationToken = default);
    Task EnqueueThumbnailGenerationAsync(Guid mediaAssetId, CancellationToken cancellationToken = default);
    Task EnqueueMetadataExtractionAsync(Guid mediaAssetId, CancellationToken cancellationToken = default);
    Task EnqueueOcrAsync(Guid mediaAssetId, CancellationToken cancellationToken = default);
    Task EnqueueAiTaggingAsync(Guid mediaAssetId, CancellationToken cancellationToken = default);
    Task EnqueueAntivirusScanAsync(Guid mediaAssetId, CancellationToken cancellationToken = default);
}
