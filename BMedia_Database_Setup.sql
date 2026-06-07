-- ============================================================
-- BMedia — Complete Database Setup (Clean Slate)
-- Combines both EF migrations into one idempotent script.
-- Run this on a FRESH (empty) PostgreSQL 16+ database.
--
-- After applying, EF Core will see both migrations as already
-- applied and will not try to run them again.
-- ============================================================

BEGIN;

-- ============================================================
-- 0. EF Migrations history table
-- ============================================================

CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId"    character varying(150) NOT NULL,
    "ProductVersion" character varying(32)  NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

-- ============================================================
-- 1. Roles
-- ============================================================

CREATE TABLE "Roles" (
    "Id"             uuid                     NOT NULL DEFAULT gen_random_uuid(),
    "Name"           character varying(100)   NOT NULL,
    "NormalizedName" character varying(100)   NOT NULL,
    "Description"    character varying(500),
    "IsSystem"       boolean                  NOT NULL,
    "CreatedAt"      timestamp with time zone NOT NULL,
    "CreatedBy"      uuid,
    "UpdatedAt"      timestamp with time zone,
    "UpdatedBy"      uuid,
    "IsDeleted"      boolean                  NOT NULL,
    "DeletedAt"      timestamp with time zone,
    "DeletedBy"      uuid,
    CONSTRAINT "PK_Roles" PRIMARY KEY ("Id")
);

CREATE UNIQUE INDEX "IX_Roles_NormalizedName"
    ON "Roles" ("NormalizedName")
    WHERE "IsDeleted" = false;

-- ============================================================
-- 2. Permissions
-- ============================================================

CREATE TABLE "Permissions" (
    "Id"             uuid                     NOT NULL DEFAULT gen_random_uuid(),
    "Name"           character varying(100)   NOT NULL,
    "NormalizedName" character varying(100)   NOT NULL,
    "Description"    character varying(500),
    "Module"         character varying(100)   NOT NULL,
    "CreatedAt"      timestamp with time zone NOT NULL,
    "CreatedBy"      uuid,
    "UpdatedAt"      timestamp with time zone,
    "UpdatedBy"      uuid,
    "IsDeleted"      boolean                  NOT NULL,
    "DeletedAt"      timestamp with time zone,
    "DeletedBy"      uuid,
    CONSTRAINT "PK_Permissions" PRIMARY KEY ("Id")
);

CREATE INDEX        "IX_Permissions_Module"         ON "Permissions" ("Module");
CREATE UNIQUE INDEX "IX_Permissions_NormalizedName" ON "Permissions" ("NormalizedName") WHERE "IsDeleted" = false;

-- ============================================================
-- 3. Users
-- ============================================================

CREATE TABLE "Users" (
    "Id"                          uuid                     NOT NULL DEFAULT gen_random_uuid(),
    "Username"                    character varying(100)   NOT NULL,
    "Email"                       character varying(256)   NOT NULL,
    "PasswordHash"                text                     NOT NULL,
    "FirstName"                   character varying(100)   NOT NULL,
    "LastName"                    character varying(100)   NOT NULL,
    "PhoneNumber"                 character varying(30),
    "ProfilePictureUrl"           character varying(2048),
    "IsActive"                    boolean                  NOT NULL DEFAULT true,
    "IsEmailVerified"             boolean                  NOT NULL DEFAULT false,
    "LastLoginAt"                 timestamp with time zone,
    "LastLoginIp"                 text,
    "FailedLoginAttempts"         integer                  NOT NULL DEFAULT 0,
    "LockoutEnd"                  timestamp with time zone,
    "EmailVerificationToken"      text,
    "EmailVerificationTokenExpiry" timestamp with time zone,
    "PasswordResetToken"          text,
    "PasswordResetTokenExpiry"    timestamp with time zone,
    "PreferredLanguage"           character varying(10)    NOT NULL DEFAULT 'en',
    "TimeZone"                    character varying(100),
    "CreatedAt"                   timestamp with time zone NOT NULL,
    "CreatedBy"                   uuid,
    "UpdatedAt"                   timestamp with time zone,
    "UpdatedBy"                   uuid,
    "IsDeleted"                   boolean                  NOT NULL,
    "DeletedAt"                   timestamp with time zone,
    "DeletedBy"                   uuid,
    CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
);

CREATE UNIQUE INDEX "IX_Users_Email"    ON "Users" ("Email")    WHERE "IsDeleted" = false;
CREATE UNIQUE INDEX "IX_Users_Username" ON "Users" ("Username") WHERE "IsDeleted" = false;
CREATE INDEX        "IX_Users_IsDeleted" ON "Users" ("IsDeleted");
CREATE INDEX        "IX_Users_IsActive"  ON "Users" ("IsActive");

-- ============================================================
-- 4. UserRoles
-- ============================================================

