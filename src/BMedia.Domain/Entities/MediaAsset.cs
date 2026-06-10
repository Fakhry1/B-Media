using BMedia.Domain.Common;
using BMedia.Domain.Enums;

namespace BMedia.Domain.Entities;

/// <summary>
/// Fully independent media asset entity. Decoupled from content to support
/// multi-asset, multi-format, multi-resolution, and AI-enriched asset management.
/// </summary>
public class MediaAsset : BaseEntity
{
    public string OriginalFileName { get; set; } = string.Empty;
    public string StorageKey { get; set; } = string.Empty;
    public string? PublicUrl { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public long FileSizeBytes { get; set; }
    public MediaType MediaType { get; set; }
    public MediaAssetStatus Status { get; set; } = MediaAssetStatus.Pending;
    public StorageProvider StorageProvider { get; set; } = StorageProvider.AzureBlob;

    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? AltText { get; set; }
    public string Language { get; set; } = "en";

    public int? DurationSeconds { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }

    public string? ThumbnailUrl { get; set; }

    public int SortOrder { get; set; } = 0;
    public bool IsPrimary { get; set; } = false;

    public Guid? ContentId { get; set; }
    public Content? Content { get; set; }

    public ICollection<MediaVersion> Versions { get; set; } = [];
}
