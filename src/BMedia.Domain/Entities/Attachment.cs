using BMedia.Domain.Common;

namespace BMedia.Domain.Entities;

public class Attachment : BaseEntity
{
    public Guid ContentId { get; set; }
    public Content Content { get; set; } = null!;

    public string FileName { get; set; } = string.Empty;
    public string StorageKey { get; set; } = string.Empty;
    public string? PublicUrl { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public long FileSizeBytes { get; set; }
    public string? Description { get; set; }
    public int SortOrder { get; set; } = 0;
}