CREATE TABLE "UserRoles" (
    "UserId"     uuid                     NOT NULL,
    "RoleId"     uuid                     NOT NULL,
    "AssignedAt" timestamp with time zone NOT NULL,
    "AssignedBy" uuid,
    CONSTRAINT "PK_UserRoles" PRIMARY KEY ("UserId", "RoleId"),
    CONSTRAINT "FK_UserRoles_Users_UserId"
        FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_UserRoles_Roles_RoleId"
        FOREIGN KEY ("RoleId") REFERENCES "Roles" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_UserRoles_UserId" ON "UserRoles" ("UserId");
CREATE INDEX "IX_UserRoles_RoleId" ON "UserRoles" ("RoleId");

-- ============================================================
-- 5. RolePermissions
-- ============================================================

CREATE TABLE "RolePermissions" (
    "RoleId"       uuid                     NOT NULL,
    "PermissionId" uuid                     NOT NULL,
    "GrantedAt"    timestamp with time zone NOT NULL,
    "GrantedBy"    uuid,
    CONSTRAINT "PK_RolePermissions" PRIMARY KEY ("RoleId", "PermissionId"),
    CONSTRAINT "FK_RolePermissions_Roles_RoleId"
        FOREIGN KEY ("RoleId") REFERENCES "Roles" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_RolePermissions_Permissions_PermissionId"
        FOREIGN KEY ("PermissionId") REFERENCES "Permissions" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_RolePermissions_PermissionId" ON "RolePermissions" ("PermissionId");

-- ============================================================
-- 6. RefreshTokens
-- ============================================================

CREATE TABLE "RefreshTokens" (
    "Id"              uuid                     NOT NULL DEFAULT gen_random_uuid(),
    "UserId"          uuid                     NOT NULL,
    "Token"           character varying(512)   NOT NULL,
    "ExpiresAt"       timestamp with time zone NOT NULL,
    "IsRevoked"       boolean                  NOT NULL,
    "RevokedAt"       timestamp with time zone,
    "RevokedReason"   character varying(500),
    "ReplacedByToken" text,
    "CreatedByIp"     character varying(50),
    "RevokedByIp"     character varying(50),
    "CreatedAt"       timestamp with time zone NOT NULL,
    "CreatedBy"       uuid,
    "UpdatedAt"       timestamp with time zone,
    "UpdatedBy"       uuid,
    "IsDeleted"       boolean                  NOT NULL,
    "DeletedAt"       timestamp with time zone,
    "DeletedBy"       uuid,
    "RowVersion"      bigint                   NOT NULL,
    CONSTRAINT "PK_RefreshTokens" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_RefreshTokens_Users_UserId"
        FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE UNIQUE INDEX "IX_RefreshTokens_Token"     ON "RefreshTokens" ("Token");
CREATE INDEX        "IX_RefreshTokens_UserId"    ON "RefreshTokens" ("UserId");
CREATE INDEX        "IX_RefreshTokens_ExpiresAt" ON "RefreshTokens" ("ExpiresAt");

-- ============================================================
-- 7. Categories
-- ============================================================

CREATE TABLE "Categories" (
    "Id"          uuid                     NOT NULL DEFAULT gen_random_uuid(),
    "Name"        character varying(200)   NOT NULL,
    "Slug"        character varying(250)   NOT NULL,
    "Description" character varying(1000),
    "IconUrl"     character varying(2048),
    "SortOrder"   integer                  NOT NULL,
    "IsActive"    boolean                  NOT NULL,
    "CreatedAt"   timestamp with time zone NOT NULL,
    "CreatedBy"   uuid,
    "UpdatedAt"   timestamp with time zone,
    "UpdatedBy"   uuid,
    "IsDeleted"   boolean                  NOT NULL,
    "DeletedAt"   timestamp with time zone,
    "DeletedBy"   uuid,
    CONSTRAINT "PK_Categories" PRIMARY KEY ("Id")
);

CREATE UNIQUE INDEX "IX_Categories_Slug" ON "Categories" ("Slug") WHERE "IsDeleted" = false;

-- ============================================================
-- 8. Subcategories
-- ============================================================

CREATE TABLE "Subcategories" (
    "Id"          uuid                     NOT NULL DEFAULT gen_random_uuid(),
    "Name"        character varying(200)   NOT NULL,
    "Slug"        character varying(250)   NOT NULL,
    "Description" character varying(1000),
    "SortOrder"   integer                  NOT NULL,
    "IsActive"    boolean                  NOT NULL,
    "CategoryId"  uuid                     NOT NULL,
    "CreatedAt"   timestamp with time zone NOT NULL,
    "CreatedBy"   uuid,
    "UpdatedAt"   timestamp with time zone,
    "UpdatedBy"   uuid,
    "IsDeleted"   boolean                  NOT NULL,
    "DeletedAt"   timestamp with time zone,
    "DeletedBy"   uuid,
    CONSTRAINT "PK_Subcategories" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Subcategories_Categories_CategoryId"
        FOREIGN KEY ("CategoryId") REFERENCES "Categories" ("Id") ON DELETE CASCADE
);

CREATE UNIQUE INDEX "IX_Subcategories_Slug"       ON "Subcategories" ("Slug") WHERE "IsDeleted" = false;
CREATE INDEX        "IX_Subcategories_CategoryId" ON "Subcategories" ("CategoryId");

-- ============================================================
-- 9. Tags
-- ============================================================

CREATE TABLE "Tags" (
    "Id"           uuid                     NOT NULL DEFAULT gen_random_uuid(),
    "Name"         character varying(150)   NOT NULL,
    "Slug"         character varying(200)   NOT NULL,
    "Description"  character varying(500),
    "IsAiGenerated" boolean                 NOT NULL,
    "CreatedAt"    timestamp with time zone NOT NULL,
    "CreatedBy"    uuid,
    "UpdatedAt"    timestamp with time zone,
    "UpdatedBy"    uuid,
    "IsDeleted"    boolean                  NOT NULL,
    "DeletedAt"    timestamp with time zone,
    "DeletedBy"    uuid,
    CONSTRAINT "PK_Tags" PRIMARY KEY ("Id")
);

CREATE UNIQUE INDEX "IX_Tags_Slug" ON "Tags" ("Slug") WHERE "IsDeleted" = false;

-- ============================================================
-- 10. WorkflowDefinitions
-- ============================================================

CREATE TABLE "WorkflowDefinitions" (
    "Id"          uuid                     NOT NULL DEFAULT gen_random_uuid(),
    "Name"        character varying(200)   NOT NULL,
    "Description" character varying(1000),
    "IsDefault"   boolean                  NOT NULL,
    "IsActive"    boolean                  NOT NULL,
    "Version"     integer                  NOT NULL,
    "CreatedAt"   timestamp with time zone NOT NULL,
    "CreatedBy"   uuid,
    "UpdatedAt"   timestamp with time zone,
    "UpdatedBy"   uuid,
    "IsDeleted"   boolean                  NOT NULL,
    "DeletedAt"   timestamp with time zone,
    "DeletedBy"   uuid,
    CONSTRAINT "PK_WorkflowDefinitions" PRIMARY KEY ("Id")
);

CREATE INDEX "IX_WorkflowDefinitions_IsDefault" ON "WorkflowDefinitions" ("IsDefault");

-- ============================================================
-- 11. WorkflowSteps
-- ============================================================

CREATE TABLE "WorkflowSteps" (
    "Id"                   uuid                     NOT NULL DEFAULT gen_random_uuid(),
    "WorkflowDefinitionId" uuid                     NOT NULL,
    "Name"                 character varying(200)   NOT NULL,
    "Description"          character varying(500),
    "MapsToStatus"         integer                  NOT NULL,
    "Order"                integer                  NOT NULL,
    "IsInitial"            boolean                  NOT NULL,
    "IsFinal"              boolean                  NOT NULL,
    "RequiresReviewer"     boolean                  NOT NULL,
    "RequiredPermission"   character varying(200),
    "SlaHours"             integer,
    "CreatedAt"            timestamp with time zone NOT NULL,
    "CreatedBy"            uuid,
    "UpdatedAt"            timestamp with time zone,
    "UpdatedBy"            uuid,
    "IsDeleted"            boolean                  NOT NULL,
    "DeletedAt"            timestamp with time zone,
    "DeletedBy"            uuid,
    CONSTRAINT "PK_WorkflowSteps" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_WorkflowSteps_WorkflowDefinitions_WorkflowDefinitionId"
        FOREIGN KEY ("WorkflowDefinitionId") REFERENCES "WorkflowDefinitions" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_WorkflowSteps_WorkflowDefinitionId"
    ON "WorkflowSteps" ("WorkflowDefinitionId");
CREATE INDEX "IX_WorkflowSteps_WorkflowDefinitionId_Order"
    ON "WorkflowSteps" ("WorkflowDefinitionId", "Order");

-- ============================================================
-- 12. WorkflowTransitions
-- ============================================================

CREATE TABLE "WorkflowTransitions" (
    "Id"                   uuid                     NOT NULL DEFAULT gen_random_uuid(),
    "WorkflowDefinitionId" uuid                     NOT NULL,
    "FromStepId"           uuid                     NOT NULL,
    "ToStepId"             uuid                     NOT NULL,
    "ActionName"           character varying(200)   NOT NULL,
    "Description"          character varying(500),
    "RequiredPermission"   character varying(200),
    "RequiresComment"      boolean                  NOT NULL,
    "IsActive"             boolean                  NOT NULL,
    "CreatedAt"            timestamp with time zone NOT NULL,
    "CreatedBy"            uuid,
    "UpdatedAt"            timestamp with time zone,
    "UpdatedBy"            uuid,
    "IsDeleted"            boolean                  NOT NULL,
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

CREATE UNIQUE INDEX "IX_WorkflowTransitions_FromStepId_ToStepId"
    ON "WorkflowTransitions" ("FromStepId", "ToStepId");
CREATE INDEX "IX_WorkflowTransitions_ToStepId"
    ON "WorkflowTransitions" ("ToStepId");
CREATE INDEX "IX_WorkflowTransitions_WorkflowDefinitionId"
    ON "WorkflowTransitions" ("WorkflowDefinitionId");

-- ============================================================
-- 13. Contents
-- ============================================================

CREATE TABLE "Contents" (
    "Id"                    uuid                     NOT NULL DEFAULT gen_random_uuid(),
    "Title"                 character varying(500)   NOT NULL,
    "Slug"                  character varying(600)   NOT NULL,
    "Summary"               character varying(2000),
    "Body"                  text,
    "Language"              character varying(10)    NOT NULL DEFAULT 'en',
    "Status"                integer                  NOT NULL DEFAULT 1,
    "IsFeatured"            boolean                  NOT NULL,
    "AllowComments"         boolean                  NOT NULL,
    "ViewCount"             integer                  NOT NULL,
    "SeoTitle"              character varying(300),
    "SeoDescription"        character varying(500),
    "SeoKeywords"           character varying(500),
    "CanonicalUrl"          character varying(2048),
    "PublishedAt"           timestamp with time zone,
    "PublishedBy"           uuid,
    "ScheduledPublishAt"    timestamp with time zone,
    "ArchivedAt"            timestamp with time zone,
    "CategoryId"            uuid,
    "SubcategoryId"         uuid,
    "CurrentWorkflowStepId" uuid,
    "AssignedReviewerId"    uuid,
    "CreatedAt"             timestamp with time zone NOT NULL,
    "CreatedBy"             uuid,
    "UpdatedAt"             timestamp with time zone,
    "UpdatedBy"             uuid,
    "IsDeleted"             boolean                  NOT NULL,
    "DeletedAt"             timestamp with time zone,
    "DeletedBy"             uuid,
    CONSTRAINT "PK_Contents" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Contents_Categories_CategoryId"
        FOREIGN KEY ("CategoryId") REFERENCES "Categories" ("Id") ON DELETE SET NULL,
    CONSTRAINT "FK_Contents_Subcategories_SubcategoryId"
        FOREIGN KEY ("SubcategoryId") REFERENCES "Subcategories" ("Id") ON DELETE SET NULL,
    CONSTRAINT "FK_Contents_WorkflowSteps_CurrentWorkflowStepId"
        FOREIGN KEY ("CurrentWorkflowStepId") REFERENCES "WorkflowSteps" ("Id") ON DELETE SET NULL
);

CREATE UNIQUE INDEX "IX_Contents_Slug"       ON "Contents" ("Slug") WHERE "IsDeleted" = false;
CREATE INDEX        "IX_Contents_Status"     ON "Contents" ("Status");
CREATE INDEX        "IX_Contents_Language"   ON "Contents" ("Language");
CREATE INDEX        "IX_Contents_IsFeatured" ON "Contents" ("IsFeatured");
CREATE INDEX        "IX_Contents_PublishedAt" ON "Contents" ("PublishedAt");
CREATE INDEX        "IX_Contents_CategoryId" ON "Contents" ("CategoryId");
CREATE INDEX        "IX_Contents_CreatedAt"  ON "Contents" ("CreatedAt");
CREATE INDEX        "IX_Contents_Status_Language_IsDeleted"
    ON "Contents" ("Status", "Language", "IsDeleted");
CREATE INDEX        "IX_Contents_SubcategoryId"         ON "Contents" ("SubcategoryId");
CREATE INDEX        "IX_Contents_CurrentWorkflowStepId" ON "Contents" ("CurrentWorkflowStepId");

-- ============================================================
-- 14. ContentTags
-- ============================================================

CREATE TABLE "ContentTags" (
    "ContentId"    uuid                     NOT NULL,
    "TagId"        uuid                     NOT NULL,
    "TaggedAt"     timestamp with time zone NOT NULL,
    "IsAiGenerated" boolean                 NOT NULL,
    CONSTRAINT "PK_ContentTags" PRIMARY KEY ("ContentId", "TagId"),
    CONSTRAINT "FK_ContentTags_Contents_ContentId"
        FOREIGN KEY ("ContentId") REFERENCES "Contents" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_ContentTags_Tags_TagId"
        FOREIGN KEY ("TagId") REFERENCES "Tags" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_ContentTags_TagId" ON "ContentTags" ("TagId");

-- ============================================================
-- 15. MediaAssets
-- ============================================================

CREATE TABLE "MediaAssets" (
    "Id"                    uuid                     NOT NULL DEFAULT gen_random_uuid(),
    "OriginalFileName"      character varying(512)   NOT NULL,
    "StorageKey"            character varying(1024)  NOT NULL,
    "PublicUrl"             character varying(2048),
    "CdnUrl"                character varying(2048),
    "ContentType"           character varying(255)   NOT NULL,
    "FileSizeBytes"         bigint                   NOT NULL,
    "MediaType"             integer                  NOT NULL,
    "Status"                integer                  NOT NULL DEFAULT 1,
    "StorageProvider"       integer                  NOT NULL DEFAULT 1,
    "Title"                 character varying(500),
    "Description"           character varying(2000),
    "AltText"               character varying(500),
    "Language"              character varying(10)    NOT NULL DEFAULT 'en',
    "DurationSeconds"       integer,
    "Width"                 integer,
    "Height"                integer,
    "AspectRatio"           double precision,
    "Codec"                 character varying(100),
    "Bitrate"               integer,
    "FrameRate"             double precision,
    "ThumbnailUrl"          character varying(2048),
    "PreviewUrl"            character varying(2048),
    "HlsManifestUrl"        character varying(2048),
    "IsTranscodingComplete" boolean                  NOT NULL,
    "IsThumbnailGenerated"  boolean                  NOT NULL,
    "IsMetadataExtracted"   boolean                  NOT NULL,
    "AntivirusScanPassed"   boolean                  NOT NULL,
    "AntivirusScannedAt"    timestamp with time zone,
    "ExtractedMetadata"     jsonb,
    "AiGeneratedTags"       jsonb,
    "OcrText"               text,
    "IsWatermarked"         boolean                  NOT NULL,
    "SortOrder"             integer                  NOT NULL,
    "IsPrimary"             boolean                  NOT NULL,
    "ContentId"             uuid,
    "CreatedAt"             timestamp with time zone NOT NULL,
    "CreatedBy"             uuid,
    "UpdatedAt"             timestamp with time zone,
    "UpdatedBy"             uuid,
    "IsDeleted"             boolean                  NOT NULL,
    "DeletedAt"             timestamp with time zone,
    "DeletedBy"             uuid,
    CONSTRAINT "PK_MediaAssets" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_MediaAssets_Contents_ContentId"
        FOREIGN KEY ("ContentId") REFERENCES "Contents" ("Id") ON DELETE SET NULL
);

CREATE INDEX        "IX_MediaAssets_ContentId"          ON "MediaAssets" ("ContentId");
CREATE INDEX        "IX_MediaAssets_MediaType"          ON "MediaAssets" ("MediaType");
CREATE INDEX        "IX_MediaAssets_Status"             ON "MediaAssets" ("Status");
CREATE UNIQUE INDEX "IX_MediaAssets_StorageKey"         ON "MediaAssets" ("StorageKey") WHERE "IsDeleted" = false;
CREATE INDEX        "IX_MediaAssets_ContentId_IsPrimary" ON "MediaAssets" ("ContentId", "IsPrimary");

-- ============================================================
-- 16. MediaVersions
-- ============================================================

CREATE TABLE "MediaVersions" (
    "Id"              uuid                     NOT NULL DEFAULT gen_random_uuid(),
    "MediaAssetId"    uuid                     NOT NULL,
    "StorageKey"      character varying(1024)  NOT NULL,
    "PublicUrl"       character varying(2048),
    "CdnUrl"          character varying(2048),
    "ContentType"     character varying(255)   NOT NULL,
    "FileSizeBytes"   bigint                   NOT NULL,
    "Width"           integer,
    "Height"          integer,
    "Bitrate"         integer,
    "DurationSeconds" integer,
    "Quality"         integer,
    "VersionLabel"    character varying(100)   NOT NULL,
    "IsHls"           boolean                  NOT NULL,
    "HlsManifestUrl"  character varying(2048),
    "IsDefault"       boolean                  NOT NULL,
    "StorageProvider" integer                  NOT NULL DEFAULT 1,
    "CreatedAt"       timestamp with time zone NOT NULL,
    "CreatedBy"       uuid,
    "UpdatedAt"       timestamp with time zone,
    "UpdatedBy"       uuid,
    "IsDeleted"       boolean                  NOT NULL,
    "DeletedAt"       timestamp with time zone,
    "DeletedBy"       uuid,
    CONSTRAINT "PK_MediaVersions" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_MediaVersions_MediaAssets_MediaAssetId"
        FOREIGN KEY ("MediaAssetId") REFERENCES "MediaAssets" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_MediaVersions_MediaAssetId" ON "MediaVersions" ("MediaAssetId");
CREATE INDEX "IX_MediaVersions_Quality"      ON "MediaVersions" ("Quality");
CREATE INDEX "IX_MediaVersions_IsDefault"    ON "MediaVersions" ("IsDefault");

-- ============================================================
-- 17. Localizations
-- ============================================================

CREATE TABLE "Localizations" (
    "Id"             uuid                     NOT NULL DEFAULT gen_random_uuid(),
    "ContentId"      uuid                     NOT NULL,
    "Language"       character varying(10)    NOT NULL,
    "Title"          character varying(500)   NOT NULL,
    "Summary"        character varying(2000),
    "Body"           text,
    "SeoTitle"       character varying(300),
    "SeoDescription" character varying(500),
    "IsApproved"     boolean                  NOT NULL,
    "ApprovedAt"     timestamp with time zone,
    "ApprovedBy"     uuid,
    "CreatedAt"      timestamp with time zone NOT NULL,
    "CreatedBy"      uuid,
    "UpdatedAt"      timestamp with time zone,
    "UpdatedBy"      uuid,
    "IsDeleted"      boolean                  NOT NULL,
    "DeletedAt"      timestamp with time zone,
    "DeletedBy"      uuid,
    CONSTRAINT "PK_Localizations" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Localizations_Contents_ContentId"
        FOREIGN KEY ("ContentId") REFERENCES "Contents" ("Id") ON DELETE CASCADE
);

CREATE UNIQUE INDEX "IX_Localizations_ContentId_Language"
    ON "Localizations" ("ContentId", "Language")
    WHERE "IsDeleted" = false;

-- ============================================================
-- 18. Attachments
-- ============================================================

CREATE TABLE "Attachments" (
    "Id"            uuid                     NOT NULL DEFAULT gen_random_uuid(),
    "ContentId"     uuid                     NOT NULL,
    "FileName"      character varying(512)   NOT NULL,
    "StorageKey"    character varying(1024)  NOT NULL,
    "PublicUrl"     character varying(2048),
    "ContentType"   character varying(255)   NOT NULL,
    "FileSizeBytes" bigint                   NOT NULL,
    "Description"   character varying(1000),
    "SortOrder"     integer                  NOT NULL,
    "CreatedAt"     timestamp with time zone NOT NULL,
    "CreatedBy"     uuid,
    "UpdatedAt"     timestamp with time zone,
    "UpdatedBy"     uuid,
    "IsDeleted"     boolean                  NOT NULL,
    "DeletedAt"     timestamp with time zone,
    "DeletedBy"     uuid,
    CONSTRAINT "PK_Attachments" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Attachments_Contents_ContentId"
        FOREIGN KEY ("ContentId") REFERENCES "Contents" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Attachments_ContentId" ON "Attachments" ("ContentId");

-- ============================================================
-- 19. ScheduledPublications
-- ============================================================

CREATE TABLE "ScheduledPublications" (
    "Id"            uuid                     NOT NULL DEFAULT gen_random_uuid(),
    "ContentId"     uuid                     NOT NULL,
    "ScheduledAt"   timestamp with time zone NOT NULL,
    "IsExecuted"    boolean                  NOT NULL,
    "ExecutedAt"    timestamp with time zone,
    "IsSuccessful"  boolean                  NOT NULL,
    "ErrorMessage"  character varying(2000),
    "HangfireJobId" character varying(200),
    "CreatedAt"     timestamp with time zone NOT NULL,
    "CreatedBy"     uuid,
    "UpdatedAt"     timestamp with time zone,
    "UpdatedBy"     uuid,
    "IsDeleted"     boolean                  NOT NULL,
    "DeletedAt"     timestamp with time zone,
    "DeletedBy"     uuid,
    CONSTRAINT "PK_ScheduledPublications" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ScheduledPublications_Contents_ContentId"
        FOREIGN KEY ("ContentId") REFERENCES "Contents" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_ScheduledPublications_ContentId" ON "ScheduledPublications" ("ContentId");
CREATE INDEX "IX_ScheduledPublications_ScheduledAt_IsExecuted"
    ON "ScheduledPublications" ("ScheduledAt", "IsExecuted");

-- ============================================================
-- 20. ReviewComments
-- ============================================================

CREATE TABLE "ReviewComments" (
    "Id"              uuid                     NOT NULL DEFAULT gen_random_uuid(),
    "ContentId"       uuid                     NOT NULL,
    "ReviewerId"      uuid                     NOT NULL,
    "Comment"         character varying(5000)  NOT NULL,
    "IsResolved"      boolean                  NOT NULL,
    "ResolvedAt"      timestamp with time zone,
    "ResolvedBy"      uuid,
    "ParentCommentId" uuid,
    "CreatedAt"       timestamp with time zone NOT NULL,
    "CreatedBy"       uuid,
    "UpdatedAt"       timestamp with time zone,
    "UpdatedBy"       uuid,
    "IsDeleted"       boolean                  NOT NULL,
    "DeletedAt"       timestamp with time zone,
    "DeletedBy"       uuid,
    CONSTRAINT "PK_ReviewComments" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ReviewComments_Contents_ContentId"
        FOREIGN KEY ("ContentId") REFERENCES "Contents" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_ReviewComments_Users_ReviewerId"
        FOREIGN KEY ("ReviewerId") REFERENCES "Users" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_ReviewComments_ReviewComments_ParentCommentId"
        FOREIGN KEY ("ParentCommentId") REFERENCES "ReviewComments" ("Id") ON DELETE RESTRICT
);

CREATE INDEX "IX_ReviewComments_ContentId"       ON "ReviewComments" ("ContentId");
CREATE INDEX "IX_ReviewComments_ReviewerId"      ON "ReviewComments" ("ReviewerId");
CREATE INDEX "IX_ReviewComments_ParentCommentId" ON "ReviewComments" ("ParentCommentId");

-- ============================================================
-- 21. ContentWorkflowHistories
-- ============================================================

CREATE TABLE "ContentWorkflowHistories" (
    "Id"               uuid                     NOT NULL DEFAULT gen_random_uuid(),
    "ContentId"        uuid                     NOT NULL,
    "FromStepId"       uuid,
    "ToStepId"         uuid                     NOT NULL,
    "TransitionedById" uuid                     NOT NULL,
    "FromStatus"       integer                  NOT NULL,
    "ToStatus"         integer                  NOT NULL,
    "Comment"          character varying(2000),
    "ActionName"       character varying(200),
    "TransitionedAt"   timestamp with time zone NOT NULL,
    "CreatedAt"        timestamp with time zone NOT NULL,
    "CreatedBy"        uuid,
    "UpdatedAt"        timestamp with time zone,
    "UpdatedBy"        uuid,
    "IsDeleted"        boolean                  NOT NULL,
    "DeletedAt"        timestamp with time zone,
    "DeletedBy"        uuid,
    "RowVersion"       bigint                   NOT NULL,
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

CREATE INDEX "IX_ContentWorkflowHistories_ContentId"
    ON "ContentWorkflowHistories" ("ContentId");
CREATE INDEX "IX_ContentWorkflowHistories_TransitionedAt"
    ON "ContentWorkflowHistories" ("TransitionedAt");
CREATE INDEX "IX_ContentWorkflowHistories_FromStepId"
    ON "ContentWorkflowHistories" ("FromStepId");
CREATE INDEX "IX_ContentWorkflowHistories_ToStepId"
    ON "ContentWorkflowHistories" ("ToStepId");
CREATE INDEX "IX_ContentWorkflowHistories_TransitionedById"
    ON "ContentWorkflowHistories" ("TransitionedById");

-- ============================================================
-- 22. Notifications
-- ============================================================

CREATE TABLE "Notifications" (
    "Id"            uuid                     NOT NULL DEFAULT gen_random_uuid(),
    "UserId"        uuid                     NOT NULL,
    "Type"          integer                  NOT NULL,
    "Title"         character varying(300)   NOT NULL,
    "Message"       character varying(2000)  NOT NULL,
    "IsRead"        boolean                  NOT NULL,
    "ReadAt"        timestamp with time zone,
    "ReferenceId"   uuid,
    "ReferenceType" character varying(100),
    "ActionUrl"     character varying(2048),
    "Metadata"      jsonb,
    "CreatedAt"     timestamp with time zone NOT NULL,
    "CreatedBy"     uuid,
    "UpdatedAt"     timestamp with time zone,
    "UpdatedBy"     uuid,
    "IsDeleted"     boolean                  NOT NULL,
    "DeletedAt"     timestamp with time zone,
    "DeletedBy"     uuid,
    CONSTRAINT "PK_Notifications" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Notifications_Users_UserId"
        FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Notifications_UserId"        ON "Notifications" ("UserId");
CREATE INDEX "IX_Notifications_UserId_IsRead" ON "Notifications" ("UserId", "IsRead");
CREATE INDEX "IX_Notifications_CreatedAt"     ON "Notifications" ("CreatedAt");

-- ============================================================
-- 23. AuditLogs
-- ============================================================

CREATE TABLE "AuditLogs" (
    "Id"             uuid                     NOT NULL DEFAULT gen_random_uuid(),
    "UserId"         uuid,
    "UserEmail"      character varying(256),
    "Action"         integer                  NOT NULL,
    "EntityType"     character varying(200)   NOT NULL,
    "EntityId"       character varying(100),
    "OldValues"      jsonb,
    "NewValues"      jsonb,
    "IpAddress"      character varying(50),
    "UserAgent"      character varying(512),
    "AdditionalData" jsonb,
    "IsSuccessful"   boolean                  NOT NULL,
    "ErrorMessage"   character varying(2000),
    "CreatedAt"      timestamp with time zone NOT NULL,
    "CreatedBy"      uuid,
    "UpdatedAt"      timestamp with time zone,
    "UpdatedBy"      uuid,
    "IsDeleted"      boolean                  NOT NULL,
    "DeletedAt"      timestamp with time zone,
    "DeletedBy"      uuid,
    "RowVersion"     bigint                   NOT NULL,
    CONSTRAINT "PK_AuditLogs" PRIMARY KEY ("Id")
);

CREATE INDEX "IX_AuditLogs_UserId"              ON "AuditLogs" ("UserId");
CREATE INDEX "IX_AuditLogs_Action"              ON "AuditLogs" ("Action");
CREATE INDEX "IX_AuditLogs_EntityType"          ON "AuditLogs" ("EntityType");
CREATE INDEX "IX_AuditLogs_CreatedAt"           ON "AuditLogs" ("CreatedAt");
CREATE INDEX "IX_AuditLogs_EntityType_EntityId" ON "AuditLogs" ("EntityType", "EntityId");

-- ============================================================
-- 24. Seed data — 7 Roles
--     (timestamps taken from InitialDatabase UpdateData calls)
-- ============================================================

INSERT INTO "Roles" ("Id", "Name", "NormalizedName", "Description", "IsSystem", "CreatedAt", "IsDeleted")
VALUES
  ('10000000-0000-0000-0000-000000000001', 'Administrator',    'ADMINISTRATOR',    'Full system access',                    true,  '2026-05-20 13:33:35.539030+00', false),
  ('10000000-0000-0000-0000-000000000002', 'ContentCreator',   'CONTENTCREATOR',   'Create and edit content',               true,  '2026-05-20 13:33:35.539106+00', false),
  ('10000000-0000-0000-0000-000000000003', 'Reviewer',         'REVIEWER',         'Review and approve content',            true,  '2026-05-20 13:33:35.539107+00', false),
  ('10000000-0000-0000-0000-000000000004', 'LanguageReviewer', 'LANGUAGEREVIEWER', 'Review language and translations',      true,  '2026-05-20 13:33:35.539107+00', false),
  ('10000000-0000-0000-0000-000000000005', 'Designer',         'DESIGNER',         'Manage media assets',                   true,  '2026-05-20 13:33:35.539107+00', false),
  ('10000000-0000-0000-0000-000000000006', 'Publisher',        'PUBLISHER',        'Publish and schedule content',          true,  '2026-05-20 13:33:35.539107+00', false),
  ('10000000-0000-0000-0000-000000000007', 'Archivist',        'ARCHIVIST',        'Archive and restore content',           true,  '2026-05-20 13:33:35.539108+00', false);

-- ============================================================
-- 25. Seed data — 17 Permissions
--     (timestamps taken from InitialDatabase InsertData calls)
-- ============================================================

INSERT INTO "Permissions" ("Id", "Name", "NormalizedName", "Description", "Module", "CreatedAt", "IsDeleted")
VALUES
  ('20000000-0000-0000-0000-000000000001', 'CreateContent',      'CREATECONTENT',      'CreateContent permission',      'Content',  '2026-05-20 13:33:35.533352+00', false),
  ('20000000-0000-0000-0000-000000000002', 'EditContent',        'EDITCONTENT',        'EditContent permission',        'Content',  '2026-05-20 13:33:35.534274+00', false),
  ('20000000-0000-0000-0000-000000000003', 'DeleteContent',      'DELETECONTENT',      'DeleteContent permission',      'Content',  '2026-05-20 13:33:35.534283+00', false),
  ('20000000-0000-0000-0000-000000000004', 'PublishContent',     'PUBLISHCONTENT',     'PublishContent permission',     'Content',  '2026-05-20 13:33:35.534283+00', false),
  ('20000000-0000-0000-0000-000000000005', 'ArchiveContent',     'ARCHIVECONTENT',     'ArchiveContent permission',     'Content',  '2026-05-20 13:33:35.534284+00', false),
  ('20000000-0000-0000-0000-000000000006', 'ViewContent',        'VIEWCONTENT',        'ViewContent permission',        'Content',  '2026-05-20 13:33:35.534284+00', false),
  ('20000000-0000-0000-0000-000000000007', 'UploadMedia',        'UPLOADMEDIA',        'UploadMedia permission',        'Media',    '2026-05-20 13:33:35.534284+00', false),
  ('20000000-0000-0000-0000-000000000008', 'DeleteMedia',        'DELETEMEDIA',        'DeleteMedia permission',        'Media',    '2026-05-20 13:33:35.534285+00', false),
  ('20000000-0000-0000-0000-000000000009', 'ManageMedia',        'MANAGEMEDIA',        'ManageMedia permission',        'Media',    '2026-05-20 13:33:35.534286+00', false),
  ('20000000-0000-0000-0000-000000000010', 'ApproveReview',      'APPROVEREVIEW',      'ApproveReview permission',      'Workflow', '2026-05-20 13:33:35.534320+00', false),
  ('20000000-0000-0000-0000-000000000011', 'RejectReview',       'REJECTREVIEW',       'RejectReview permission',       'Workflow', '2026-05-20 13:33:35.534322+00', false),
  ('20000000-0000-0000-0000-000000000012', 'TransitionWorkflow', 'TRANSITIONWORKFLOW', 'TransitionWorkflow permission', 'Workflow', '2026-05-20 13:33:35.534322+00', false),
  ('20000000-0000-0000-0000-000000000013', 'ManageUsers',        'MANAGEUSERS',        'ManageUsers permission',        'Admin',    '2026-05-20 13:33:35.534322+00', false),
  ('20000000-0000-0000-0000-000000000014', 'ManageRoles',        'MANAGEROLES',        'ManageRoles permission',        'Admin',    '2026-05-20 13:33:35.534322+00', false),
  ('20000000-0000-0000-0000-000000000015', 'ViewAuditLogs',      'VIEWAUDITLOGS',      'ViewAuditLogs permission',      'Admin',    '2026-05-20 13:33:35.534323+00', false),
  ('20000000-0000-0000-0000-000000000016', 'ManageCategories',   'MANAGECATEGORIES',   'ManageCategories permission',   'Taxonomy', '2026-05-20 13:33:35.534323+00', false),
  ('20000000-0000-0000-0000-000000000017', 'ManageTags',         'MANAGETAGS',         'ManageTags permission',         'Taxonomy', '2026-05-20 13:33:35.534323+00', false);

-- ============================================================
-- 26. Mark both EF migrations as applied
-- ============================================================

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion") VALUES
  ('20260518000000_InitialCreate',  '9.0.4'),
  ('20260520133335_InitialDatabase', '9.0.4');

COMMIT;
