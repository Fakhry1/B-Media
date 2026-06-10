using BMedia.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BMedia.Infrastructure.Persistence.Configurations;

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    // Fixed role IDs from RoleConfiguration seed
    private static readonly Guid AdminRoleId         = Guid.Parse("10000000-0000-0000-0000-000000000001");
    private static readonly Guid ContentCreatorId    = Guid.Parse("10000000-0000-0000-0000-000000000002");
    private static readonly Guid ReviewerId          = Guid.Parse("10000000-0000-0000-0000-000000000003");
    private static readonly Guid LanguageReviewerId  = Guid.Parse("10000000-0000-0000-0000-000000000004");
    private static readonly Guid DesignerId          = Guid.Parse("10000000-0000-0000-0000-000000000005");
    private static readonly Guid PublisherId         = Guid.Parse("10000000-0000-0000-0000-000000000006");
    private static readonly Guid ArchivistId         = Guid.Parse("10000000-0000-0000-0000-000000000007");

    // Fixed permission IDs from PermissionConfiguration seed (index i+1 → 000000000001..000000000017)
    private static Guid P(int n) => Guid.Parse($"20000000-0000-0000-0000-{n:D12}");

    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.HasKey(rp => new { rp.RoleId, rp.PermissionId });

        builder.HasOne(rp => rp.Role).WithMany(r => r.RolePermissions).HasForeignKey(rp => rp.RoleId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(rp => rp.Permission).WithMany(p => p.RolePermissions).HasForeignKey(rp => rp.PermissionId).OnDelete(DeleteBehavior.Cascade);

        var now = new DateTime(2026, 5, 18, 0, 0, 0, DateTimeKind.Utc);

        // Administrator ← all 17 permissions
        var adminSeeds = Enumerable.Range(1, 17)
            .Select(i => new RolePermission { RoleId = AdminRoleId, PermissionId = P(i), GrantedAt = now })
            .ToArray();

        // ContentCreator: CreateContent(1), EditContent(2), ViewContent(6), UploadMedia(7), ManageMedia(9)
        var contentCreatorSeeds = new[]
        {
            new RolePermission { RoleId = ContentCreatorId, PermissionId = P(1),  GrantedAt = now },
            new RolePermission { RoleId = ContentCreatorId, PermissionId = P(2),  GrantedAt = now },
            new RolePermission { RoleId = ContentCreatorId, PermissionId = P(6),  GrantedAt = now },
            new RolePermission { RoleId = ContentCreatorId, PermissionId = P(7),  GrantedAt = now },
            new RolePermission { RoleId = ContentCreatorId, PermissionId = P(9),  GrantedAt = now },
        };

        // Reviewer: ViewContent(6), ApproveReview(10), RejectReview(11), TransitionWorkflow(12)
        var reviewerSeeds = new[]
        {
            new RolePermission { RoleId = ReviewerId, PermissionId = P(6),  GrantedAt = now },
            new RolePermission { RoleId = ReviewerId, PermissionId = P(10), GrantedAt = now },
            new RolePermission { RoleId = ReviewerId, PermissionId = P(11), GrantedAt = now },
            new RolePermission { RoleId = ReviewerId, PermissionId = P(12), GrantedAt = now },
        };

        // LanguageReviewer: EditContent(2), ViewContent(6)
        var langReviewerSeeds = new[]
        {
            new RolePermission { RoleId = LanguageReviewerId, PermissionId = P(2), GrantedAt = now },
            new RolePermission { RoleId = LanguageReviewerId, PermissionId = P(6), GrantedAt = now },
        };

        // Designer: ViewContent(6), UploadMedia(7), DeleteMedia(8), ManageMedia(9)
        var designerSeeds = new[]
        {
            new RolePermission { RoleId = DesignerId, PermissionId = P(6), GrantedAt = now },
            new RolePermission { RoleId = DesignerId, PermissionId = P(7), GrantedAt = now },
            new RolePermission { RoleId = DesignerId, PermissionId = P(8), GrantedAt = now },
            new RolePermission { RoleId = DesignerId, PermissionId = P(9), GrantedAt = now },
        };

        // Publisher: PublishContent(4), ViewContent(6), TransitionWorkflow(12)
        var publisherSeeds = new[]
        {
            new RolePermission { RoleId = PublisherId, PermissionId = P(4),  GrantedAt = now },
            new RolePermission { RoleId = PublisherId, PermissionId = P(6),  GrantedAt = now },
            new RolePermission { RoleId = PublisherId, PermissionId = P(12), GrantedAt = now },
        };

        // Archivist: ArchiveContent(5), ViewContent(6)
        var archivistSeeds = new[]
        {
            new RolePermission { RoleId = ArchivistId, PermissionId = P(5), GrantedAt = now },
            new RolePermission { RoleId = ArchivistId, PermissionId = P(6), GrantedAt = now },
        };

        builder.HasData(adminSeeds
            .Concat(contentCreatorSeeds)
            .Concat(reviewerSeeds)
            .Concat(langReviewerSeeds)
            .Concat(designerSeeds)
            .Concat(publisherSeeds)
            .Concat(archivistSeeds));
    }
}
