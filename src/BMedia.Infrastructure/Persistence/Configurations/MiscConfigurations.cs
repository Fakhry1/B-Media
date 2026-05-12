using BMedia.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BMedia.Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasDefaultValueSql("gen_random_uuid()");
        builder.Property(c => c.Name).IsRequired().HasMaxLength(200);
        builder.Property(c => c.Slug).IsRequired().HasMaxLength(250);
        builder.Property(c => c.Description).HasMaxLength(1000);
        builder.Property(c => c.IconUrl).HasMaxLength(2048);
        builder.Property(c => c.RowVersion).IsRowVersion();
        builder.HasIndex(c => c.Slug).IsUnique().HasFilter("\"IsDeleted\" = false");
    }
}

public class SubcategoryConfiguration : IEntityTypeConfiguration<Subcategory>
{
    public void Configure(EntityTypeBuilder<Subcategory> builder)
    {
        builder.ToTable("Subcategories");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasDefaultValueSql("gen_random_uuid()");
        builder.Property(s => s.Name).IsRequired().HasMaxLength(200);
        builder.Property(s => s.Slug).IsRequired().HasMaxLength(250);
        builder.Property(s => s.Description).HasMaxLength(1000);
        builder.Property(s => s.RowVersion).IsRowVersion();
        builder.HasOne(s => s.Category).WithMany(c => c.Subcategories).HasForeignKey(s => s.CategoryId).OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(s => s.Slug).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(s => s.CategoryId);
    }
}

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("Tags");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).HasDefaultValueSql("gen_random_uuid()");
        builder.Property(t => t.Name).IsRequired().HasMaxLength(150);
        builder.Property(t => t.Slug).IsRequired().HasMaxLength(200);
        builder.Property(t => t.Description).HasMaxLength(500);
        builder.Property(t => t.RowVersion).IsRowVersion();
        builder.HasIndex(t => t.Slug).IsUnique().HasFilter("\"IsDeleted\" = false");
    }
}

