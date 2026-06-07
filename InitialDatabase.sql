-- ============================================================
-- Migration: InitialDatabase
-- Generated from EF Core migration (BMedia.Infrastructure.Migrations)
-- Database: PostgreSQL 16+
-- ============================================================
--
-- IMPORTANT NOTE about xmin / row_version:
--   Several tables had a user-defined "row_version" column of type xid
--   created by the prior InitialCreate migration. EF Core with Npgsql maps
--   optimistic concurrency to the PostgreSQL SYSTEM column "xmin" (not a
--   user column). You CANNOT rename a user column to "xmin" in PostgreSQL
--   because "xmin" is a reserved system identifier.
--
--   CORRECT approach: DROP the "row_version" column; the system "xmin"
--   column already exists on every table automatically.
--
--   Affected tables: Users, Roles, Contents, MediaAssets, Categories.
--   (AuditLogs uses a regular bigint "RowVersion" column — handled normally.)
-- ============================================================

BEGIN;

-- ============================================================
-- SECTION 1: Drop existing foreign keys (snake_case names)
-- ============================================================

ALTER TABLE "Contents"    DROP CONSTRAINT "fk_contents_categories_category_id";
ALTER TABLE "MediaAssets" DROP CONSTRAINT "fk_media_assets_contents_content_id";

-- ============================================================
-- SECTION 2: Drop existing primary keys (snake_case names)
-- ============================================================

ALTER TABLE "Users"      DROP CONSTRAINT "pk_users";
ALTER TABLE "Roles"      DROP CONSTRAINT "pk_roles";
ALTER TABLE "MediaAssets" DROP CONSTRAINT "pk_media_assets";
ALTER TABLE "Contents"   DROP CONSTRAINT "pk_contents";
ALTER TABLE "Categories" DROP CONSTRAINT "pk_categories";
ALTER TABLE "AuditLogs"  DROP CONSTRAINT "pk_audit_logs";

-- ============================================================
-- SECTION 3: Rename columns on Users
--            + drop row_version (replaced by system xmin)
-- ============================================================

ALTER TABLE "Users" RENAME COLUMN "username"                       TO "Username";
ALTER TABLE "Users" RENAME COLUMN "email"                          TO "Email";
ALTER TABLE "Users" RENAME COLUMN "id"                             TO "Id";
ALTER TABLE "Users" RENAME COLUMN "updated_by"                     TO "UpdatedBy";
ALTER TABLE "Users" RENAME COLUMN "updated_at"                     TO "UpdatedAt";
ALTER TABLE "Users" RENAME COLUMN "time_zone"                      TO "TimeZone";
ALTER TABLE "Users" DROP   COLUMN "row_version";   -- system xmin takes over
ALTER TABLE "Users" RENAME COLUMN "profile_picture_url"            TO "ProfilePictureUrl";
ALTER TABLE "Users" RENAME COLUMN "preferred_language"             TO "PreferredLanguage";
ALTER TABLE "Users" RENAME COLUMN "phone_number"                   TO "PhoneNumber";
ALTER TABLE "Users" RENAME COLUMN "password_reset_token_expiry"    TO "PasswordResetTokenExpiry";
ALTER TABLE "Users" RENAME COLUMN "password_reset_token"           TO "PasswordResetToken";
ALTER TABLE "Users" RENAME COLUMN "password_hash"                  TO "PasswordHash";
ALTER TABLE "Users" RENAME COLUMN "lockout_end"                    TO "LockoutEnd";
ALTER TABLE "Users" RENAME COLUMN "last_name"                      TO "LastName";
ALTER TABLE "Users" RENAME COLUMN "last_login_ip"                  TO "LastLoginIp";
ALTER TABLE "Users" RENAME COLUMN "last_login_at"                  TO "LastLoginAt";
ALTER TABLE "Users" RENAME COLUMN "is_email_verified"              TO "IsEmailVerified";
ALTER TABLE "Users" RENAME COLUMN "is_deleted"                     TO "IsDeleted";
ALTER TABLE "Users" RENAME COLUMN "is_active"                      TO "IsActive";
ALTER TABLE "Users" RENAME COLUMN "first_name"                     TO "FirstName";
ALTER TABLE "Users" RENAME COLUMN "failed_login_attempts"          TO "FailedLoginAttempts";
ALTER TABLE "Users" RENAME COLUMN "email_verification_token_expiry" TO "EmailVerificationTokenExpiry";
ALTER TABLE "Users" RENAME COLUMN "email_verification_token"       TO "EmailVerificationToken";
ALTER TABLE "Users" RENAME COLUMN "deleted_by"                     TO "DeletedBy";
ALTER TABLE "Users" RENAME COLUMN "deleted_at"                     TO "DeletedAt";
ALTER TABLE "Users" RENAME COLUMN "created_by"                     TO "CreatedBy";
ALTER TABLE "Users" RENAME COLUMN "created_at"                     TO "CreatedAt";

ALTER INDEX "ix_users_username" RENAME TO "IX_Users_Username";
ALTER INDEX "ix_users_email"    RENAME TO "IX_Users_Email";

-- ============================================================
-- SECTION 4: Rename columns on Roles
--            + drop row_version + add DEFAULT on Id
-- ============================================================

ALTER TABLE "Roles" RENAME COLUMN "name"            TO "Name";
ALTER TABLE "Roles" RENAME COLUMN "description"     TO "Description";
ALTER TABLE "Roles" RENAME COLUMN "id"              TO "Id";
ALTER TABLE "Roles" RENAME COLUMN "updated_by"      TO "UpdatedBy";
ALTER TABLE "Roles" RENAME COLUMN "updated_at"      TO "UpdatedAt";
ALTER TABLE "Roles" DROP   COLUMN "row_version";    -- system xmin takes over
ALTER TABLE "Roles" RENAME COLUMN "normalized_name" TO "NormalizedName";
ALTER TABLE "Roles" RENAME COLUMN "is_system"       TO "IsSystem";
ALTER TABLE "Roles" RENAME COLUMN "is_deleted"      TO "IsDeleted";
ALTER TABLE "Roles" RENAME COLUMN "deleted_by"      TO "DeletedBy";
ALTER TABLE "Roles" RENAME COLUMN "deleted_at"      TO "DeletedAt";
ALTER TABLE "Roles" RENAME COLUMN "created_by"      TO "CreatedBy";
ALTER TABLE "Roles" RENAME COLUMN "created_at"      TO "CreatedAt";

ALTER INDEX "ix_roles_normalized_name" RENAME TO "IX_Roles_NormalizedName";

-- ============================================================
-- SECTION 5: Rename columns on MediaAssets
--            + drop row_version
-- ============================================================

