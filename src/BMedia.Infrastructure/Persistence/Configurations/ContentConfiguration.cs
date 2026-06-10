using BMedia.Domain.Entities;
using BMedia.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BMedia.Infrastructure.Persistence.Configurations;

public class ContentConfiguration : IEntityTypeConfiguration<Content>
{
    public void Configure(EntityTypeBuilder<Content> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasDefaultValueSql("gen_random_uuid()");

        builder.Property(c => c.Title).IsRequired().HasMaxLength(500);
        builder.Property(c => c.Slug).IsRequired().HasMaxLength(600);
        builder.Property(c => c.Summary).HasMaxLength(2000);
        builder.Property(c => c.Language).IsRequired().HasMaxLength(10).HasDefaultValue("en");
        builder.Property(c => c.Status).HasConversion<int>().HasDefaultValue(ContentStatus.Draft);
        builder.Property(c => c.RowVersion).IsRowVersion();

        builder.HasOne(c => c.Category).WithMany(cat => cat.Contents).HasForeignKey(c => c.CategoryId).OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(c => c.Subcategory).WithMany(s => s.Contents).HasForeignKey(c => c.SubcategoryId).OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(c => c.CurrentWorkflowStep).WithMany(ws => ws.Contents).HasForeignKey(c => c.CurrentWorkflowStepId).OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(c => c.Slug).IsUnique().HasFilter("is_deleted = false");
        builder.HasIndex(c => c.Status);
        builder.HasIndex(c => c.Language);
        builder.HasIndex(c => c.IsFeatured);
        builder.HasIndex(c => c.PublishedAt);
        builder.HasIndex(c => c.CategoryId);
        builder.HasIndex(c => c.CreatedAt);
        builder.HasIndex(c => new { c.Status, c.Language, c.IsDeleted });
    }
}
