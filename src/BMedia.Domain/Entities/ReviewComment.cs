using BMedia.Domain.Common;

namespace BMedia.Domain.Entities;

public class ReviewComment : BaseEntity
{
    public Guid ContentId { get; set; }
    public Content Content { get; set; } = null!;

    public Guid ReviewerId { get; set; }
    public User Reviewer { get; set; } = null!;

    public string Comment { get; set; } = string.Empty;
    public bool IsResolved { get; set; } = false;
    public DateTime? ResolvedAt { get; set; }
    public Guid? ResolvedBy { get; set; }
    public Guid? ParentCommentId { get; set; }
    public ReviewComment? ParentComment { get; set; }

    public ICollection<ReviewComment> Replies { get; set; } = [];
}