ALTER TABLE "MediaAssets" RENAME COLUMN "width"                      TO "Width";
ALTER TABLE "MediaAssets" RENAME COLUMN "title"                      TO "Title";
ALTER TABLE "MediaAssets" RENAME COLUMN "status"                     TO "Status";
ALTER TABLE "MediaAssets" RENAME COLUMN "language"                   TO "Language";
ALTER TABLE "MediaAssets" RENAME COLUMN "height"                     TO "Height";
ALTER TABLE "MediaAssets" RENAME COLUMN "description"                TO "Description";
ALTER TABLE "MediaAssets" RENAME COLUMN "codec"                      TO "Codec";
ALTER TABLE "MediaAssets" RENAME COLUMN "bitrate"                    TO "Bitrate";
ALTER TABLE "MediaAssets" RENAME COLUMN "id"                         TO "Id";
ALTER TABLE "MediaAssets" RENAME COLUMN "updated_by"                 TO "UpdatedBy";
ALTER TABLE "MediaAssets" RENAME COLUMN "updated_at"                 TO "UpdatedAt";
ALTER TABLE "MediaAssets" RENAME COLUMN "thumbnail_url"              TO "ThumbnailUrl";
ALTER TABLE "MediaAssets" RENAME COLUMN "storage_provider"           TO "StorageProvider";
ALTER TABLE "MediaAssets" RENAME COLUMN "storage_key"                TO "StorageKey";
ALTER TABLE "MediaAssets" RENAME COLUMN "sort_order"                 TO "SortOrder";
ALTER TABLE "MediaAssets" DROP   COLUMN "row_version";              -- system xmin takes over
ALTER TABLE "MediaAssets" RENAME COLUMN "public_url"                 TO "PublicUrl";
ALTER TABLE "MediaAssets" RENAME COLUMN "preview_url"                TO "PreviewUrl";
ALTER TABLE "MediaAssets" RENAME COLUMN "original_file_name"         TO "OriginalFileName";
ALTER TABLE "MediaAssets" RENAME COLUMN "ocr_text"                   TO "OcrText";
ALTER TABLE "MediaAssets" RENAME COLUMN "media_type"                 TO "MediaType";
ALTER TABLE "MediaAssets" RENAME COLUMN "is_watermarked"             TO "IsWatermarked";
ALTER TABLE "MediaAssets" RENAME COLUMN "is_transcoding_complete"    TO "IsTranscodingComplete";
ALTER TABLE "MediaAssets" RENAME COLUMN "is_thumbnail_generated"     TO "IsThumbnailGenerated";
ALTER TABLE "MediaAssets" RENAME COLUMN "is_primary"                 TO "IsPrimary";
ALTER TABLE "MediaAssets" RENAME COLUMN "is_metadata_extracted"      TO "IsMetadataExtracted";
ALTER TABLE "MediaAssets" RENAME COLUMN "is_deleted"                 TO "IsDeleted";
ALTER TABLE "MediaAssets" RENAME COLUMN "hls_manifest_url"           TO "HlsManifestUrl";
ALTER TABLE "MediaAssets" RENAME COLUMN "frame_rate"                 TO "FrameRate";
ALTER TABLE "MediaAssets" RENAME COLUMN "file_size_bytes"            TO "FileSizeBytes";
ALTER TABLE "MediaAssets" RENAME COLUMN "extracted_metadata"         TO "ExtractedMetadata";
ALTER TABLE "MediaAssets" RENAME COLUMN "duration_seconds"           TO "DurationSeconds";
ALTER TABLE "MediaAssets" RENAME COLUMN "deleted_by"                 TO "DeletedBy";
ALTER TABLE "MediaAssets" RENAME COLUMN "deleted_at"                 TO "DeletedAt";
ALTER TABLE "MediaAssets" RENAME COLUMN "created_by"                 TO "CreatedBy";
ALTER TABLE "MediaAssets" RENAME COLUMN "created_at"                 TO "CreatedAt";
ALTER TABLE "MediaAssets" RENAME COLUMN "content_type"               TO "ContentType";
ALTER TABLE "MediaAssets" RENAME COLUMN "content_id"                 TO "ContentId";
ALTER TABLE "MediaAssets" RENAME COLUMN "cdn_url"                    TO "CdnUrl";
ALTER TABLE "MediaAssets" RENAME COLUMN "aspect_ratio"               TO "AspectRatio";
ALTER TABLE "MediaAssets" RENAME COLUMN "antivirus_scanned_at"       TO "AntivirusScannedAt";
ALTER TABLE "MediaAssets" RENAME COLUMN "antivirus_scan_passed"      TO "AntivirusScanPassed";
ALTER TABLE "MediaAssets" RENAME COLUMN "alt_text"                   TO "AltText";
ALTER TABLE "MediaAssets" RENAME COLUMN "ai_generated_tags"          TO "AiGeneratedTags";

ALTER INDEX "ix_media_assets_storage_key"  RENAME TO "IX_MediaAssets_StorageKey";
ALTER INDEX "ix_media_assets_status"       RENAME TO "IX_MediaAssets_Status";
ALTER INDEX "ix_media_assets_media_type"   RENAME TO "IX_MediaAssets_MediaType";
ALTER INDEX "ix_media_assets_content_id"   RENAME TO "IX_MediaAssets_ContentId";

-- ============================================================
-- SECTION 6: Rename columns on Contents
--            + drop row_version
-- ============================================================

