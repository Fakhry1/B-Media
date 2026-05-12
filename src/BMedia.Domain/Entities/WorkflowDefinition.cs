using BMedia.Domain.Common;

namespace BMedia.Domain.Entities;

/// <summary>Dynamic, database-driven workflow definition. New workflows can be added without code changes.</summary>
public class WorkflowDefinition : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsDefault { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public int Version { get; set; } = 1;

    public ICollection<WorkflowStep> Steps { get; set; } = [];
}
