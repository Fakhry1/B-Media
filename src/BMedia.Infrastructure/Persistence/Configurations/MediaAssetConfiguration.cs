using BMedia.Domain.Entities;
using BMedia.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BMedia.Infrastructure.Persistence.Configurations;

public class MediaAssetConfiguration : IEntityTypeConfiguration<MediaAsset>
{
    public void Configure(EntityTypeBuilder<MediaAsset> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).HasDefaultValueSql("gen_random_uuid()");

        builder.Property(m => m.OriginalFileName).IsRequired().HasMaxLength(512);
        builder.Property(m => m.StorageKey).IsRequired().HasMaxLength(1024);
        builder.Property(m => m.PublicUrl).HasMaxLength(2048);
        builder.Property(m => m.ContentType).IsRequired().HasMaxLength(255);
        builder.Property(m => m.MediaType).HasConversion<int>();
        builder.Property(m => m.Status).HasConversion<int>().HasDefaultValue(MediaAssetStatus.Pending);
        builder.Property(m => m.StorageProvider).HasConversion<int>().HasDefaultValue(StorageProvider.AzureBlob);
        builder.Property(m => m.Title).HasMaxLength(500);
        builder.Property(m => m.Description).HasMaxLength(2000);
        builder.Property(m => m.AltText).HasMaxLength(500);
        builder.Property(m => m.Language).IsRequired().HasMaxLength(10).HasDefaultValue("en");
        builder.Property(m => m.ThumbnailUrl).HasMaxLength(2048);
        builder.Property(m => m.RowVersion).IsRowVersion();

        builder.HasOne(m => m.Content).WithMany(c => c.MediaAssets).HasForeignKey(m => m.ContentId).OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(m => m.ContentId);
        builder.HasIndex(m => m.MediaType);
        builder.HasIndex(m => m.Status);
        builder.HasIndex(m => m.StorageKey).IsUnique().HasFilter("is_deleted = false");
        builder.HasIndex(m => new { m.ContentId, m.IsPrimary });
    }
}