ALTER TABLE "Contents" RENAME COLUMN "title"                       TO "Title";
ALTER TABLE "Contents" RENAME COLUMN "summary"                     TO "Summary";
ALTER TABLE "Contents" RENAME COLUMN "status"                      TO "Status";
ALTER TABLE "Contents" RENAME COLUMN "slug"                        TO "Slug";
ALTER TABLE "Contents" RENAME COLUMN "language"                    TO "Language";
ALTER TABLE "Contents" RENAME COLUMN "body"                        TO "Body";
ALTER TABLE "Contents" RENAME COLUMN "id"                          TO "Id";
ALTER TABLE "Contents" RENAME COLUMN "view_count"                  TO "ViewCount";
ALTER TABLE "Contents" RENAME COLUMN "updated_by"                  TO "UpdatedBy";
ALTER TABLE "Contents" RENAME COLUMN "updated_at"                  TO "UpdatedAt";
ALTER TABLE "Contents" RENAME COLUMN "subcategory_id"              TO "SubcategoryId";
ALTER TABLE "Contents" RENAME COLUMN "seo_title"                   TO "SeoTitle";
ALTER TABLE "Contents" RENAME COLUMN "seo_keywords"                TO "SeoKeywords";
ALTER TABLE "Contents" RENAME COLUMN "seo_description"             TO "SeoDescription";
ALTER TABLE "Contents" RENAME COLUMN "scheduled_publish_at"        TO "ScheduledPublishAt";
ALTER TABLE "Contents" DROP   COLUMN "row_version";               -- system xmin takes over
ALTER TABLE "Contents" RENAME COLUMN "published_by"                TO "PublishedBy";
ALTER TABLE "Contents" RENAME COLUMN "published_at"                TO "PublishedAt";
ALTER TABLE "Contents" RENAME COLUMN "is_featured"                 TO "IsFeatured";
ALTER TABLE "Contents" RENAME COLUMN "is_deleted"                  TO "IsDeleted";
ALTER TABLE "Contents" RENAME COLUMN "deleted_by"                  TO "DeletedBy";
ALTER TABLE "Contents" RENAME COLUMN "deleted_at"                  TO "DeletedAt";
ALTER TABLE "Contents" RENAME COLUMN "current_workflow_step_id"    TO "CurrentWorkflowStepId";
ALTER TABLE "Contents" RENAME COLUMN "created_by"                  TO "CreatedBy";
ALTER TABLE "Contents" RENAME COLUMN "created_at"                  TO "CreatedAt";
ALTER TABLE "Contents" RENAME COLUMN "category_id"                 TO "CategoryId";
ALTER TABLE "Contents" RENAME COLUMN "canonical_url"               TO "CanonicalUrl";
ALTER TABLE "Contents" RENAME COLUMN "assigned_reviewer_id"        TO "AssignedReviewerId";
ALTER TABLE "Contents" RENAME COLUMN "archived_at"                 TO "ArchivedAt";
ALTER TABLE "Contents" RENAME COLUMN "allow_comments"              TO "AllowComments";

ALTER INDEX "ix_contents_status"       RENAME TO "IX_Contents_Status";
ALTER INDEX "ix_contents_slug"         RENAME TO "IX_Contents_Slug";
ALTER INDEX "ix_contents_published_at" RENAME TO "IX_Contents_PublishedAt";
ALTER INDEX "ix_contents_category_id"  RENAME TO "IX_Contents_CategoryId";

-- ============================================================
-- SECTION 7: Rename columns on Categories
--            + drop row_version
-- ============================================================

ALTER TABLE "Categories" RENAME COLUMN "slug"        TO "Slug";
ALTER TABLE "Categories" RENAME COLUMN "name"        TO "Name";
ALTER TABLE "Categories" RENAME COLUMN "description" TO "Description";
ALTER TABLE "Categories" RENAME COLUMN "id"          TO "Id";
ALTER TABLE "Categories" RENAME COLUMN "updated_by"  TO "UpdatedBy";
ALTER TABLE "Categories" RENAME COLUMN "updated_at"  TO "UpdatedAt";
ALTER TABLE "Categories" RENAME COLUMN "sort_order"  TO "SortOrder";
ALTER TABLE "Categories" DROP   COLUMN "row_version"; -- system xmin takes over
ALTER TABLE "Categories" RENAME COLUMN "is_deleted"  TO "IsDeleted";
ALTER TABLE "Categories" RENAME COLUMN "is_active"   TO "IsActive";
ALTER TABLE "Categories" RENAME COLUMN "icon_url"    TO "IconUrl";
ALTER TABLE "Categories" RENAME COLUMN "deleted_by"  TO "DeletedBy";
ALTER TABLE "Categories" RENAME COLUMN "deleted_at"  TO "DeletedAt";
ALTER TABLE "Categories" RENAME COLUMN "created_by"  TO "CreatedBy";
ALTER TABLE "Categories" RENAME COLUMN "created_at"  TO "CreatedAt";

ALTER INDEX "ix_categories_slug" RENAME TO "IX_Categories_Slug";

-- ============================================================
-- SECTION 8: Rename columns on AuditLogs
--            (uses a regular bigint RowVersion, NOT xmin)
-- ============================================================

ALTER TABLE "AuditLogs" RENAME COLUMN "action"          TO "Action";
ALTER TABLE "AuditLogs" RENAME COLUMN "id"              TO "Id";
ALTER TABLE "AuditLogs" RENAME COLUMN "user_id"         TO "UserId";
ALTER TABLE "AuditLogs" RENAME COLUMN "user_email"      TO "UserEmail";
ALTER TABLE "AuditLogs" RENAME COLUMN "user_agent"      TO "UserAgent";
ALTER TABLE "AuditLogs" RENAME COLUMN "updated_by"      TO "UpdatedBy";
ALTER TABLE "AuditLogs" RENAME COLUMN "updated_at"      TO "UpdatedAt";
ALTER TABLE "AuditLogs" RENAME COLUMN "row_version"     TO "RowVersion";
ALTER TABLE "AuditLogs" RENAME COLUMN "old_values"      TO "OldValues";
ALTER TABLE "AuditLogs" RENAME COLUMN "new_values"      TO "NewValues";
ALTER TABLE "AuditLogs" RENAME COLUMN "is_successful"   TO "IsSuccessful";
ALTER TABLE "AuditLogs" RENAME COLUMN "is_deleted"      TO "IsDeleted";
ALTER TABLE "AuditLogs" RENAME COLUMN "ip_address"      TO "IpAddress";
ALTER TABLE "AuditLogs" RENAME COLUMN "error_message"   TO "ErrorMessage";
ALTER TABLE "AuditLogs" RENAME COLUMN "entity_type"     TO "EntityType";
ALTER TABLE "AuditLogs" RENAME COLUMN "entity_id"       TO "EntityId";
ALTER TABLE "AuditLogs" RENAME COLUMN "deleted_by"      TO "DeletedBy";
ALTER TABLE "AuditLogs" RENAME COLUMN "deleted_at"      TO "DeletedAt";
ALTER TABLE "AuditLogs" RENAME COLUMN "created_by"      TO "CreatedBy";
ALTER TABLE "AuditLogs" RENAME COLUMN "created_at"      TO "CreatedAt";
ALTER TABLE "AuditLogs" RENAME COLUMN "additional_data" TO "AdditionalData";

-- ============================================================
-- SECTION 9: Alter column types
-- ============================================================

-- Roles.Id: add DEFAULT gen_random_uuid()
ALTER TABLE "Roles" ALTER COLUMN "Id" SET DEFAULT gen_random_uuid();

-- AuditLogs.RowVersion: change from xid (system row version) to bigint
ALTER TABLE "AuditLogs" ALTER COLUMN "RowVersion" TYPE bigint
    USING "RowVersion"::text::bigint;

-- ============================================================
-- SECTION 10: Re-add primary keys with PascalCase names
-- ============================================================

