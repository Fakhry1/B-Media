using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BMedia.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DropRemovedMediaAssetColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop columns removed from MediaAsset entity
            // Using IF EXISTS so this is safe to run even if columns were already dropped
            migrationBuilder.Sql(@"
ALTER TABLE media_assets
    DROP COLUMN IF EXISTS cdn_url,
    DROP COLUMN IF EXISTS aspect_ratio,
    DROP COLUMN IF EXISTS codec,
    DROP COLUMN IF EXISTS bitrate,
    DROP COLUMN IF EXISTS frame_rate,
    DROP COLUMN IF EXISTS preview_url,
    DROP COLUMN IF EXISTS hls_manifest_url,
    DROP COLUMN IF EXISTS is_transcoding_complete,
    DROP COLUMN IF EXISTS is_thumbnail_generated,
    DROP COLUMN IF EXISTS is_metadata_extracted,
    DROP COLUMN IF EXISTS antivirus_scan_passed,
    DROP COLUMN IF EXISTS antivirus_scanned_at,
    DROP COLUMN IF EXISTS extracted_metadata,
    DROP COLUMN IF EXISTS ai_generated_tags,
    DROP COLUMN IF EXISTS ocr_text,
    DROP COLUMN IF EXISTS is_watermarked;
");

            // Drop columns removed from Content entity
            migrationBuilder.Sql(@"
ALTER TABLE contents
    DROP COLUMN IF EXISTS body,
    DROP COLUMN IF EXISTS seo_title,
    DROP COLUMN IF EXISTS seo_description,
    DROP COLUMN IF EXISTS seo_keywords,
    DROP COLUMN IF EXISTS canonical_url;
");

            // Drop columns removed from Localization entity
            migrationBuilder.Sql(@"
ALTER TABLE localizations
    DROP COLUMN IF EXISTS body,
    DROP COLUMN IF EXISTS seo_title,
    DROP COLUMN IF EXISTS seo_description;
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Restore media_assets columns
            migrationBuilder.Sql(@"
ALTER TABLE media_assets
    ADD COLUMN IF NOT EXISTS cdn_url varchar(2048),
    ADD COLUMN IF NOT EXISTS aspect_ratio varchar(20),
    ADD COLUMN IF NOT EXISTS codec varchar(100),
    ADD COLUMN IF NOT EXISTS bitrate integer,
    ADD COLUMN IF NOT EXISTS frame_rate numeric(8,3),
    ADD COLUMN IF NOT EXISTS preview_url varchar(2048),
    ADD COLUMN IF NOT EXISTS hls_manifest_url varchar(2048),
    ADD COLUMN IF NOT EXISTS is_transcoding_complete boolean NOT NULL DEFAULT false,
    ADD COLUMN IF NOT EXISTS is_thumbnail_generated boolean NOT NULL DEFAULT false,
    ADD COLUMN IF NOT EXISTS is_metadata_extracted boolean NOT NULL DEFAULT false,
    ADD COLUMN IF NOT EXISTS antivirus_scan_passed boolean,
    ADD COLUMN IF NOT EXISTS antivirus_scanned_at timestamptz,
    ADD COLUMN IF NOT EXISTS extracted_metadata jsonb,
    ADD COLUMN IF NOT EXISTS ai_generated_tags text[],
    ADD COLUMN IF NOT EXISTS ocr_text text,
    ADD COLUMN IF NOT EXISTS is_watermarked boolean NOT NULL DEFAULT false;
");
        }
    }
}
