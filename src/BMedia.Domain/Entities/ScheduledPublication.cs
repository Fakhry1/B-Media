using BMedia.Domain.Common;

namespace BMedia.Domain.Entities;

public class ScheduledPublication : BaseEntity
{
    public Guid ContentId { get; set; }
    public Content Content { get; set; } = null!;

    public DateTime ScheduledAt { get; set; }
    public bool IsExecuted { get; set; } = false;
    public DateTime? ExecutedAt { get; set; }
    public bool IsSuccessful { get; set; } = false;
    public string? ErrorMessage { get; set; }
    public string? HangfireJobId { get; set; }
}
