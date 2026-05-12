using BMedia.Domain.Common;

namespace BMedia.Domain.Entities;

public class Tag : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsAiGenerated { get; set; } = false;

    public ICollection<ContentTag> ContentTags { get; set; } = [];
}