ALTER TABLE "Users"      ADD CONSTRAINT "PK_Users"      PRIMARY KEY ("Id");
ALTER TABLE "Roles"      ADD CONSTRAINT "PK_Roles"      PRIMARY KEY ("Id");
ALTER TABLE "MediaAssets" ADD CONSTRAINT "PK_MediaAssets" PRIMARY KEY ("Id");
ALTER TABLE "Contents"   ADD CONSTRAINT "PK_Contents"   PRIMARY KEY ("Id");
ALTER TABLE "Categories" ADD CONSTRAINT "PK_Categories" PRIMARY KEY ("Id");
ALTER TABLE "AuditLogs"  ADD CONSTRAINT "PK_AuditLogs"  PRIMARY KEY ("Id");

-- ============================================================
-- SECTION 11: Create new tables
-- ============================================================

-- Attachments
CREATE TABLE "Attachments" (
    "Id"            uuid                        NOT NULL DEFAULT gen_random_uuid(),
    "ContentId"     uuid                        NOT NULL,
    "FileName"      character varying(512)      NOT NULL,
    "StorageKey"    character varying(1024)     NOT NULL,
    "PublicUrl"     character varying(2048),
    "ContentType"   character varying(255)      NOT NULL,
    "FileSizeBytes" bigint                      NOT NULL,
    "Description"   character varying(1000),
    "SortOrder"     integer                     NOT NULL,
    "CreatedAt"     timestamp with time zone    NOT NULL,
    "CreatedBy"     uuid,
    "UpdatedAt"     timestamp with time zone,
    "UpdatedBy"     uuid,
    "IsDeleted"     boolean                     NOT NULL,
    "DeletedAt"     timestamp with time zone,
    "DeletedBy"     uuid,
    CONSTRAINT "PK_Attachments" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Attachments_Contents_ContentId"
        FOREIGN KEY ("ContentId") REFERENCES "Contents" ("Id") ON DELETE CASCADE
);

-- Localizations
CREATE TABLE "Localizations" (
    "Id"             uuid                        NOT NULL DEFAULT gen_random_uuid(),
    "ContentId"      uuid                        NOT NULL,
    "Language"       character varying(10)       NOT NULL,
    "Title"          character varying(500)      NOT NULL,
    "Summary"        character varying(2000),
    "Body"           text,
    "SeoTitle"       character varying(300),
    "SeoDescription" character varying(500),
    "IsApproved"     boolean                     NOT NULL,
    "ApprovedAt"     timestamp with time zone,
    "ApprovedBy"     uuid,
    "CreatedAt"      timestamp with time zone    NOT NULL,
    "CreatedBy"      uuid,
    "UpdatedAt"      timestamp with time zone,
    "UpdatedBy"      uuid,
    "IsDeleted"      boolean                     NOT NULL,
    "DeletedAt"      timestamp with time zone,
    "DeletedBy"      uuid,
    CONSTRAINT "PK_Localizations" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Localizations_Contents_ContentId"
        FOREIGN KEY ("ContentId") REFERENCES "Contents" ("Id") ON DELETE CASCADE
);

-- MediaVersions
CREATE TABLE "MediaVersions" (
    "Id"              uuid                        NOT NULL DEFAULT gen_random_uuid(),
    "MediaAssetId"    uuid                        NOT NULL,
    "StorageKey"      character varying(1024)     NOT NULL,
    "PublicUrl"       character varying(2048),
    "CdnUrl"          character varying(2048),
    "ContentType"     character varying(255)      NOT NULL,
    "FileSizeBytes"   bigint                      NOT NULL,
    "Width"           integer,
    "Height"          integer,
    "Bitrate"         integer,
    "DurationSeconds" integer,
    "Quality"         integer,
    "VersionLabel"    character varying(100)      NOT NULL,
    "IsHls"           boolean                     NOT NULL,
    "HlsManifestUrl"  character varying(2048),
    "IsDefault"       boolean                     NOT NULL,
    "StorageProvider" integer                     NOT NULL DEFAULT 1,
    "CreatedAt"       timestamp with time zone    NOT NULL,
    "CreatedBy"       uuid,
    "UpdatedAt"       timestamp with time zone,
    "UpdatedBy"       uuid,
    "IsDeleted"       boolean                     NOT NULL,
    "DeletedAt"       timestamp with time zone,
    "DeletedBy"       uuid,
    CONSTRAINT "PK_MediaVersions" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_MediaVersions_MediaAssets_MediaAssetId"
        FOREIGN KEY ("MediaAssetId") REFERENCES "MediaAssets" ("Id") ON DELETE CASCADE
);

