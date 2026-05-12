using BMedia.Domain.Common;

namespace BMedia.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? IconUrl { get; set; }
    public int SortOrder { get; set; } = 0;
    public bool IsActive { get; set; } = true;

    public ICollection<Subcategory> Subcategories { get; set; } = [];
    public ICollection<Content> Contents { get; set; } = [];
}
