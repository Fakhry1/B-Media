using BMedia.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BMedia.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id).HasDefaultValueSql("gen_random_uuid()");
        builder.Property(u => u.Username).IsRequired().HasMaxLength(100);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(256);
        builder.Property(u => u.PasswordHash).IsRequired();
        builder.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(u => u.LastName).IsRequired().HasMaxLength(100);
        builder.Property(u => u.PhoneNumber).HasMaxLength(30);
        builder.Property(u => u.ProfilePictureUrl).HasMaxLength(2048);
        builder.Property(u => u.PreferredLanguage).IsRequired().HasMaxLength(10).HasDefaultValue("en");
        builder.Property(u => u.TimeZone).HasMaxLength(100);

        builder.Property(u => u.RowVersion).IsRowVersion();

        builder.HasIndex(u => u.Email).IsUnique().HasFilter("is_deleted = false");
        builder.HasIndex(u => u.Username).IsUnique().HasFilter("is_deleted = false");
        builder.HasIndex(u => u.IsDeleted);
        builder.HasIndex(u => u.IsActive);
    }
}
