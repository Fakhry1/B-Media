using BMedia.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BMedia.Infrastructure.Persistence.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("Permissions");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasDefaultValueSql("gen_random_uuid()");
        builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        builder.Property(p => p.NormalizedName).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Description).HasMaxLength(500);
        builder.Property(p => p.Module).IsRequired().HasMaxLength(100);
        builder.Property(p => p.RowVersion).IsRowVersion();

        builder.HasIndex(p => p.NormalizedName).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(p => p.Module);

        var permissions = new[]
        {
            ("CreateContent", "CREATECONTENT", "Content"),
            ("EditContent", "EDITCONTENT", "Content"),
            ("DeleteContent", "DELETECONTENT", "Content"),
            ("PublishContent", "PUBLISHCONTENT", "Content"),
            ("ArchiveContent", "ARCHIVECONTENT", "Content"),
            ("ViewContent", "VIEWCONTENT", "Content"),
            ("UploadMedia", "UPLOADMEDIA", "Media"),
            ("DeleteMedia", "DELETEMEDIA", "Media"),
            ("ManageMedia", "MANAGEMEDIA", "Media"),
            ("ApproveReview", "APPROVEREVIEW", "Workflow"),
            ("RejectReview", "REJECTREVIEW", "Workflow"),
            ("TransitionWorkflow", "TRANSITIONWORKFLOW", "Workflow"),
            ("ManageUsers", "MANAGEUSERS", "Admin"),
            ("ManageRoles", "MANAGEROLES", "Admin"),
            ("ViewAuditLogs", "VIEWAUDITLOGS", "Admin"),
            ("ManageCategories", "MANAGECATEGORIES", "Taxonomy"),
            ("ManageTags", "MANAGETAGS", "Taxonomy"),
        };

        var seeds = permissions.Select((p, i) => new Permission
        {
            Id = Guid.Parse($"20000000-0000-0000-0000-{(i + 1):D12}"),
            Name = p.Item1,
            NormalizedName = p.Item2,
            Module = p.Item3,
            Description = $"{p.Item1} permission"
        }).ToArray();

        builder.HasData(seeds);
    }
}