public class ContentTagConfiguration : IEntityTypeConfiguration<ContentTag>
{
    public void Configure(EntityTypeBuilder<ContentTag> builder)
    {
        builder.ToTable("ContentTags");
        builder.HasKey(ct => new { ct.ContentId, ct.TagId });
        builder.HasOne(ct => ct.Content).WithMany(c => c.ContentTags).HasForeignKey(ct => ct.ContentId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(ct => ct.Tag).WithMany(t => t.ContentTags).HasForeignKey(ct => ct.TagId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications");
        builder.HasKey(n => n.Id);
        builder.Property(n => n.Id).HasDefaultValueSql("gen_random_uuid()");
        builder.Property(n => n.Title).IsRequired().HasMaxLength(300);
        builder.Property(n => n.Message).IsRequired().HasMaxLength(2000);
        builder.Property(n => n.Type).HasConversion<int>();
        builder.Property(n => n.ReferenceType).HasMaxLength(100);
        builder.Property(n => n.ActionUrl).HasMaxLength(2048);
        builder.Property(n => n.Metadata).HasColumnType("jsonb");
        builder.Property(n => n.RowVersion).IsRowVersion();
        builder.HasOne(n => n.User).WithMany(u => u.Notifications).HasForeignKey(n => n.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(n => n.UserId);
        builder.HasIndex(n => new { n.UserId, n.IsRead });
        builder.HasIndex(n => n.CreatedAt);
    }
}

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasDefaultValueSql("gen_random_uuid()");
        builder.Property(r => r.Token).IsRequired().HasMaxLength(512);
        builder.Property(r => r.RevokedReason).HasMaxLength(500);
        builder.Property(r => r.CreatedByIp).HasMaxLength(50);
        builder.Property(r => r.RevokedByIp).HasMaxLength(50);
        builder.HasOne(r => r.User).WithMany(u => u.RefreshTokens).HasForeignKey(r => r.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(r => r.Token).IsUnique();
        builder.HasIndex(r => r.UserId);
        builder.HasIndex(r => r.ExpiresAt);
    }
}

public class ReviewCommentConfiguration : IEntityTypeConfiguration<ReviewComment>
{
    public void Configure(EntityTypeBuilder<ReviewComment> builder)
    {
        builder.ToTable("ReviewComments");
        builder.HasKey(rc => rc.Id);
        builder.Property(rc => rc.Id).HasDefaultValueSql("gen_random_uuid()");
        builder.Property(rc => rc.Comment).IsRequired().HasMaxLength(5000);
        builder.Property(rc => rc.RowVersion).IsRowVersion();
        builder.HasOne(rc => rc.Content).WithMany(c => c.ReviewComments).HasForeignKey(rc => rc.ContentId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(rc => rc.Reviewer).WithMany(u => u.ReviewComments).HasForeignKey(rc => rc.ReviewerId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(rc => rc.ParentComment).WithMany(c => c.Replies).HasForeignKey(rc => rc.ParentCommentId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(rc => rc.ContentId);
        builder.HasIndex(rc => rc.ReviewerId);
    }
}

public class ContentWorkflowHistoryConfiguration : IEntityTypeConfiguration<ContentWorkflowHistory>
{
    public void Configure(EntityTypeBuilder<ContentWorkflowHistory> builder)
    {
        builder.ToTable("ContentWorkflowHistories");
        builder.HasKey(h => h.Id);
        builder.Property(h => h.Id).HasDefaultValueSql("gen_random_uuid()");
        builder.Property(h => h.FromStatus).HasConversion<int>();
        builder.Property(h => h.ToStatus).HasConversion<int>();
        builder.Property(h => h.Comment).HasMaxLength(2000);
        builder.Property(h => h.ActionName).HasMaxLength(200);
        builder.HasOne(h => h.Content).WithMany(c => c.WorkflowHistories).HasForeignKey(h => h.ContentId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(h => h.TransitionedBy).WithMany(u => u.WorkflowHistories).HasForeignKey(h => h.TransitionedById).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(h => h.FromStep).WithMany().HasForeignKey(h => h.FromStepId).OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(h => h.ToStep).WithMany().HasForeignKey(h => h.ToStepId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(h => h.ContentId);
        builder.HasIndex(h => h.TransitionedAt);
    }
}

public class LocalizationConfiguration : IEntityTypeConfiguration<Localization>
{
    public void Configure(EntityTypeBuilder<Localization> builder)
    {
        builder.ToTable("Localizations");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id).HasDefaultValueSql("gen_random_uuid()");
        builder.Property(l => l.Language).IsRequired().HasMaxLength(10);
        builder.Property(l => l.Title).IsRequired().HasMaxLength(500);
        builder.Property(l => l.Summary).HasMaxLength(2000);
        builder.Property(l => l.Body).HasColumnType("text");
        builder.Property(l => l.SeoTitle).HasMaxLength(300);
        builder.Property(l => l.SeoDescription).HasMaxLength(500);
        builder.Property(l => l.RowVersion).IsRowVersion();
        builder.HasOne(l => l.Content).WithMany(c => c.Localizations).HasForeignKey(l => l.ContentId).OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(l => new { l.ContentId, l.Language }).IsUnique().HasFilter("\"IsDeleted\" = false");
    }
}

public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
{
    public void Configure(EntityTypeBuilder<Attachment> builder)
    {
        builder.ToTable("Attachments");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasDefaultValueSql("gen_random_uuid()");
        builder.Property(a => a.FileName).IsRequired().HasMaxLength(512);
        builder.Property(a => a.StorageKey).IsRequired().HasMaxLength(1024);
        builder.Property(a => a.PublicUrl).HasMaxLength(2048);
        builder.Property(a => a.ContentType).IsRequired().HasMaxLength(255);
        builder.Property(a => a.Description).HasMaxLength(1000);
        builder.Property(a => a.RowVersion).IsRowVersion();
        builder.HasOne(a => a.Content).WithMany(c => c.Attachments).HasForeignKey(a => a.ContentId).OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(a => a.ContentId);
    }
}

public class ScheduledPublicationConfiguration : IEntityTypeConfiguration<ScheduledPublication>
{
    public void Configure(EntityTypeBuilder<ScheduledPublication> builder)
    {
        builder.ToTable("ScheduledPublications");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasDefaultValueSql("gen_random_uuid()");
        builder.Property(s => s.HangfireJobId).HasMaxLength(200);
        builder.Property(s => s.ErrorMessage).HasMaxLength(2000);
        builder.Property(s => s.RowVersion).IsRowVersion();
        builder.HasOne(s => s.Content).WithMany(c => c.ScheduledPublications).HasForeignKey(s => s.ContentId).OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(s => s.ContentId);
        builder.HasIndex(s => new { s.ScheduledAt, s.IsExecuted });
    }
}
