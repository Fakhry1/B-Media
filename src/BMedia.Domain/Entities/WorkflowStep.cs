using BMedia.Domain.Common;
using BMedia.Domain.Enums;

namespace BMedia.Domain.Entities;

public class WorkflowStep : BaseEntity
{
    public Guid WorkflowDefinitionId { get; set; }
    public WorkflowDefinition WorkflowDefinition { get; set; } = null!;

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ContentStatus MapsToStatus { get; set; }
    public int Order { get; set; }
    public bool IsInitial { get; set; } = false;
    public bool IsFinal { get; set; } = false;
    public bool RequiresReviewer { get; set; } = false;
    public string? RequiredPermission { get; set; }
    public int? SlaHours { get; set; }

    public ICollection<WorkflowTransition> FromTransitions { get; set; } = [];
    public ICollection<WorkflowTransition> ToTransitions { get; set; } = [];
    public ICollection<Content> Contents { get; set; } = [];
}
