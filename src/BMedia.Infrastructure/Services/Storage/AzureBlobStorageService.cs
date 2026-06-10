using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using BMedia.Domain.Interfaces;
using Microsoft.Extensions.Options;

namespace BMedia.Infrastructure.Services.Storage;

public class AzureBlobStorageOptions
{
    public string ConnectionString { get; set; } = string.Empty;
    public string ContainerName { get; set; } = "bmedia-assets";
    public string BaseUrl { get; set; } = string.Empty;
}

public class AzureBlobStorageService : IStorageService
{
    private readonly AzureBlobStorageOptions _options;
    private readonly BlobContainerClient _container;

    public AzureBlobStorageService(IOptions<AzureBlobStorageOptions> options)
    {
        _options = options.Value;
        _container = new BlobContainerClient(_options.ConnectionString, _options.ContainerName);
        _container.CreateIfNotExists(PublicAccessType.None);
    }

    public async Task<StorageUploadResult> UploadAsync(
        Stream fileStream, string fileName, string contentType,
        string? folder = null, CancellationToken cancellationToken = default)
    {
        var blobName = folder is not null
            ? $"{folder}/{Guid.NewGuid()}_{fileName}"
            : $"{Guid.NewGuid()}_{fileName}";

        var blobClient = _container.GetBlobClient(blobName);

        await blobClient.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: cancellationToken);

        var properties = await blobClient.GetPropertiesAsync(cancellationToken: cancellationToken);
        var publicUrl = string.IsNullOrEmpty(_options.BaseUrl)
            ? blobClient.Uri.ToString()
            : $"{_options.BaseUrl.TrimEnd('/')}/{blobName}";

        return new StorageUploadResult(blobName, publicUrl, properties.Value.ContentLength, contentType);
    }

    public async Task DeleteAsync(string storageKey, CancellationToken cancellationToken = default)
    {
        var blobClient = _container.GetBlobClient(storageKey);
        await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }

    public async Task<string> GenerateSignedUrlAsync(string storageKey, TimeSpan expiry, CancellationToken cancellationToken = default)
    {
        var blobClient = _container.GetBlobClient(storageKey);

        if (!blobClient.CanGenerateSasUri)
            return blobClient.Uri.ToString();

        var sasUri = blobClient.GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.UtcNow.Add(expiry));
        return sasUri.ToString();
    }

    public async Task<Stream> DownloadAsync(string storageKey, CancellationToken cancellationToken = default)
    {
        var blobClient = _container.GetBlobClient(storageKey);
        var response = await blobClient.DownloadStreamingAsync(cancellationToken: cancellationToken);
        return response.Value.Content;
    }

    public async Task<bool> ExistsAsync(string storageKey, CancellationToken cancellationToken = default)
    {
        var blobClient = _container.GetBlobClient(storageKey);
        var response = await blobClient.ExistsAsync(cancellationToken);
        return response.Value;
    }

    public string GetPublicUrl(string storageKey)
    {
        if (!string.IsNullOrEmpty(_options.BaseUrl))
            return $"{_options.BaseUrl.TrimEnd('/')}/{storageKey}";

        return _container.GetBlobClient(storageKey).Uri.ToString();
    }
}
