using BMedia.Domain.Common;
using BMedia.Domain.Enums;

namespace BMedia.Domain.Entities;

/// <summary>
/// Core editorial content entity. Decoupled from media assets to support
/// multi-asset, multi-language, and multi-format content lifecycle management.
/// </summary>
public class Content : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string Language { get; set; } = "en";
    public ContentStatus Status { get; set; } = ContentStatus.Draft;
    public bool IsFeatured { get; set; } = false;
    public bool AllowComments { get; set; } = true;
    public int ViewCount { get; set; } = 0;

    public DateTime? PublishedAt { get; set; }
    public Guid? PublishedBy { get; set; }
    public DateTime? ScheduledPublishAt { get; set; }
    public DateTime? ArchivedAt { get; set; }

    public Guid? CategoryId { get; set; }
    public Category? Category { get; set; }

    public Guid? SubcategoryId { get; set; }
    public Subcategory? Subcategory { get; set; }

    public Guid? CurrentWorkflowStepId { get; set; }
    public WorkflowStep? CurrentWorkflowStep { get; set; }

    public Guid? AssignedReviewerId { get; set; }

    public ICollection<ContentTag> ContentTags { get; set; } = [];
    public ICollection<MediaAsset> MediaAssets { get; set; } = [];
    public ICollection<ContentWorkflowHistory> WorkflowHistories { get; set; } = [];
    public ICollection<ReviewComment> ReviewComments { get; set; } = [];
    public ICollection<Localization> Localizations { get; set; } = [];
    public ICollection<Attachment> Attachments { get; set; } = [];
    public ICollection<ScheduledPublication> ScheduledPublications { get; set; } = [];
}
