using BMedia.Domain.Common;

namespace BMedia.Domain.Entities;

public class WorkflowTransition : BaseEntity
{
    public Guid WorkflowDefinitionId { get; set; }
    public WorkflowDefinition WorkflowDefinition { get; set; } = null!;

    public Guid FromStepId { get; set; }
    public WorkflowStep FromStep { get; set; } = null!;

    public Guid ToStepId { get; set; }
    public WorkflowStep ToStep { get; set; } = null!;

    public string ActionName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? RequiredPermission { get; set; }
    public bool RequiresComment { get; set; } = false;
    public bool IsActive { get; set; } = true;
}
