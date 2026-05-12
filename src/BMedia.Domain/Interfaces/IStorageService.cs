namespace BMedia.Domain.Interfaces;

public interface IStorageService
{
    Task<StorageUploadResult> UploadAsync(Stream fileStream, string fileName, string contentType, string? folder = null, CancellationToken cancellationToken = default);
    Task DeleteAsync(string storageKey, CancellationToken cancellationToken = default);
    Task<string> GenerateSignedUrlAsync(string storageKey, TimeSpan expiry, CancellationToken cancellationToken = default);
    Task<Stream> DownloadAsync(string storageKey, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string storageKey, CancellationToken cancellationToken = default);
    string GetPublicUrl(string storageKey);
}

public record StorageUploadResult(
    string StorageKey,
    string PublicUrl,
    long FileSizeBytes,
    string ContentType
);
