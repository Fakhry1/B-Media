using BMedia.Domain.Common;
using BMedia.Domain.Enums;

namespace BMedia.Domain.Entities;

public class ContentWorkflowHistory : BaseEntity
{
    public Guid ContentId { get; set; }
    public Content Content { get; set; } = null!;

    public Guid? FromStepId { get; set; }
    public WorkflowStep? FromStep { get; set; }

    public Guid ToStepId { get; set; }
    public WorkflowStep ToStep { get; set; } = null!;

    public Guid TransitionedById { get; set; }
    public User TransitionedBy { get; set; } = null!;

    public ContentStatus FromStatus { get; set; }
    public ContentStatus ToStatus { get; set; }
    public string? Comment { get; set; }
    public string? ActionName { get; set; }
    public DateTime TransitionedAt { get; set; } = DateTime.UtcNow;
}
