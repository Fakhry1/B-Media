namespace BMedia.Domain.Entities;

public class ContentTag
{
    public Guid ContentId { get; set; }
    public Content Content { get; set; } = null!;

    public Guid TagId { get; set; }
    public Tag Tag { get; set; } = null!;

    public DateTime TaggedAt { get; set; } = DateTime.UtcNow;
    public bool IsAiGenerated { get; set; } = false;
}
