using BMedia.Domain.Entities;
using BMedia.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BMedia.Infrastructure.Persistence.Configurations;

public class MediaVersionConfiguration : IEntityTypeConfiguration<MediaVersion>
{
    public void Configure(EntityTypeBuilder<MediaVersion> builder)
    {
        builder.ToTable("MediaVersions");
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id).HasDefaultValueSql("gen_random_uuid()");

        builder.Property(v => v.StorageKey).IsRequired().HasMaxLength(1024);
        builder.Property(v => v.PublicUrl).HasMaxLength(2048);
        builder.Property(v => v.CdnUrl).HasMaxLength(2048);
        builder.Property(v => v.ContentType).IsRequired().HasMaxLength(255);
        builder.Property(v => v.VersionLabel).IsRequired().HasMaxLength(100);
        builder.Property(v => v.HlsManifestUrl).HasMaxLength(2048);
        builder.Property(v => v.Quality).HasConversion<int?>();
        builder.Property(v => v.StorageProvider).HasConversion<int>().HasDefaultValue(StorageProvider.Local);
        builder.Property(v => v.RowVersion).IsRowVersion();

        builder.HasOne(v => v.MediaAsset).WithMany(m => m.Versions).HasForeignKey(v => v.MediaAssetId).OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(v => v.MediaAssetId);
        builder.HasIndex(v => v.Quality);
        builder.HasIndex(v => v.IsDefault);
    }
}
