using BMedia.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BMedia.Infrastructure.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasDefaultValueSql("gen_random_uuid()");
        builder.Property(r => r.Name).IsRequired().HasMaxLength(100);
        builder.Property(r => r.NormalizedName).IsRequired().HasMaxLength(100);
        builder.Property(r => r.Description).HasMaxLength(500);
        builder.Property(r => r.RowVersion).IsRowVersion();

        builder.HasIndex(r => r.NormalizedName).IsUnique().HasFilter("is_deleted = false");

        builder.HasData(
            new Role { Id = Guid.Parse("10000000-0000-0000-0000-000000000001"), Name = "Administrator", NormalizedName = "ADMINISTRATOR", IsSystem = true },
            new Role { Id = Guid.Parse("10000000-0000-0000-0000-000000000002"), Name = "ContentCreator", NormalizedName = "CONTENTCREATOR", IsSystem = true },
            new Role { Id = Guid.Parse("10000000-0000-0000-0000-000000000003"), Name = "Reviewer", NormalizedName = "REVIEWER", IsSystem = true },
            new Role { Id = Guid.Parse("10000000-0000-0000-0000-000000000004"), Name = "LanguageReviewer", NormalizedName = "LANGUAGEREVIEWER", IsSystem = true },
            new Role { Id = Guid.Parse("10000000-0000-0000-0000-000000000005"), Name = "Designer", NormalizedName = "DESIGNER", IsSystem = true },
            new Role { Id = Guid.Parse("10000000-0000-0000-0000-000000000006"), Name = "Publisher", NormalizedName = "PUBLISHER", IsSystem = true },
            new Role { Id = Guid.Parse("10000000-0000-0000-0000-000000000007"), Name = "Archivist", NormalizedName = "ARCHIVIST", IsSystem = true }
        );
    }
}
