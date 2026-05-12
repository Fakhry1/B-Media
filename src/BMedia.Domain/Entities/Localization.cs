using BMedia.Domain.Common;

namespace BMedia.Domain.Entities;

public class Localization : BaseEntity
{
    public Guid ContentId { get; set; }
    public Content Content { get; set; } = null!;

    public string Language { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string? Body { get; set; }
    public string? SeoTitle { get; set; }
    public string? SeoDescription { get; set; }
    public bool IsApproved { get; set; } = false;
    public DateTime? ApprovedAt { get; set; }
    public Guid? ApprovedBy { get; set; }
}