-- Notifications
CREATE TABLE "Notifications" (
    "Id"            uuid                        NOT NULL DEFAULT gen_random_uuid(),
    "UserId"        uuid                        NOT NULL,
    "Type"          integer                     NOT NULL,
    "Title"         character varying(300)      NOT NULL,
    "Message"       character varying(2000)     NOT NULL,
    "IsRead"        boolean                     NOT NULL,
    "ReadAt"        timestamp with time zone,
    "ReferenceId"   uuid,
    "ReferenceType" character varying(100),
    "ActionUrl"     character varying(2048),
    "Metadata"      jsonb,
    "CreatedAt"     timestamp with time zone    NOT NULL,
    "CreatedBy"     uuid,
    "UpdatedAt"     timestamp with time zone,
    "UpdatedBy"     uuid,
    "IsDeleted"     boolean                     NOT NULL,
    "DeletedAt"     timestamp with time zone,
    "DeletedBy"     uuid,
    CONSTRAINT "PK_Notifications" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Notifications_Users_UserId"
        FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

-- Permissions
CREATE TABLE "Permissions" (
    "Id"             uuid                        NOT NULL DEFAULT gen_random_uuid(),
    "Name"           character varying(100)      NOT NULL,
    "NormalizedName" character varying(100)      NOT NULL,
    "Description"    character varying(500),
    "Module"         character varying(100)      NOT NULL,
    "CreatedAt"      timestamp with time zone    NOT NULL,
    "CreatedBy"      uuid,
    "UpdatedAt"      timestamp with time zone,
    "UpdatedBy"      uuid,
    "IsDeleted"      boolean                     NOT NULL,
    "DeletedAt"      timestamp with time zone,
    "DeletedBy"      uuid,
    CONSTRAINT "PK_Permissions" PRIMARY KEY ("Id")
);

-- RefreshTokens
CREATE TABLE "RefreshTokens" (
    "Id"             uuid                        NOT NULL DEFAULT gen_random_uuid(),
    "UserId"         uuid                        NOT NULL,
    "Token"          character varying(512)      NOT NULL,
    "ExpiresAt"      timestamp with time zone    NOT NULL,
    "IsRevoked"      boolean                     NOT NULL,
    "RevokedAt"      timestamp with time zone,
    "RevokedReason"  character varying(500),
    "ReplacedByToken" text,
    "CreatedByIp"    character varying(50),
    "RevokedByIp"    character varying(50),
    "CreatedAt"      timestamp with time zone    NOT NULL,
    "CreatedBy"      uuid,
    "UpdatedAt"      timestamp with time zone,
    "UpdatedBy"      uuid,
    "IsDeleted"      boolean                     NOT NULL,
    "DeletedAt"      timestamp with time zone,
    "DeletedBy"      uuid,
    "RowVersion"     bigint                      NOT NULL,
    CONSTRAINT "PK_RefreshTokens" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_RefreshTokens_Users_UserId"
        FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

-- ReviewComments
CREATE TABLE "ReviewComments" (
    "Id"              uuid                        NOT NULL DEFAULT gen_random_uuid(),
    "ContentId"       uuid                        NOT NULL,
    "ReviewerId"      uuid                        NOT NULL,
    "Comment"         character varying(5000)     NOT NULL,
    "IsResolved"      boolean                     NOT NULL,
    "ResolvedAt"      timestamp with time zone,
    "ResolvedBy"      uuid,
    "ParentCommentId" uuid,
    "CreatedAt"       timestamp with time zone    NOT NULL,
    "CreatedBy"       uuid,
    "UpdatedAt"       timestamp with time zone,
    "UpdatedBy"       uuid,
    "IsDeleted"       boolean                     NOT NULL,
    "DeletedAt"       timestamp with time zone,
    "DeletedBy"       uuid,
    CONSTRAINT "PK_ReviewComments" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ReviewComments_Contents_ContentId"
        FOREIGN KEY ("ContentId") REFERENCES "Contents" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_ReviewComments_ReviewComments_ParentCommentId"
        FOREIGN KEY ("ParentCommentId") REFERENCES "ReviewComments" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_ReviewComments_Users_ReviewerId"
        FOREIGN KEY ("ReviewerId") REFERENCES "Users" ("Id") ON DELETE RESTRICT
);

-- ScheduledPublications
CREATE TABLE "ScheduledPublications" (
    "Id"             uuid                        NOT NULL DEFAULT gen_random_uuid(),
    "ContentId"      uuid                        NOT NULL,
    "ScheduledAt"    timestamp with time zone    NOT NULL,
    "IsExecuted"     boolean                     NOT NULL,
    "ExecutedAt"     timestamp with time zone,
    "IsSuccessful"   boolean                     NOT NULL,
    "ErrorMessage"   character varying(2000),
    "HangfireJobId"  character varying(200),
    "CreatedAt"      timestamp with time zone    NOT NULL,
    "CreatedBy"      uuid,
    "UpdatedAt"      timestamp with time zone,
    "UpdatedBy"      uuid,
    "IsDeleted"      boolean                     NOT NULL,
    "DeletedAt"      timestamp with time zone,
    "DeletedBy"      uuid,
    CONSTRAINT "PK_ScheduledPublications" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ScheduledPublications_Contents_ContentId"
        FOREIGN KEY ("ContentId") REFERENCES "Contents" ("Id") ON DELETE CASCADE
);

-- Subcategories
CREATE TABLE "Subcategories" (
    "Id"          uuid                        NOT NULL DEFAULT gen_random_uuid(),
    "Name"        character varying(200)      NOT NULL,
    "Slug"        character varying(250)      NOT NULL,
    "Description" character varying(1000),
    "SortOrder"   integer                     NOT NULL,
    "IsActive"    boolean                     NOT NULL,
    "CategoryId"  uuid                        NOT NULL,
    "CreatedAt"   timestamp with time zone    NOT NULL,
    "CreatedBy"   uuid,
    "UpdatedAt"   timestamp with time zone,
    "UpdatedBy"   uuid,
    "IsDeleted"   boolean                     NOT NULL,
    "DeletedAt"   timestamp with time zone,
    "DeletedBy"   uuid,
    CONSTRAINT "PK_Subcategories" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Subcategories_Categories_CategoryId"
        FOREIGN KEY ("CategoryId") REFERENCES "Categories" ("Id") ON DELETE CASCADE
);

-- Tags
CREATE TABLE "Tags" (
    "Id"           uuid                        NOT NULL DEFAULT gen_random_uuid(),
    "Name"         character varying(150)      NOT NULL,
    "Slug"         character varying(200)      NOT NULL,
    "Description"  character varying(500),
    "IsAiGenerated" boolean                    NOT NULL,
    "CreatedAt"    timestamp with time zone    NOT NULL,
    "CreatedBy"    uuid,
    "UpdatedAt"    timestamp with time zone,
    "UpdatedBy"    uuid,
    "IsDeleted"    boolean                     NOT NULL,
    "DeletedAt"    timestamp with time zone,
    "DeletedBy"    uuid,
    CONSTRAINT "PK_Tags" PRIMARY KEY ("Id")
);

-- UserRoles
CREATE TABLE "UserRoles" (
    "UserId"     uuid                        NOT NULL,
    "RoleId"     uuid                        NOT NULL,
    "AssignedAt" timestamp with time zone    NOT NULL,
    "AssignedBy" uuid,
    CONSTRAINT "PK_UserRoles" PRIMARY KEY ("UserId", "RoleId"),
    CONSTRAINT "FK_UserRoles_Roles_RoleId"
        FOREIGN KEY ("RoleId") REFERENCES "Roles" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_UserRoles_Users_UserId"
        FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

-- WorkflowDefinitions
CREATE TABLE "WorkflowDefinitions" (
    "Id"          uuid                        NOT NULL DEFAULT gen_random_uuid(),
    "Name"        character varying(200)      NOT NULL,
    "Description" character varying(1000),
    "IsDefault"   boolean                     NOT NULL,
    "IsActive"    boolean                     NOT NULL,
    "Version"     integer                     NOT NULL,
    "CreatedAt"   timestamp with time zone    NOT NULL,
    "CreatedBy"   uuid,
    "UpdatedAt"   timestamp with time zone,
    "UpdatedBy"   uuid,
    "IsDeleted"   boolean                     NOT NULL,
    "DeletedAt"   timestamp with time zone,
    "DeletedBy"   uuid,
    CONSTRAINT "PK_WorkflowDefinitions" PRIMARY KEY ("Id")
);

-- RolePermissions (depends on Roles + Permissions)
CREATE TABLE "RolePermissions" (
    "RoleId"       uuid                        NOT NULL,
    "PermissionId" uuid                        NOT NULL,
    "GrantedAt"    timestamp with time zone    NOT NULL,
    "GrantedBy"    uuid,
    CONSTRAINT "PK_RolePermissions" PRIMARY KEY ("RoleId", "PermissionId"),
    CONSTRAINT "FK_RolePermissions_Permissions_PermissionId"
        FOREIGN KEY ("PermissionId") REFERENCES "Permissions" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_RolePermissions_Roles_RoleId"
        FOREIGN KEY ("RoleId") REFERENCES "Roles" ("Id") ON DELETE CASCADE
);

-- ContentTags (depends on Contents + Tags)
CREATE TABLE "ContentTags" (
    "ContentId"    uuid                        NOT NULL,
    "TagId"        uuid                        NOT NULL,
    "TaggedAt"     timestamp with time zone    NOT NULL,
    "IsAiGenerated" boolean                    NOT NULL,
    CONSTRAINT "PK_ContentTags" PRIMARY KEY ("ContentId", "TagId"),
    CONSTRAINT "FK_ContentTags_Contents_ContentId"
        FOREIGN KEY ("ContentId") REFERENCES "Contents" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_ContentTags_Tags_TagId"
        FOREIGN KEY ("TagId") REFERENCES "Tags" ("Id") ON DELETE CASCADE
);

-- WorkflowSteps (depends on WorkflowDefinitions)
CREATE TABLE "WorkflowSteps" (
    "Id"                   uuid                        NOT NULL DEFAULT gen_random_uuid(),
    "WorkflowDefinitionId" uuid                        NOT NULL,
    "Name"                 character varying(200)      NOT NULL,
    "Description"          character varying(500),
    "MapsToStatus"         integer                     NOT NULL,
    "Order"                integer                     NOT NULL,
    "IsInitial"            boolean                     NOT NULL,
    "IsFinal"              boolean                     NOT NULL,
    "RequiresReviewer"     boolean                     NOT NULL,
    "RequiredPermission"   character varying(200),
    "SlaHours"             integer,
    "CreatedAt"            timestamp with time zone    NOT NULL,
    "CreatedBy"            uuid,
    "UpdatedAt"            timestamp with time zone,
    "UpdatedBy"            uuid,
    "IsDeleted"            boolean                     NOT NULL,
    "DeletedAt"            timestamp with time zone,
    "DeletedBy"            uuid,
    CONSTRAINT "PK_WorkflowSteps" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_WorkflowSteps_WorkflowDefinitions_WorkflowDefinitionId"
        FOREIGN KEY ("WorkflowDefinitionId") REFERENCES "WorkflowDefinitions" ("Id") ON DELETE CASCADE
);

-- ContentWorkflowHistories (depends on Contents, Users, WorkflowSteps)
CREATE TABLE "ContentWorkflowHistories" (
    "Id"               uuid                        NOT NULL DEFAULT gen_random_uuid(),
    "ContentId"        uuid                        NOT NULL,
    "FromStepId"       uuid,
    "ToStepId"         uuid                        NOT NULL,
    "TransitionedById" uuid                        NOT NULL,
    "FromStatus"       integer                     NOT NULL,
    "ToStatus"         integer                     NOT NULL,
    "Comment"          character varying(2000),
    "ActionName"       character varying(200),
    "TransitionedAt"   timestamp with time zone    NOT NULL,
    "CreatedAt"        timestamp with time zone    NOT NULL,
    "CreatedBy"        uuid,
    "UpdatedAt"        timestamp with time zone,
    "UpdatedBy"        uuid,
    "IsDeleted"        boolean                     NOT NULL,
    "DeletedAt"        timestamp with time zone,
    "DeletedBy"        uuid,
    "RowVersion"       bigint                      NOT NULL,
    CONSTRAINT "PK_ContentWorkflowHistories" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ContentWorkflowHistories_Contents_ContentId"
        FOREIGN KEY ("ContentId") REFERENCES "Contents" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_ContentWorkflowHistories_Users_TransitionedById"
        FOREIGN KEY ("TransitionedById") REFERENCES "Users" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_ContentWorkflowHistories_WorkflowSteps_FromStepId"
        FOREIGN KEY ("FromStepId") REFERENCES "WorkflowSteps" ("Id") ON DELETE SET NULL,
    CONSTRAINT "FK_ContentWorkflowHistories_WorkflowSteps_ToStepId"
        FOREIGN KEY ("ToStepId") REFERENCES "WorkflowSteps" ("Id") ON DELETE RESTRICT
);

-- WorkflowTransitions (depends on WorkflowDefinitions, WorkflowSteps)
CREATE TABLE "WorkflowTransitions" (
    "Id"                   uuid                        NOT NULL DEFAULT gen_random_uuid(),
    "WorkflowDefinitionId" uuid                        NOT NULL,
    "FromStepId"           uuid                        NOT NULL,
    "ToStepId"             uuid                        NOT NULL,
    "ActionName"           character varying(200)      NOT NULL,
    "Description"          character varying(500),
    "RequiredPermission"   character varying(200),
    "RequiresComment"      boolean                     NOT NULL,
    "IsActive"             boolean                     NOT NULL,
    "CreatedAt"            timestamp with time zone    NOT NULL,
    "CreatedBy"            uuid,
    "UpdatedAt"            timestamp with time zone,
    "UpdatedBy"            uuid,
    "IsDeleted"            boolean                     NOT NULL,
    "DeletedAt"            timestamp with time zone,
    "DeletedBy"            uuid,
    CONSTRAINT "PK_WorkflowTransitions" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_WorkflowTransitions_WorkflowDefinitions_WorkflowDefinitionId"
        FOREIGN KEY ("WorkflowDefinitionId") REFERENCES "WorkflowDefinitions" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_WorkflowTransitions_WorkflowSteps_FromStepId"
        FOREIGN KEY ("FromStepId") REFERENCES "WorkflowSteps" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_WorkflowTransitions_WorkflowSteps_ToStepId"
        FOREIGN KEY ("ToStepId") REFERENCES "WorkflowSteps" ("Id") ON DELETE RESTRICT
);

-- ============================================================
-- SECTION 12: Seed data — Permissions (17 rows)
-- ============================================================
-- Timestamps converted from .NET DateTime ticks to microseconds:
--   AddTicks(n) → n * 100 ns → n/10 μs (truncated)

INSERT INTO "Permissions" ("Id", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Description", "IsDeleted", "Module", "Name", "NormalizedName", "UpdatedAt", "UpdatedBy")
VALUES
  ('20000000-0000-0000-0000-000000000001', '2026-05-20 13:33:35.533352+00', NULL, NULL, NULL, 'CreateContent permission',      FALSE, 'Content',  'CreateContent',      'CREATECONTENT',      NULL, NULL),
  ('20000000-0000-0000-0000-000000000002', '2026-05-20 13:33:35.534274+00', NULL, NULL, NULL, 'EditContent permission',        FALSE, 'Content',  'EditContent',        'EDITCONTENT',        NULL, NULL),
  ('20000000-0000-0000-0000-000000000003', '2026-05-20 13:33:35.534283+00', NULL, NULL, NULL, 'DeleteContent permission',      FALSE, 'Content',  'DeleteContent',      'DELETECONTENT',      NULL, NULL),
  ('20000000-0000-0000-0000-000000000004', '2026-05-20 13:33:35.534283+00', NULL, NULL, NULL, 'PublishContent permission',     FALSE, 'Content',  'PublishContent',     'PUBLISHCONTENT',     NULL, NULL),
  ('20000000-0000-0000-0000-000000000005', '2026-05-20 13:33:35.534284+00', NULL, NULL, NULL, 'ArchiveContent permission',     FALSE, 'Content',  'ArchiveContent',     'ARCHIVECONTENT',     NULL, NULL),
  ('20000000-0000-0000-0000-000000000006', '2026-05-20 13:33:35.534284+00', NULL, NULL, NULL, 'ViewContent permission',        FALSE, 'Content',  'ViewContent',        'VIEWCONTENT',        NULL, NULL),
  ('20000000-0000-0000-0000-000000000007', '2026-05-20 13:33:35.534284+00', NULL, NULL, NULL, 'UploadMedia permission',        FALSE, 'Media',    'UploadMedia',        'UPLOADMEDIA',        NULL, NULL),
  ('20000000-0000-0000-0000-000000000008', '2026-05-20 13:33:35.534285+00', NULL, NULL, NULL, 'DeleteMedia permission',        FALSE, 'Media',    'DeleteMedia',        'DELETEMEDIA',        NULL, NULL),
  ('20000000-0000-0000-0000-000000000009', '2026-05-20 13:33:35.534286+00', NULL, NULL, NULL, 'ManageMedia permission',        FALSE, 'Media',    'ManageMedia',        'MANAGEMEDIA',        NULL, NULL),
  ('20000000-0000-0000-0000-000000000010', '2026-05-20 13:33:35.534320+00', NULL, NULL, NULL, 'ApproveReview permission',      FALSE, 'Workflow', 'ApproveReview',      'APPROVEREVIEW',      NULL, NULL),
  ('20000000-0000-0000-0000-000000000011', '2026-05-20 13:33:35.534322+00', NULL, NULL, NULL, 'RejectReview permission',       FALSE, 'Workflow', 'RejectReview',       'REJECTREVIEW',       NULL, NULL),
  ('20000000-0000-0000-0000-000000000012', '2026-05-20 13:33:35.534322+00', NULL, NULL, NULL, 'TransitionWorkflow permission', FALSE, 'Workflow', 'TransitionWorkflow', 'TRANSITIONWORKFLOW', NULL, NULL),
  ('20000000-0000-0000-0000-000000000013', '2026-05-20 13:33:35.534322+00', NULL, NULL, NULL, 'ManageUsers permission',        FALSE, 'Admin',    'ManageUsers',        'MANAGEUSERS',        NULL, NULL),
  ('20000000-0000-0000-0000-000000000014', '2026-05-20 13:33:35.534322+00', NULL, NULL, NULL, 'ManageRoles permission',        FALSE, 'Admin',    'ManageRoles',        'MANAGEROLES',        NULL, NULL),
  ('20000000-0000-0000-0000-000000000015', '2026-05-20 13:33:35.534323+00', NULL, NULL, NULL, 'ViewAuditLogs permission',      FALSE, 'Admin',    'ViewAuditLogs',      'VIEWAUDITLOGS',      NULL, NULL),
  ('20000000-0000-0000-0000-000000000016', '2026-05-20 13:33:35.534323+00', NULL, NULL, NULL, 'ManageCategories permission',   FALSE, 'Taxonomy', 'ManageCategories',   'MANAGECATEGORIES',   NULL, NULL),
  ('20000000-0000-0000-0000-000000000017', '2026-05-20 13:33:35.534323+00', NULL, NULL, NULL, 'ManageTags permission',         FALSE, 'Taxonomy', 'ManageTags',         'MANAGETAGS',         NULL, NULL);

-- ============================================================
-- SECTION 13: Update seed Roles — refresh CreatedAt timestamps
-- ============================================================

UPDATE "Roles" SET "CreatedAt" = '2026-05-20 13:33:35.539030+00' WHERE "Id" = '10000000-0000-0000-0000-000000000001';
UPDATE "Roles" SET "CreatedAt" = '2026-05-20 13:33:35.539106+00' WHERE "Id" = '10000000-0000-0000-0000-000000000002';
UPDATE "Roles" SET "CreatedAt" = '2026-05-20 13:33:35.539107+00' WHERE "Id" = '10000000-0000-0000-0000-000000000003';
UPDATE "Roles" SET "CreatedAt" = '2026-05-20 13:33:35.539107+00' WHERE "Id" = '10000000-0000-0000-0000-000000000004';
UPDATE "Roles" SET "CreatedAt" = '2026-05-20 13:33:35.539107+00' WHERE "Id" = '10000000-0000-0000-0000-000000000005';
UPDATE "Roles" SET "CreatedAt" = '2026-05-20 13:33:35.539107+00' WHERE "Id" = '10000000-0000-0000-0000-000000000006';
UPDATE "Roles" SET "CreatedAt" = '2026-05-20 13:33:35.539108+00' WHERE "Id" = '10000000-0000-0000-0000-000000000007';

-- ============================================================
-- SECTION 14: Create indexes
-- ============================================================

-- Users
CREATE INDEX "IX_Users_IsActive"  ON "Users" ("IsActive");
CREATE INDEX "IX_Users_IsDeleted" ON "Users" ("IsDeleted");

-- MediaAssets
CREATE INDEX "IX_MediaAssets_ContentId_IsPrimary" ON "MediaAssets" ("ContentId", "IsPrimary");

-- Contents
CREATE INDEX        "IX_Contents_CreatedAt"              ON "Contents" ("CreatedAt");
CREATE INDEX        "IX_Contents_CurrentWorkflowStepId"  ON "Contents" ("CurrentWorkflowStepId");
CREATE INDEX        "IX_Contents_IsFeatured"             ON "Contents" ("IsFeatured");
CREATE INDEX        "IX_Contents_Language"               ON "Contents" ("Language");
CREATE INDEX        "IX_Contents_Status_Language_IsDeleted" ON "Contents" ("Status", "Language", "IsDeleted");
CREATE INDEX        "IX_Contents_SubcategoryId"          ON "Contents" ("SubcategoryId");

-- AuditLogs
CREATE INDEX "IX_AuditLogs_Action"            ON "AuditLogs" ("Action");
CREATE INDEX "IX_AuditLogs_CreatedAt"         ON "AuditLogs" ("CreatedAt");
CREATE INDEX "IX_AuditLogs_EntityType"        ON "AuditLogs" ("EntityType");
CREATE INDEX "IX_AuditLogs_EntityType_EntityId" ON "AuditLogs" ("EntityType", "EntityId");
CREATE INDEX "IX_AuditLogs_UserId"            ON "AuditLogs" ("UserId");

-- Attachments
CREATE INDEX "IX_Attachments_ContentId" ON "Attachments" ("ContentId");

-- ContentTags
CREATE INDEX "IX_ContentTags_TagId" ON "ContentTags" ("TagId");

-- ContentWorkflowHistories
CREATE INDEX "IX_ContentWorkflowHistories_ContentId"      ON "ContentWorkflowHistories" ("ContentId");
CREATE INDEX "IX_ContentWorkflowHistories_FromStepId"     ON "ContentWorkflowHistories" ("FromStepId");
CREATE INDEX "IX_ContentWorkflowHistories_ToStepId"       ON "ContentWorkflowHistories" ("ToStepId");
CREATE INDEX "IX_ContentWorkflowHistories_TransitionedAt" ON "ContentWorkflowHistories" ("TransitionedAt");
CREATE INDEX "IX_ContentWorkflowHistories_TransitionedById" ON "ContentWorkflowHistories" ("TransitionedById");

-- Localizations (partial unique index)
CREATE UNIQUE INDEX "IX_Localizations_ContentId_Language"
    ON "Localizations" ("ContentId", "Language")
    WHERE "IsDeleted" = false;

-- MediaVersions
CREATE INDEX "IX_MediaVersions_IsDefault"    ON "MediaVersions" ("IsDefault");
CREATE INDEX "IX_MediaVersions_MediaAssetId" ON "MediaVersions" ("MediaAssetId");
CREATE INDEX "IX_MediaVersions_Quality"      ON "MediaVersions" ("Quality");

-- Notifications
CREATE INDEX "IX_Notifications_CreatedAt"      ON "Notifications" ("CreatedAt");
CREATE INDEX "IX_Notifications_UserId"         ON "Notifications" ("UserId");
CREATE INDEX "IX_Notifications_UserId_IsRead"  ON "Notifications" ("UserId", "IsRead");

-- Permissions (partial unique index)
CREATE INDEX        "IX_Permissions_Module" ON "Permissions" ("Module");
CREATE UNIQUE INDEX "IX_Permissions_NormalizedName"
    ON "Permissions" ("NormalizedName")
    WHERE "IsDeleted" = false;

-- RefreshTokens
CREATE INDEX        "IX_RefreshTokens_ExpiresAt" ON "RefreshTokens" ("ExpiresAt");
CREATE UNIQUE INDEX "IX_RefreshTokens_Token"     ON "RefreshTokens" ("Token");
CREATE INDEX        "IX_RefreshTokens_UserId"    ON "RefreshTokens" ("UserId");

-- ReviewComments
CREATE INDEX "IX_ReviewComments_ContentId"      ON "ReviewComments" ("ContentId");
CREATE INDEX "IX_ReviewComments_ParentCommentId" ON "ReviewComments" ("ParentCommentId");
CREATE INDEX "IX_ReviewComments_ReviewerId"     ON "ReviewComments" ("ReviewerId");

-- RolePermissions
CREATE INDEX "IX_RolePermissions_PermissionId" ON "RolePermissions" ("PermissionId");

-- ScheduledPublications
CREATE INDEX "IX_ScheduledPublications_ContentId"              ON "ScheduledPublications" ("ContentId");
CREATE INDEX "IX_ScheduledPublications_ScheduledAt_IsExecuted" ON "ScheduledPublications" ("ScheduledAt", "IsExecuted");

-- Subcategories (partial unique index)
CREATE INDEX        "IX_Subcategories_CategoryId" ON "Subcategories" ("CategoryId");
CREATE UNIQUE INDEX "IX_Subcategories_Slug"
    ON "Subcategories" ("Slug")
    WHERE "IsDeleted" = false;

-- Tags (partial unique index)
CREATE UNIQUE INDEX "IX_Tags_Slug"
    ON "Tags" ("Slug")
    WHERE "IsDeleted" = false;

-- UserRoles
CREATE INDEX "IX_UserRoles_RoleId" ON "UserRoles" ("RoleId");
CREATE INDEX "IX_UserRoles_UserId" ON "UserRoles" ("UserId");

-- WorkflowDefinitions
CREATE INDEX "IX_WorkflowDefinitions_IsDefault" ON "WorkflowDefinitions" ("IsDefault");

-- WorkflowSteps
CREATE INDEX "IX_WorkflowSteps_WorkflowDefinitionId"       ON "WorkflowSteps" ("WorkflowDefinitionId");
CREATE INDEX "IX_WorkflowSteps_WorkflowDefinitionId_Order" ON "WorkflowSteps" ("WorkflowDefinitionId", "Order");

-- WorkflowTransitions
CREATE UNIQUE INDEX "IX_WorkflowTransitions_FromStepId_ToStepId"
    ON "WorkflowTransitions" ("FromStepId", "ToStepId");
CREATE INDEX "IX_WorkflowTransitions_ToStepId"             ON "WorkflowTransitions" ("ToStepId");
CREATE INDEX "IX_WorkflowTransitions_WorkflowDefinitionId" ON "WorkflowTransitions" ("WorkflowDefinitionId");

-- ============================================================
-- SECTION 15: Add foreign keys on existing tables (PascalCase)
-- ============================================================

ALTER TABLE "Contents"
    ADD CONSTRAINT "FK_Contents_Categories_CategoryId"
        FOREIGN KEY ("CategoryId") REFERENCES "Categories" ("Id") ON DELETE SET NULL;

ALTER TABLE "Contents"
    ADD CONSTRAINT "FK_Contents_Subcategories_SubcategoryId"
        FOREIGN KEY ("SubcategoryId") REFERENCES "Subcategories" ("Id") ON DELETE SET NULL;

ALTER TABLE "Contents"
    ADD CONSTRAINT "FK_Contents_WorkflowSteps_CurrentWorkflowStepId"
        FOREIGN KEY ("CurrentWorkflowStepId") REFERENCES "WorkflowSteps" ("Id") ON DELETE SET NULL;

ALTER TABLE "MediaAssets"
    ADD CONSTRAINT "FK_MediaAssets_Contents_ContentId"
        FOREIGN KEY ("ContentId") REFERENCES "Contents" ("Id") ON DELETE SET NULL;

-- ============================================================
-- Mark migration as applied in EF Core history table
-- ============================================================

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260520133335_InitialDatabase', '9.0.4');

COMMIT;
