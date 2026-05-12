using BMedia.Domain.Interfaces;
using Microsoft.Extensions.Options;

namespace BMedia.Infrastructure.Services.Storage;

public class LocalStorageOptions
{
    public string BasePath { get; set; } = "uploads";
    public string BaseUrl { get; set; } = "http://localhost:5000/files";
}

public class LocalStorageService : IStorageService
{
    private readonly LocalStorageOptions _options;

    public LocalStorageService(IOptions<LocalStorageOptions> options)
    {
        _options = options.Value;
        Directory.CreateDirectory(_options.BasePath);
    }

    public async Task<StorageUploadResult> UploadAsync(Stream fileStream, string fileName, string contentType,
        string? folder = null, CancellationToken cancellationToken = default)
    {
        var storageKey = folder is not null
            ? Path.Combine(folder, $"{Guid.NewGuid()}_{fileName}")
            : $"{Guid.NewGuid()}_{fileName}";

        var fullPath = Path.Combine(_options.BasePath, storageKey);
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

        await using var fileStreamOut = File.Create(fullPath);
        await fileStream.CopyToAsync(fileStreamOut, cancellationToken);

        var fileSize = new FileInfo(fullPath).Length;
        var publicUrl = $"{_options.BaseUrl}/{storageKey.Replace(Path.DirectorySeparatorChar, '/')}";

        return new StorageUploadResult(storageKey, publicUrl, fileSize, contentType);
    }

    public Task DeleteAsync(string storageKey, CancellationToken cancellationToken = default)
    {
        var fullPath = Path.Combine(_options.BasePath, storageKey);
        if (File.Exists(fullPath)) File.Delete(fullPath);
        return Task.CompletedTask;
    }

    public Task<string> GenerateSignedUrlAsync(string storageKey, TimeSpan expiry, CancellationToken cancellationToken = default)
    {
        // Local storage returns public URL directly; signing is for cloud providers
        return Task.FromResult(GetPublicUrl(storageKey));
    }

    public Task<Stream> DownloadAsync(string storageKey, CancellationToken cancellationToken = default)
    {
        var fullPath = Path.Combine(_options.BasePath, storageKey);
        Stream stream = File.OpenRead(fullPath);
        return Task.FromResult(stream);
    }

    public Task<bool> ExistsAsync(string storageKey, CancellationToken cancellationToken = default)
    {
        var fullPath = Path.Combine(_options.BasePath, storageKey);
        return Task.FromResult(File.Exists(fullPath));
    }

    public string GetPublicUrl(string storageKey)
        => $"{_options.BaseUrl}/{storageKey.Replace(Path.DirectorySeparatorChar, '/')}";
}
