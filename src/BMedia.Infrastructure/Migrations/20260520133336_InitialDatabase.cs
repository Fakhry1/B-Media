using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BMedia.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_contents_categories_category_id",
                table: "Contents");

            migrationBuilder.DropForeignKey(
                name: "fk_media_assets_contents_content_id",
                table: "MediaAssets");

            migrationBuilder.DropPrimaryKey(
                name: "pk_users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "pk_roles",
                table: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "pk_media_assets",
                table: "MediaAssets");

            migrationBuilder.DropPrimaryKey(
                name: "pk_contents",
                table: "Contents");

            migrationBuilder.DropPrimaryKey(
                name: "pk_categories",
                table: "Categories");

            migrationBuilder.DropPrimaryKey(
                name: "pk_audit_logs",
                table: "AuditLogs");

            migrationBuilder.RenameColumn(
                name: "username",
                table: "Users",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "Users",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Users",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "time_zone",
                table: "Users",
                newName: "TimeZone");

            migrationBuilder.RenameColumn(
                name: "row_version",
                table: "Users",
                newName: "xmin");

            migrationBuilder.RenameColumn(
                name: "profile_picture_url",
                table: "Users",
                newName: "ProfilePictureUrl");

            migrationBuilder.RenameColumn(
                name: "preferred_language",
                table: "Users",
                newName: "PreferredLanguage");

            migrationBuilder.RenameColumn(
                name: "phone_number",
                table: "Users",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "password_reset_token_expiry",
                table: "Users",
                newName: "PasswordResetTokenExpiry");

            migrationBuilder.RenameColumn(
                name: "password_reset_token",
                table: "Users",
                newName: "PasswordResetToken");

            migrationBuilder.RenameColumn(
                name: "password_hash",
                table: "Users",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "lockout_end",
                table: "Users",
                newName: "LockoutEnd");

            migrationBuilder.RenameColumn(
                name: "last_name",
                table: "Users",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "last_login_ip",
                table: "Users",
                newName: "LastLoginIp");

            migrationBuilder.RenameColumn(
                name: "last_login_at",
                table: "Users",
                newName: "LastLoginAt");

            migrationBuilder.RenameColumn(
                name: "is_email_verified",
                table: "Users",
                newName: "IsEmailVerified");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "Users",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Users",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "first_name",
                table: "Users",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "failed_login_attempts",
                table: "Users",
                newName: "FailedLoginAttempts");

            migrationBuilder.RenameColumn(
                name: "email_verification_token_expiry",
                table: "Users",
                newName: "EmailVerificationTokenExpiry");

            migrationBuilder.RenameColumn(
                name: "email_verification_token",
                table: "Users",
                newName: "EmailVerificationToken");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "Users",
                newName: "DeletedBy");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "Users",
                newName: "DeletedAt");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "Users",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Users",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_users_username",
                table: "Users",
                newName: "IX_Users_Username");

            migrationBuilder.RenameIndex(
                name: "ix_users_email",
                table: "Users",
                newName: "IX_Users_Email");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Roles",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Roles",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Roles",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "Roles",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Roles",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "row_version",
                table: "Roles",
                newName: "xmin");

            migrationBuilder.RenameColumn(
                name: "normalized_name",
                table: "Roles",
                newName: "NormalizedName");

            migrationBuilder.RenameColumn(
                name: "is_system",
                table: "Roles",
                newName: "IsSystem");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "Roles",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "Roles",
                newName: "DeletedBy");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "Roles",
                newName: "DeletedAt");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "Roles",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Roles",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_roles_normalized_name",
                table: "Roles",
                newName: "IX_Roles_NormalizedName");

            migrationBuilder.RenameColumn(
                name: "width",
                table: "MediaAssets",
                newName: "Width");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "MediaAssets",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "MediaAssets",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "language",
                table: "MediaAssets",
                newName: "Language");

            migrationBuilder.RenameColumn(
                name: "height",
                table: "MediaAssets",
                newName: "Height");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "MediaAssets",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "codec",
                table: "MediaAssets",
                newName: "Codec");

            migrationBuilder.RenameColumn(
                name: "bitrate",
                table: "MediaAssets",
                newName: "Bitrate");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "MediaAssets",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "MediaAssets",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "MediaAssets",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "thumbnail_url",
                table: "MediaAssets",
                newName: "ThumbnailUrl");

            migrationBuilder.RenameColumn(
                name: "storage_provider",
                table: "MediaAssets",
                newName: "StorageProvider");

            migrationBuilder.RenameColumn(
                name: "storage_key",
                table: "MediaAssets",
                newName: "StorageKey");

            migrationBuilder.RenameColumn(
                name: "sort_order",
                table: "MediaAssets",
                newName: "SortOrder");

            migrationBuilder.RenameColumn(
                name: "row_version",
                table: "MediaAssets",
                newName: "xmin");

            migrationBuilder.RenameColumn(
                name: "public_url",
                table: "MediaAssets",
                newName: "PublicUrl");

            migrationBuilder.RenameColumn(
                name: "preview_url",
                table: "MediaAssets",
                newName: "PreviewUrl");

            migrationBuilder.RenameColumn(
                name: "original_file_name",
                table: "MediaAssets",
                newName: "OriginalFileName");

            migrationBuilder.RenameColumn(
                name: "ocr_text",
                table: "MediaAssets",
                newName: "OcrText");

            migrationBuilder.RenameColumn(
                name: "media_type",
                table: "MediaAssets",
                newName: "MediaType");

            migrationBuilder.RenameColumn(
                name: "is_watermarked",
                table: "MediaAssets",
                newName: "IsWatermarked");

            migrationBuilder.RenameColumn(
                name: "is_transcoding_complete",
                table: "MediaAssets",
                newName: "IsTranscodingComplete");

            migrationBuilder.RenameColumn(
                name: "is_thumbnail_generated",
                table: "MediaAssets",
                newName: "IsThumbnailGenerated");

            migrationBuilder.RenameColumn(
                name: "is_primary",
                table: "MediaAssets",
                newName: "IsPrimary");

            migrationBuilder.RenameColumn(
                name: "is_metadata_extracted",
                table: "MediaAssets",
                newName: "IsMetadataExtracted");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "MediaAssets",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "hls_manifest_url",
                table: "MediaAssets",
                newName: "HlsManifestUrl");

            migrationBuilder.RenameColumn(
                name: "frame_rate",
                table: "MediaAssets",
                newName: "FrameRate");

            migrationBuilder.RenameColumn(
                name: "file_size_bytes",
                table: "MediaAssets",
                newName: "FileSizeBytes");

            migrationBuilder.RenameColumn(
                name: "extracted_metadata",
                table: "MediaAssets",
                newName: "ExtractedMetadata");

            migrationBuilder.RenameColumn(
                name: "duration_seconds",
                table: "MediaAssets",
                newName: "DurationSeconds");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "MediaAssets",
                newName: "DeletedBy");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "MediaAssets",
                newName: "DeletedAt");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "MediaAssets",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "MediaAssets",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "content_type",
                table: "MediaAssets",
                newName: "ContentType");

            migrationBuilder.RenameColumn(
                name: "content_id",
                table: "MediaAssets",
                newName: "ContentId");

            migrationBuilder.RenameColumn(
                name: "cdn_url",
                table: "MediaAssets",
                newName: "CdnUrl");

            migrationBuilder.RenameColumn(
                name: "aspect_ratio",
                table: "MediaAssets",
                newName: "AspectRatio");

            migrationBuilder.RenameColumn(
                name: "antivirus_scanned_at",
                table: "MediaAssets",
                newName: "AntivirusScannedAt");

            migrationBuilder.RenameColumn(
                name: "antivirus_scan_passed",
                table: "MediaAssets",
                newName: "AntivirusScanPassed");

            migrationBuilder.RenameColumn(
                name: "alt_text",
                table: "MediaAssets",
                newName: "AltText");

            migrationBuilder.RenameColumn(
                name: "ai_generated_tags",
                table: "MediaAssets",
                newName: "AiGeneratedTags");

            migrationBuilder.RenameIndex(
                name: "ix_media_assets_storage_key",
                table: "MediaAssets",
                newName: "IX_MediaAssets_StorageKey");

            migrationBuilder.RenameIndex(
                name: "ix_media_assets_status",
                table: "MediaAssets",
                newName: "IX_MediaAssets_Status");

            migrationBuilder.RenameIndex(
                name: "ix_media_assets_media_type",
                table: "MediaAssets",
                newName: "IX_MediaAssets_MediaType");

            migrationBuilder.RenameIndex(
                name: "ix_media_assets_content_id",
                table: "MediaAssets",
                newName: "IX_MediaAssets_ContentId");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "Contents",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "summary",
                table: "Contents",
                newName: "Summary");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Contents",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "slug",
                table: "Contents",
                newName: "Slug");

            migrationBuilder.RenameColumn(
                name: "language",
                table: "Contents",
                newName: "Language");

            migrationBuilder.RenameColumn(
                name: "body",
                table: "Contents",
                newName: "Body");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Contents",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "view_count",
                table: "Contents",
                newName: "ViewCount");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "Contents",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Contents",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "subcategory_id",
                table: "Contents",
                newName: "SubcategoryId");

            migrationBuilder.RenameColumn(
                name: "seo_title",
                table: "Contents",
                newName: "SeoTitle");

            migrationBuilder.RenameColumn(
                name: "seo_keywords",
                table: "Contents",
                newName: "SeoKeywords");

            migrationBuilder.RenameColumn(
                name: "seo_description",
                table: "Contents",
                newName: "SeoDescription");

            migrationBuilder.RenameColumn(
                name: "scheduled_publish_at",
                table: "Contents",
                newName: "ScheduledPublishAt");

            migrationBuilder.RenameColumn(
                name: "row_version",
                table: "Contents",
                newName: "xmin");

            migrationBuilder.RenameColumn(
                name: "published_by",
                table: "Contents",
                newName: "PublishedBy");

            migrationBuilder.RenameColumn(
                name: "published_at",
                table: "Contents",
                newName: "PublishedAt");

            migrationBuilder.RenameColumn(
                name: "is_featured",
                table: "Contents",
                newName: "IsFeatured");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "Contents",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "Contents",
                newName: "DeletedBy");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "Contents",
                newName: "DeletedAt");

            migrationBuilder.RenameColumn(
                name: "current_workflow_step_id",
                table: "Contents",
                newName: "CurrentWorkflowStepId");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "Contents",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Contents",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "category_id",
                table: "Contents",
                newName: "CategoryId");

            migrationBuilder.RenameColumn(
                name: "canonical_url",
                table: "Contents",
                newName: "CanonicalUrl");

            migrationBuilder.RenameColumn(
                name: "assigned_reviewer_id",
                table: "Contents",
                newName: "AssignedReviewerId");

            migrationBuilder.RenameColumn(
                name: "archived_at",
                table: "Contents",
                newName: "ArchivedAt");

            migrationBuilder.RenameColumn(
                name: "allow_comments",
                table: "Contents",
                newName: "AllowComments");

            migrationBuilder.RenameIndex(
                name: "ix_contents_status",
                table: "Contents",
                newName: "IX_Contents_Status");

            migrationBuilder.RenameIndex(
                name: "ix_contents_slug",
                table: "Contents",
                newName: "IX_Contents_Slug");

            migrationBuilder.RenameIndex(
                name: "ix_contents_published_at",
                table: "Contents",
                newName: "IX_Contents_PublishedAt");

            migrationBuilder.RenameIndex(
                name: "ix_contents_category_id",
                table: "Contents",
                newName: "IX_Contents_CategoryId");

            migrationBuilder.RenameColumn(
                name: "slug",
                table: "Categories",
                newName: "Slug");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Categories",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Categories",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Categories",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "Categories",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Categories",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "sort_order",
                table: "Categories",
                newName: "SortOrder");

            migrationBuilder.RenameColumn(
                name: "row_version",
                table: "Categories",
                newName: "xmin");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "Categories",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Categories",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "icon_url",
                table: "Categories",
                newName: "IconUrl");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "Categories",
                newName: "DeletedBy");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "Categories",
                newName: "DeletedAt");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "Categories",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Categories",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_categories_slug",
                table: "Categories",
                newName: "IX_Categories_Slug");

            migrationBuilder.RenameColumn(
                name: "action",
                table: "AuditLogs",
                newName: "Action");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "AuditLogs",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "AuditLogs",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "user_email",
                table: "AuditLogs",
                newName: "UserEmail");

            migrationBuilder.RenameColumn(
                name: "user_agent",
                table: "AuditLogs",
                newName: "UserAgent");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "AuditLogs",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "AuditLogs",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "row_version",
                table: "AuditLogs",
                newName: "RowVersion");

            migrationBuilder.RenameColumn(
                name: "old_values",
                table: "AuditLogs",
                newName: "OldValues");

            migrationBuilder.RenameColumn(
                name: "new_values",
                table: "AuditLogs",
                newName: "NewValues");

            migrationBuilder.RenameColumn(
                name: "is_successful",
                table: "AuditLogs",
                newName: "IsSuccessful");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "AuditLogs",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "ip_address",
                table: "AuditLogs",
                newName: "IpAddress");

            migrationBuilder.RenameColumn(
                name: "error_message",
                table: "AuditLogs",
                newName: "ErrorMessage");

            migrationBuilder.RenameColumn(
                name: "entity_type",
                table: "AuditLogs",
                newName: "EntityType");

            migrationBuilder.RenameColumn(
                name: "entity_id",
                table: "AuditLogs",
                newName: "EntityId");

            migrationBuilder.RenameColumn(
                name: "deleted_by",
                table: "AuditLogs",
                newName: "DeletedBy");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "AuditLogs",
                newName: "DeletedAt");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "AuditLogs",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "AuditLogs",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "additional_data",
                table: "AuditLogs",
                newName: "AdditionalData");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Roles",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "RowVersion",
                table: "AuditLogs",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(uint),
                oldType: "xid",
                oldRowVersion: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MediaAssets",
                table: "MediaAssets",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contents",
                table: "Contents",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuditLogs",
                table: "AuditLogs",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ContentId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    StorageKey = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    PublicUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    ContentType = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FileSizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attachments_Contents_ContentId",
                        column: x => x.ContentId,
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Localizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ContentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Language = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Summary = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Body = table.Column<string>(type: "text", nullable: true),
                    SeoTitle = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    SeoDescription = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsApproved = table.Column<bool>(type: "boolean", nullable: false),
                    ApprovedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ApprovedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Localizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Localizations_Contents_ContentId",
                        column: x => x.ContentId,
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MediaVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    MediaAssetId = table.Column<Guid>(type: "uuid", nullable: false),
                    StorageKey = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    PublicUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    CdnUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    ContentType = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FileSizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    Width = table.Column<int>(type: "integer", nullable: true),
                    Height = table.Column<int>(type: "integer", nullable: true),
                    Bitrate = table.Column<int>(type: "integer", nullable: true),
                    DurationSeconds = table.Column<int>(type: "integer", nullable: true),
                    Quality = table.Column<int>(type: "integer", nullable: true),
                    VersionLabel = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsHls = table.Column<bool>(type: "boolean", nullable: false),
                    HlsManifestUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false),
                    StorageProvider = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaVersions_MediaAssets_MediaAssetId",
                        column: x => x.MediaAssetId,
                        principalTable: "MediaAssets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Message = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReferenceId = table.Column<Guid>(type: "uuid", nullable: true),
                    ReferenceType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ActionUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Metadata = table.Column<string>(type: "jsonb", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NormalizedName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Module = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsRevoked = table.Column<bool>(type: "boolean", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RevokedReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ReplacedByToken = table.Column<string>(type: "text", nullable: true),
                    CreatedByIp = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    RevokedByIp = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    RowVersion = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReviewComments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ContentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReviewerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Comment = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false),
                    IsResolved = table.Column<bool>(type: "boolean", nullable: false),
                    ResolvedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ResolvedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    ParentCommentId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewComments_Contents_ContentId",
                        column: x => x.ContentId,
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReviewComments_ReviewComments_ParentCommentId",
                        column: x => x.ParentCommentId,
                        principalTable: "ReviewComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReviewComments_Users_ReviewerId",
                        column: x => x.ReviewerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ScheduledPublications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ContentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ScheduledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsExecuted = table.Column<bool>(type: "boolean", nullable: false),
                    ExecutedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsSuccessful = table.Column<bool>(type: "boolean", nullable: false),
                    ErrorMessage = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    HangfireJobId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledPublications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduledPublications_Contents_ContentId",
                        column: x => x.ContentId,
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subcategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Slug = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subcategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subcategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Slug = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsAiGenerated = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AssignedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowDefinitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowDefinitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uuid", nullable: false),
                    GrantedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GrantedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContentTags",
                columns: table => new
                {
                    ContentId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagId = table.Column<Guid>(type: "uuid", nullable: false),
                    TaggedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsAiGenerated = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentTags", x => new { x.ContentId, x.TagId });
                    table.ForeignKey(
                        name: "FK_ContentTags_Contents_ContentId",
                        column: x => x.ContentId,
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowSteps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    WorkflowDefinitionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    MapsToStatus = table.Column<int>(type: "integer", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    IsInitial = table.Column<bool>(type: "boolean", nullable: false),
                    IsFinal = table.Column<bool>(type: "boolean", nullable: false),
                    RequiresReviewer = table.Column<bool>(type: "boolean", nullable: false),
                    RequiredPermission = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    SlaHours = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowSteps_WorkflowDefinitions_WorkflowDefinitionId",
                        column: x => x.WorkflowDefinitionId,
                        principalTable: "WorkflowDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContentWorkflowHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ContentId = table.Column<Guid>(type: "uuid", nullable: false),
                    FromStepId = table.Column<Guid>(type: "uuid", nullable: true),
                    ToStepId = table.Column<Guid>(type: "uuid", nullable: false),
                    TransitionedById = table.Column<Guid>(type: "uuid", nullable: false),
                    FromStatus = table.Column<int>(type: "integer", nullable: false),
                    ToStatus = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ActionName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    TransitionedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    RowVersion = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentWorkflowHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentWorkflowHistories_Contents_ContentId",
                        column: x => x.ContentId,
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentWorkflowHistories_Users_TransitionedById",
                        column: x => x.TransitionedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContentWorkflowHistories_WorkflowSteps_FromStepId",
                        column: x => x.FromStepId,
                        principalTable: "WorkflowSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ContentWorkflowHistories_WorkflowSteps_ToStepId",
                        column: x => x.ToStepId,
                        principalTable: "WorkflowSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowTransitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    WorkflowDefinitionId = table.Column<Guid>(type: "uuid", nullable: false),
                    FromStepId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToStepId = table.Column<Guid>(type: "uuid", nullable: false),
                    ActionName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    RequiredPermission = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    RequiresComment = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowTransitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowTransitions_WorkflowDefinitions_WorkflowDefinitionId",
                        column: x => x.WorkflowDefinitionId,
                        principalTable: "WorkflowDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkflowTransitions_WorkflowSteps_FromStepId",
                        column: x => x.FromStepId,
                        principalTable: "WorkflowSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkflowTransitions_WorkflowSteps_ToStepId",
                        column: x => x.ToStepId,
                        principalTable: "WorkflowSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Description", "IsDeleted", "Module", "Name", "NormalizedName", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("20000000-0000-0000-0000-000000000001"), new DateTime(2026, 5, 20, 13, 33, 35, 533, DateTimeKind.Utc).AddTicks(3526), null, null, null, "CreateContent permission", false, "Content", "CreateContent", "CREATECONTENT", null, null },
                    { new Guid("20000000-0000-0000-0000-000000000002"), new DateTime(2026, 5, 20, 13, 33, 35, 534, DateTimeKind.Utc).AddTicks(2740), null, null, null, "EditContent permission", false, "Content", "EditContent", "EDITCONTENT", null, null },
                    { new Guid("20000000-0000-0000-0000-000000000003"), new DateTime(2026, 5, 20, 13, 33, 35, 534, DateTimeKind.Utc).AddTicks(2830), null, null, null, "DeleteContent permission", false, "Content", "DeleteContent", "DELETECONTENT", null, null },
                    { new Guid("20000000-0000-0000-0000-000000000004"), new DateTime(2026, 5, 20, 13, 33, 35, 534, DateTimeKind.Utc).AddTicks(2836), null, null, null, "PublishContent permission", false, "Content", "PublishContent", "PUBLISHCONTENT", null, null },
                    { new Guid("20000000-0000-0000-0000-000000000005"), new DateTime(2026, 5, 20, 13, 33, 35, 534, DateTimeKind.Utc).AddTicks(2840), null, null, null, "ArchiveContent permission", false, "Content", "ArchiveContent", "ARCHIVECONTENT", null, null },
                    { new Guid("20000000-0000-0000-0000-000000000006"), new DateTime(2026, 5, 20, 13, 33, 35, 534, DateTimeKind.Utc).AddTicks(2843), null, null, null, "ViewContent permission", false, "Content", "ViewContent", "VIEWCONTENT", null, null },
                    { new Guid("20000000-0000-0000-0000-000000000007"), new DateTime(2026, 5, 20, 13, 33, 35, 534, DateTimeKind.Utc).AddTicks(2847), null, null, null, "UploadMedia permission", false, "Media", "UploadMedia", "UPLOADMEDIA", null, null },
                    { new Guid("20000000-0000-0000-0000-000000000008"), new DateTime(2026, 5, 20, 13, 33, 35, 534, DateTimeKind.Utc).AddTicks(2858), null, null, null, "DeleteMedia permission", false, "Media", "DeleteMedia", "DELETEMEDIA", null, null },
                    { new Guid("20000000-0000-0000-0000-000000000009"), new DateTime(2026, 5, 20, 13, 33, 35, 534, DateTimeKind.Utc).AddTicks(2862), null, null, null, "ManageMedia permission", false, "Media", "ManageMedia", "MANAGEMEDIA", null, null },
                    { new Guid("20000000-0000-0000-0000-000000000010"), new DateTime(2026, 5, 20, 13, 33, 35, 534, DateTimeKind.Utc).AddTicks(3209), null, null, null, "ApproveReview permission", false, "Workflow", "ApproveReview", "APPROVEREVIEW", null, null },
                    { new Guid("20000000-0000-0000-0000-000000000011"), new DateTime(2026, 5, 20, 13, 33, 35, 534, DateTimeKind.Utc).AddTicks(3220), null, null, null, "RejectReview permission", false, "Workflow", "RejectReview", "REJECTREVIEW", null, null },
                    { new Guid("20000000-0000-0000-0000-000000000012"), new DateTime(2026, 5, 20, 13, 33, 35, 534, DateTimeKind.Utc).AddTicks(3223), null, null, null, "TransitionWorkflow permission", false, "Workflow", "TransitionWorkflow", "TRANSITIONWORKFLOW", null, null },
                    { new Guid("20000000-0000-0000-0000-000000000013"), new DateTime(2026, 5, 20, 13, 33, 35, 534, DateTimeKind.Utc).AddTicks(3226), null, null, null, "ManageUsers permission", false, "Admin", "ManageUsers", "MANAGEUSERS", null, null },
                    { new Guid("20000000-0000-0000-0000-000000000014"), new DateTime(2026, 5, 20, 13, 33, 35, 534, DateTimeKind.Utc).AddTicks(3229), null, null, null, "ManageRoles permission", false, "Admin", "ManageRoles", "MANAGEROLES", null, null },
                    { new Guid("20000000-0000-0000-0000-000000000015"), new DateTime(2026, 5, 20, 13, 33, 35, 534, DateTimeKind.Utc).AddTicks(3232), null, null, null, "ViewAuditLogs permission", false, "Admin", "ViewAuditLogs", "VIEWAUDITLOGS", null, null },
                    { new Guid("20000000-0000-0000-0000-000000000016"), new DateTime(2026, 5, 20, 13, 33, 35, 534, DateTimeKind.Utc).AddTicks(3235), null, null, null, "ManageCategories permission", false, "Taxonomy", "ManageCategories", "MANAGECATEGORIES", null, null },
                    { new Guid("20000000-0000-0000-0000-000000000017"), new DateTime(2026, 5, 20, 13, 33, 35, 534, DateTimeKind.Utc).AddTicks(3239), null, null, null, "ManageTags permission", false, "Taxonomy", "ManageTags", "MANAGETAGS", null, null }
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 5, 20, 13, 33, 35, 539, DateTimeKind.Utc).AddTicks(308));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 5, 20, 13, 33, 35, 539, DateTimeKind.Utc).AddTicks(1066));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 5, 20, 13, 33, 35, 539, DateTimeKind.Utc).AddTicks(1072));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 5, 20, 13, 33, 35, 539, DateTimeKind.Utc).AddTicks(1074));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000005"),
                column: "CreatedAt",
                value: new DateTime(2026, 5, 20, 13, 33, 35, 539, DateTimeKind.Utc).AddTicks(1076));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000006"),
                column: "CreatedAt",
                value: new DateTime(2026, 5, 20, 13, 33, 35, 539, DateTimeKind.Utc).AddTicks(1078));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000007"),
                column: "CreatedAt",
                value: new DateTime(2026, 5, 20, 13, 33, 35, 539, DateTimeKind.Utc).AddTicks(1080));

            migrationBuilder.CreateIndex(
                name: "IX_Users_IsActive",
                table: "Users",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IsDeleted",
                table: "Users",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_MediaAssets_ContentId_IsPrimary",
                table: "MediaAssets",
                columns: new[] { "ContentId", "IsPrimary" });

            migrationBuilder.CreateIndex(
                name: "IX_Contents_CreatedAt",
                table: "Contents",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_CurrentWorkflowStepId",
                table: "Contents",
                column: "CurrentWorkflowStepId");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_IsFeatured",
                table: "Contents",
                column: "IsFeatured");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_Language",
                table: "Contents",
                column: "Language");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_Status_Language_IsDeleted",
                table: "Contents",
                columns: new[] { "Status", "Language", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Contents_SubcategoryId",
                table: "Contents",
                column: "SubcategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_Action",
                table: "AuditLogs",
                column: "Action");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_CreatedAt",
                table: "AuditLogs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_EntityType",
                table: "AuditLogs",
                column: "EntityType");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_EntityType_EntityId",
                table: "AuditLogs",
                columns: new[] { "EntityType", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_UserId",
                table: "AuditLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_ContentId",
                table: "Attachments",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentTags_TagId",
                table: "ContentTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentWorkflowHistories_ContentId",
                table: "ContentWorkflowHistories",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentWorkflowHistories_FromStepId",
                table: "ContentWorkflowHistories",
                column: "FromStepId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentWorkflowHistories_ToStepId",
                table: "ContentWorkflowHistories",
                column: "ToStepId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentWorkflowHistories_TransitionedAt",
                table: "ContentWorkflowHistories",
                column: "TransitionedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ContentWorkflowHistories_TransitionedById",
                table: "ContentWorkflowHistories",
                column: "TransitionedById");

            migrationBuilder.CreateIndex(
                name: "IX_Localizations_ContentId_Language",
                table: "Localizations",
                columns: new[] { "ContentId", "Language" },
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_MediaVersions_IsDefault",
                table: "MediaVersions",
                column: "IsDefault");

            migrationBuilder.CreateIndex(
                name: "IX_MediaVersions_MediaAssetId",
                table: "MediaVersions",
                column: "MediaAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaVersions_Quality",
                table: "MediaVersions",
                column: "Quality");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CreatedAt",
                table: "Notifications",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId_IsRead",
                table: "Notifications",
                columns: new[] { "UserId", "IsRead" });

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_Module",
                table: "Permissions",
                column: "Module");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_NormalizedName",
                table: "Permissions",
                column: "NormalizedName",
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_ExpiresAt",
                table: "RefreshTokens",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_Token",
                table: "RefreshTokens",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewComments_ContentId",
                table: "ReviewComments",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewComments_ParentCommentId",
                table: "ReviewComments",
                column: "ParentCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewComments_ReviewerId",
                table: "ReviewComments",
                column: "ReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledPublications_ContentId",
                table: "ScheduledPublications",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledPublications_ScheduledAt_IsExecuted",
                table: "ScheduledPublications",
                columns: new[] { "ScheduledAt", "IsExecuted" });

            migrationBuilder.CreateIndex(
                name: "IX_Subcategories_CategoryId",
                table: "Subcategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Subcategories_Slug",
                table: "Subcategories",
                column: "Slug",
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Slug",
                table: "Tags",
                column: "Slug",
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowDefinitions_IsDefault",
                table: "WorkflowDefinitions",
                column: "IsDefault");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowSteps_WorkflowDefinitionId",
                table: "WorkflowSteps",
                column: "WorkflowDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowSteps_WorkflowDefinitionId_Order",
                table: "WorkflowSteps",
                columns: new[] { "WorkflowDefinitionId", "Order" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowTransitions_FromStepId_ToStepId",
                table: "WorkflowTransitions",
                columns: new[] { "FromStepId", "ToStepId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowTransitions_ToStepId",
                table: "WorkflowTransitions",
                column: "ToStepId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowTransitions_WorkflowDefinitionId",
                table: "WorkflowTransitions",
                column: "WorkflowDefinitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_Categories_CategoryId",
                table: "Contents",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_Subcategories_SubcategoryId",
                table: "Contents",
                column: "SubcategoryId",
                principalTable: "Subcategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_WorkflowSteps_CurrentWorkflowStepId",
                table: "Contents",
                column: "CurrentWorkflowStepId",
                principalTable: "WorkflowSteps",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_MediaAssets_Contents_ContentId",
                table: "MediaAssets",
                column: "ContentId",
                principalTable: "Contents",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contents_Categories_CategoryId",
                table: "Contents");

            migrationBuilder.DropForeignKey(
                name: "FK_Contents_Subcategories_SubcategoryId",
                table: "Contents");

            migrationBuilder.DropForeignKey(
                name: "FK_Contents_WorkflowSteps_CurrentWorkflowStepId",
                table: "Contents");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaAssets_Contents_ContentId",
                table: "MediaAssets");

            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "ContentTags");

            migrationBuilder.DropTable(
                name: "ContentWorkflowHistories");

            migrationBuilder.DropTable(
                name: "Localizations");

            migrationBuilder.DropTable(
                name: "MediaVersions");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "ReviewComments");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "ScheduledPublications");

            migrationBuilder.DropTable(
                name: "Subcategories");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "WorkflowTransitions");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "WorkflowSteps");

            migrationBuilder.DropTable(
                name: "WorkflowDefinitions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_IsActive",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_IsDeleted",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MediaAssets",
                table: "MediaAssets");

            migrationBuilder.DropIndex(
                name: "IX_MediaAssets_ContentId_IsPrimary",
                table: "MediaAssets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contents",
                table: "Contents");

            migrationBuilder.DropIndex(
                name: "IX_Contents_CreatedAt",
                table: "Contents");

            migrationBuilder.DropIndex(
                name: "IX_Contents_CurrentWorkflowStepId",
                table: "Contents");

            migrationBuilder.DropIndex(
                name: "IX_Contents_IsFeatured",
                table: "Contents");

            migrationBuilder.DropIndex(
                name: "IX_Contents_Language",
                table: "Contents");

            migrationBuilder.DropIndex(
                name: "IX_Contents_Status_Language_IsDeleted",
                table: "Contents");

            migrationBuilder.DropIndex(
                name: "IX_Contents_SubcategoryId",
                table: "Contents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuditLogs",
                table: "AuditLogs");

            migrationBuilder.DropIndex(
                name: "IX_AuditLogs_Action",
                table: "AuditLogs");

            migrationBuilder.DropIndex(
                name: "IX_AuditLogs_CreatedAt",
                table: "AuditLogs");

            migrationBuilder.DropIndex(
                name: "IX_AuditLogs_EntityType",
                table: "AuditLogs");

            migrationBuilder.DropIndex(
                name: "IX_AuditLogs_EntityType_EntityId",
                table: "AuditLogs");

            migrationBuilder.DropIndex(
                name: "IX_AuditLogs_UserId",
                table: "AuditLogs");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Users",
                newName: "username");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Users",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Users",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "xmin",
                table: "Users",
                newName: "row_version");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Users",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Users",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TimeZone",
                table: "Users",
                newName: "time_zone");

            migrationBuilder.RenameColumn(
                name: "ProfilePictureUrl",
                table: "Users",
                newName: "profile_picture_url");

            migrationBuilder.RenameColumn(
                name: "PreferredLanguage",
                table: "Users",
                newName: "preferred_language");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Users",
                newName: "phone_number");

            migrationBuilder.RenameColumn(
                name: "PasswordResetTokenExpiry",
                table: "Users",
                newName: "password_reset_token_expiry");

            migrationBuilder.RenameColumn(
                name: "PasswordResetToken",
                table: "Users",
                newName: "password_reset_token");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Users",
                newName: "password_hash");

            migrationBuilder.RenameColumn(
                name: "LockoutEnd",
                table: "Users",
                newName: "lockout_end");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Users",
                newName: "last_name");

            migrationBuilder.RenameColumn(
                name: "LastLoginIp",
                table: "Users",
                newName: "last_login_ip");

            migrationBuilder.RenameColumn(
                name: "LastLoginAt",
                table: "Users",
                newName: "last_login_at");

            migrationBuilder.RenameColumn(
                name: "IsEmailVerified",
                table: "Users",
                newName: "is_email_verified");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Users",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Users",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Users",
                newName: "first_name");

            migrationBuilder.RenameColumn(
                name: "FailedLoginAttempts",
                table: "Users",
                newName: "failed_login_attempts");

            migrationBuilder.RenameColumn(
                name: "EmailVerificationTokenExpiry",
                table: "Users",
                newName: "email_verification_token_expiry");

            migrationBuilder.RenameColumn(
                name: "EmailVerificationToken",
                table: "Users",
                newName: "email_verification_token");

            migrationBuilder.RenameColumn(
                name: "DeletedBy",
                table: "Users",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "Users",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Users",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Users",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Username",
                table: "Users",
                newName: "ix_users_username");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Email",
                table: "Users",
                newName: "ix_users_email");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Roles",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Roles",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Roles",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "xmin",
                table: "Roles",
                newName: "row_version");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Roles",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Roles",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "NormalizedName",
                table: "Roles",
                newName: "normalized_name");

            migrationBuilder.RenameColumn(
                name: "IsSystem",
                table: "Roles",
                newName: "is_system");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Roles",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "DeletedBy",
                table: "Roles",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "Roles",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Roles",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Roles",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_Roles_NormalizedName",
                table: "Roles",
                newName: "ix_roles_normalized_name");

            migrationBuilder.RenameColumn(
                name: "Width",
                table: "MediaAssets",
                newName: "width");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "MediaAssets",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "MediaAssets",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Language",
                table: "MediaAssets",
                newName: "language");

            migrationBuilder.RenameColumn(
                name: "Height",
                table: "MediaAssets",
                newName: "height");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "MediaAssets",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Codec",
                table: "MediaAssets",
                newName: "codec");

            migrationBuilder.RenameColumn(
                name: "Bitrate",
                table: "MediaAssets",
                newName: "bitrate");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "MediaAssets",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "xmin",
                table: "MediaAssets",
                newName: "row_version");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "MediaAssets",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "MediaAssets",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "ThumbnailUrl",
                table: "MediaAssets",
                newName: "thumbnail_url");

            migrationBuilder.RenameColumn(
                name: "StorageProvider",
                table: "MediaAssets",
                newName: "storage_provider");

            migrationBuilder.RenameColumn(
                name: "StorageKey",
                table: "MediaAssets",
                newName: "storage_key");

            migrationBuilder.RenameColumn(
                name: "SortOrder",
                table: "MediaAssets",
                newName: "sort_order");

            migrationBuilder.RenameColumn(
                name: "PublicUrl",
                table: "MediaAssets",
                newName: "public_url");

            migrationBuilder.RenameColumn(
                name: "PreviewUrl",
                table: "MediaAssets",
                newName: "preview_url");

            migrationBuilder.RenameColumn(
                name: "OriginalFileName",
                table: "MediaAssets",
                newName: "original_file_name");

            migrationBuilder.RenameColumn(
                name: "OcrText",
                table: "MediaAssets",
                newName: "ocr_text");

            migrationBuilder.RenameColumn(
                name: "MediaType",
                table: "MediaAssets",
                newName: "media_type");

            migrationBuilder.RenameColumn(
                name: "IsWatermarked",
                table: "MediaAssets",
                newName: "is_watermarked");

            migrationBuilder.RenameColumn(
                name: "IsTranscodingComplete",
                table: "MediaAssets",
                newName: "is_transcoding_complete");

            migrationBuilder.RenameColumn(
                name: "IsThumbnailGenerated",
                table: "MediaAssets",
                newName: "is_thumbnail_generated");

            migrationBuilder.RenameColumn(
                name: "IsPrimary",
                table: "MediaAssets",
                newName: "is_primary");

            migrationBuilder.RenameColumn(
                name: "IsMetadataExtracted",
                table: "MediaAssets",
                newName: "is_metadata_extracted");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "MediaAssets",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "HlsManifestUrl",
                table: "MediaAssets",
                newName: "hls_manifest_url");

            migrationBuilder.RenameColumn(
                name: "FrameRate",
                table: "MediaAssets",
                newName: "frame_rate");

            migrationBuilder.RenameColumn(
                name: "FileSizeBytes",
                table: "MediaAssets",
                newName: "file_size_bytes");

            migrationBuilder.RenameColumn(
                name: "ExtractedMetadata",
                table: "MediaAssets",
                newName: "extracted_metadata");

            migrationBuilder.RenameColumn(
                name: "DurationSeconds",
                table: "MediaAssets",
                newName: "duration_seconds");

            migrationBuilder.RenameColumn(
                name: "DeletedBy",
                table: "MediaAssets",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "MediaAssets",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "MediaAssets",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "MediaAssets",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "ContentType",
                table: "MediaAssets",
                newName: "content_type");

            migrationBuilder.RenameColumn(
                name: "ContentId",
                table: "MediaAssets",
                newName: "content_id");

            migrationBuilder.RenameColumn(
                name: "CdnUrl",
                table: "MediaAssets",
                newName: "cdn_url");

            migrationBuilder.RenameColumn(
                name: "AspectRatio",
                table: "MediaAssets",
                newName: "aspect_ratio");

            migrationBuilder.RenameColumn(
                name: "AntivirusScannedAt",
                table: "MediaAssets",
                newName: "antivirus_scanned_at");

            migrationBuilder.RenameColumn(
                name: "AntivirusScanPassed",
                table: "MediaAssets",
                newName: "antivirus_scan_passed");

            migrationBuilder.RenameColumn(
                name: "AltText",
                table: "MediaAssets",
                newName: "alt_text");

            migrationBuilder.RenameColumn(
                name: "AiGeneratedTags",
                table: "MediaAssets",
                newName: "ai_generated_tags");

            migrationBuilder.RenameIndex(
                name: "IX_MediaAssets_StorageKey",
                table: "MediaAssets",
                newName: "ix_media_assets_storage_key");

            migrationBuilder.RenameIndex(
                name: "IX_MediaAssets_Status",
                table: "MediaAssets",
                newName: "ix_media_assets_status");

            migrationBuilder.RenameIndex(
                name: "IX_MediaAssets_MediaType",
                table: "MediaAssets",
                newName: "ix_media_assets_media_type");

            migrationBuilder.RenameIndex(
                name: "IX_MediaAssets_ContentId",
                table: "MediaAssets",
                newName: "ix_media_assets_content_id");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Contents",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Summary",
                table: "Contents",
                newName: "summary");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Contents",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Slug",
                table: "Contents",
                newName: "slug");

            migrationBuilder.RenameColumn(
                name: "Language",
                table: "Contents",
                newName: "language");

            migrationBuilder.RenameColumn(
                name: "Body",
                table: "Contents",
                newName: "body");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Contents",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "xmin",
                table: "Contents",
                newName: "row_version");

            migrationBuilder.RenameColumn(
                name: "ViewCount",
                table: "Contents",
                newName: "view_count");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Contents",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Contents",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "SubcategoryId",
                table: "Contents",
                newName: "subcategory_id");

            migrationBuilder.RenameColumn(
                name: "SeoTitle",
                table: "Contents",
                newName: "seo_title");

            migrationBuilder.RenameColumn(
                name: "SeoKeywords",
                table: "Contents",
                newName: "seo_keywords");

            migrationBuilder.RenameColumn(
                name: "SeoDescription",
                table: "Contents",
                newName: "seo_description");

            migrationBuilder.RenameColumn(
                name: "ScheduledPublishAt",
                table: "Contents",
                newName: "scheduled_publish_at");

            migrationBuilder.RenameColumn(
                name: "PublishedBy",
                table: "Contents",
                newName: "published_by");

            migrationBuilder.RenameColumn(
                name: "PublishedAt",
                table: "Contents",
                newName: "published_at");

            migrationBuilder.RenameColumn(
                name: "IsFeatured",
                table: "Contents",
                newName: "is_featured");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Contents",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "DeletedBy",
                table: "Contents",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "Contents",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "CurrentWorkflowStepId",
                table: "Contents",
                newName: "current_workflow_step_id");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Contents",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Contents",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Contents",
                newName: "category_id");

            migrationBuilder.RenameColumn(
                name: "CanonicalUrl",
                table: "Contents",
                newName: "canonical_url");

            migrationBuilder.RenameColumn(
                name: "AssignedReviewerId",
                table: "Contents",
                newName: "assigned_reviewer_id");

            migrationBuilder.RenameColumn(
                name: "ArchivedAt",
                table: "Contents",
                newName: "archived_at");

            migrationBuilder.RenameColumn(
                name: "AllowComments",
                table: "Contents",
                newName: "allow_comments");

            migrationBuilder.RenameIndex(
                name: "IX_Contents_Status",
                table: "Contents",
                newName: "ix_contents_status");

            migrationBuilder.RenameIndex(
                name: "IX_Contents_Slug",
                table: "Contents",
                newName: "ix_contents_slug");

            migrationBuilder.RenameIndex(
                name: "IX_Contents_PublishedAt",
                table: "Contents",
                newName: "ix_contents_published_at");

            migrationBuilder.RenameIndex(
                name: "IX_Contents_CategoryId",
                table: "Contents",
                newName: "ix_contents_category_id");

            migrationBuilder.RenameColumn(
                name: "Slug",
                table: "Categories",
                newName: "slug");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Categories",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Categories",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Categories",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "xmin",
                table: "Categories",
                newName: "row_version");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Categories",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Categories",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "SortOrder",
                table: "Categories",
                newName: "sort_order");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Categories",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Categories",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "IconUrl",
                table: "Categories",
                newName: "icon_url");

            migrationBuilder.RenameColumn(
                name: "DeletedBy",
                table: "Categories",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "Categories",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Categories",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Categories",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_Slug",
                table: "Categories",
                newName: "ix_categories_slug");

            migrationBuilder.RenameColumn(
                name: "Action",
                table: "AuditLogs",
                newName: "action");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AuditLogs",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "AuditLogs",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "UserEmail",
                table: "AuditLogs",
                newName: "user_email");

            migrationBuilder.RenameColumn(
                name: "UserAgent",
                table: "AuditLogs",
                newName: "user_agent");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "AuditLogs",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "AuditLogs",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "RowVersion",
                table: "AuditLogs",
                newName: "row_version");

            migrationBuilder.RenameColumn(
                name: "OldValues",
                table: "AuditLogs",
                newName: "old_values");

            migrationBuilder.RenameColumn(
                name: "NewValues",
                table: "AuditLogs",
                newName: "new_values");

            migrationBuilder.RenameColumn(
                name: "IsSuccessful",
                table: "AuditLogs",
                newName: "is_successful");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "AuditLogs",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IpAddress",
                table: "AuditLogs",
                newName: "ip_address");

            migrationBuilder.RenameColumn(
                name: "ErrorMessage",
                table: "AuditLogs",
                newName: "error_message");

            migrationBuilder.RenameColumn(
                name: "EntityType",
                table: "AuditLogs",
                newName: "entity_type");

            migrationBuilder.RenameColumn(
                name: "EntityId",
                table: "AuditLogs",
                newName: "entity_id");

            migrationBuilder.RenameColumn(
                name: "DeletedBy",
                table: "AuditLogs",
                newName: "deleted_by");

            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "AuditLogs",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "AuditLogs",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "AuditLogs",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "AdditionalData",
                table: "AuditLogs",
                newName: "additional_data");

            migrationBuilder.AlterColumn<Guid>(
                name: "id",
                table: "Roles",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");

            migrationBuilder.AlterColumn<uint>(
                name: "row_version",
                table: "AuditLogs",
                type: "xid",
                rowVersion: true,
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddPrimaryKey(
                name: "pk_users",
                table: "Users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_roles",
                table: "Roles",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_media_assets",
                table: "MediaAssets",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_contents",
                table: "Contents",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_categories",
                table: "Categories",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_audit_logs",
                table: "AuditLogs",
                column: "id");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"),
                column: "created_at",
                value: new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"),
                column: "created_at",
                value: new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"),
                column: "created_at",
                value: new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"),
                column: "created_at",
                value: new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000005"),
                column: "created_at",
                value: new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000006"),
                column: "created_at",
                value: new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000007"),
                column: "created_at",
                value: new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.AddForeignKey(
                name: "fk_contents_categories_category_id",
                table: "Contents",
                column: "category_id",
                principalTable: "Categories",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_media_assets_contents_content_id",
                table: "MediaAssets",
                column: "content_id",
                principalTable: "Contents",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
