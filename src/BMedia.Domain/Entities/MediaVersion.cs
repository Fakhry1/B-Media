using BMedia.Domain.Common;
using BMedia.Domain.Enums;

namespace BMedia.Domain.Entities;

/// <summary>
/// Represents a specific derived version of a media asset
/// (e.g., 720p transcode, compressed image, HLS segment playlist).
/// </summary>
public class MediaVersion : BaseEntity
{
    public Guid MediaAssetId { get; set; }
    public MediaAsset MediaAsset { get; set; } = null!;

    public string StorageKey { get; set; } = string.Empty;
    public string? PublicUrl { get; set; }
    public string? CdnUrl { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public long FileSizeBytes { get; set; }

    public int? Width { get; set; }
    public int? Height { get; set; }
    public int? Bitrate { get; set; }
    public int? DurationSeconds { get; set; }
    public VideoQuality? Quality { get; set; }

    public string VersionLabel { get; set; } = string.Empty;
    public bool IsHls { get; set; } = false;
    public string? HlsManifestUrl { get; set; }
    public bool IsDefault { get; set; } = false;
    public StorageProvider StorageProvider { get; set; } = StorageProvider.Local;
}
