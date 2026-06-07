-- ============================================================
-- BMedia — Complete Database Setup (Clean Slate)
-- Schema matches EF Core UseSnakeCaseNamingConvention() output.
-- All table and column names are lowercase snake_case (unquoted).
-- Tables that use PostgreSQL xmin system column for concurrency
-- do NOT have a user-defined row_version column.
-- Only audit_logs, content_workflow_histories, and refresh_tokens
-- have an explicit row_version bigint column.
-- ============================================================

BEGIN;

-- ============================================================
-- Drop all existing BMedia tables (both naming styles).
-- Order matters: leaf tables first to avoid FK violations.
-- ============================================================

DROP TABLE IF EXISTS "AuditLogs"                CASCADE;
DROP TABLE IF EXISTS "Notifications"            CASCADE;
DROP TABLE IF EXISTS "ContentWorkflowHistories" CASCADE;
DROP TABLE IF EXISTS "ReviewComments"           CASCADE;
DROP TABLE IF EXISTS "ScheduledPublications"    CASCADE;
DROP TABLE IF EXISTS "Attachments"              CASCADE;
DROP TABLE IF EXISTS "Localizations"            CASCADE;
DROP TABLE IF EXISTS "MediaVersions"            CASCADE;
DROP TABLE IF EXISTS "ContentTags"              CASCADE;
DROP TABLE IF EXISTS "MediaAssets"              CASCADE;
DROP TABLE IF EXISTS "Contents"                 CASCADE;
DROP TABLE IF EXISTS "WorkflowTransitions"      CASCADE;
DROP TABLE IF EXISTS "WorkflowSteps"            CASCADE;
DROP TABLE IF EXISTS "WorkflowDefinitions"      CASCADE;
DROP TABLE IF EXISTS "Tags"                     CASCADE;
DROP TABLE IF EXISTS "Subcategories"            CASCADE;
DROP TABLE IF EXISTS "Categories"               CASCADE;
DROP TABLE IF EXISTS "RefreshTokens"            CASCADE;
DROP TABLE IF EXISTS "UserRoles"                CASCADE;
DROP TABLE IF EXISTS "RolePermissions"          CASCADE;
DROP TABLE IF EXISTS "Users"                    CASCADE;
DROP TABLE IF EXISTS "Roles"                    CASCADE;
DROP TABLE IF EXISTS "Permissions"              CASCADE;

DROP TABLE IF EXISTS audit_logs                  CASCADE;
DROP TABLE IF EXISTS content_workflow_histories  CASCADE;
DROP TABLE IF EXISTS review_comments             CASCADE;
DROP TABLE IF EXISTS notifications               CASCADE;
DROP TABLE IF EXISTS scheduled_publications      CASCADE;
DROP TABLE IF EXISTS attachments                 CASCADE;
DROP TABLE IF EXISTS localizations               CASCADE;
DROP TABLE IF EXISTS media_versions              CASCADE;
DROP TABLE IF EXISTS content_tags                CASCADE;
DROP TABLE IF EXISTS media_assets                CASCADE;
DROP TABLE IF EXISTS contents                    CASCADE;
DROP TABLE IF EXISTS workflow_transitions        CASCADE;
DROP TABLE IF EXISTS workflow_steps              CASCADE;
DROP TABLE IF EXISTS workflow_definitions        CASCADE;
DROP TABLE IF EXISTS tags                        CASCADE;
DROP TABLE IF EXISTS subcategories               CASCADE;
DROP TABLE IF EXISTS categories                  CASCADE;
DROP TABLE IF EXISTS refresh_tokens              CASCADE;
DROP TABLE IF EXISTS user_roles                  CASCADE;
DROP TABLE IF EXISTS role_permissions            CASCADE;
DROP TABLE IF EXISTS users                       CASCADE;
DROP TABLE IF EXISTS roles                       CASCADE;
DROP TABLE IF EXISTS permissions                 CASCADE;

-- Remove any previously recorded migrations
DELETE FROM "__EFMigrationsHistory"
WHERE "MigrationId" IN (
    '20260518000000_InitialCreate',
    '20260520133335_InitialDatabase',
    '20260520133336_InitialDatabase'
);

-- ============================================================
-- EF Migrations history table
-- ============================================================

CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId"    character varying(150) NOT NULL,
    "ProductVersion" character varying(32)  NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

-- ============================================================
-- 1. roles
-- ============================================================

CREATE TABLE roles (
    id              uuid                     NOT NULL DEFAULT gen_random_uuid(),
    name            character varying(100)   NOT NULL,
    normalized_name character varying(100)   NOT NULL,
    description     character varying(500),
    is_system       boolean                  NOT NULL,
    created_at      timestamp with time zone NOT NULL,
    created_by      uuid,
    updated_at      timestamp with time zone,
    updated_by      uuid,
    is_deleted      boolean                  NOT NULL,
    deleted_at      timestamp with time zone,
    deleted_by      uuid,
    CONSTRAINT pk_roles PRIMARY KEY (id)
);

CREATE UNIQUE INDEX ix_roles_normalized_name
    ON roles (normalized_name)
    WHERE is_deleted = false;

-- ============================================================
-- 2. permissions
-- ============================================================

CREATE TABLE permissions (
    id              uuid                     NOT NULL DEFAULT gen_random_uuid(),
    name            character varying(100)   NOT NULL,
    normalized_name character varying(100)   NOT NULL,
    description     character varying(500),
    module          character varying(100)   NOT NULL,
    created_at      timestamp with time zone NOT NULL,
    created_by      uuid,
    updated_at      timestamp with time zone,
    updated_by      uuid,
    is_deleted      boolean                  NOT NULL,
    deleted_at      timestamp with time zone,
    deleted_by      uuid,
    CONSTRAINT pk_permissions PRIMARY KEY (id)
);

CREATE INDEX        ix_permissions_module          ON permissions (module);
CREATE UNIQUE INDEX ix_permissions_normalized_name ON permissions (normalized_name) WHERE is_deleted = false;

-- ============================================================
-- 3. users
-- ============================================================

CREATE TABLE users (
    id                              uuid                     NOT NULL DEFAULT gen_random_uuid(),
    username                        character varying(100)   NOT NULL,
    email                           character varying(256)   NOT NULL,
    password_hash                   text                     NOT NULL,
    first_name                      character varying(100)   NOT NULL,
    last_name                       character varying(100)   NOT NULL,
    phone_number                    character varying(30),
    profile_picture_url             character varying(2048),
    is_active                       boolean                  NOT NULL DEFAULT true,
    is_email_verified               boolean                  NOT NULL DEFAULT false,
    last_login_at                   timestamp with time zone,
    last_login_ip                   text,
    failed_login_attempts           integer                  NOT NULL DEFAULT 0,
    lockout_end                     timestamp with time zone,
    email_verification_token        text,
    email_verification_token_expiry timestamp with time zone,
    password_reset_token            text,
    password_reset_token_expiry     timestamp with time zone,
    preferred_language              character varying(10)    NOT NULL DEFAULT 'en',
    time_zone                       character varying(100),
    created_at                      timestamp with time zone NOT NULL,
    created_by                      uuid,
    updated_at                      timestamp with time zone,
    updated_by                      uuid,
    is_deleted                      boolean                  NOT NULL,
    deleted_at                      timestamp with time zone,
    deleted_by                      uuid,
    CONSTRAINT pk_users PRIMARY KEY (id)
);

CREATE UNIQUE INDEX ix_users_email      ON users (email)      WHERE is_deleted = false;
CREATE UNIQUE INDEX ix_users_username   ON users (username)   WHERE is_deleted = false;
CREATE INDEX        ix_users_is_deleted ON users (is_deleted);
CREATE INDEX        ix_users_is_active  ON users (is_active);

-- ============================================================
-- 4. user_roles
-- ============================================================

CREATE TABLE user_roles (
    user_id     uuid                     NOT NULL,
    role_id     uuid                     NOT NULL,
    assigned_at timestamp with time zone NOT NULL,
    assigned_by uuid,
    CONSTRAINT pk_user_roles PRIMARY KEY (user_id, role_id),
    CONSTRAINT fk_user_roles_users_user_id
        FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE CASCADE,
    CONSTRAINT fk_user_roles_roles_role_id
        FOREIGN KEY (role_id) REFERENCES roles (id) ON DELETE CASCADE
);

CREATE INDEX ix_user_roles_role_id ON user_roles (role_id);
CREATE INDEX ix_user_roles_user_id ON user_roles (user_id);

-- ============================================================
-- 5. role_permissions
-- ============================================================

CREATE TABLE role_permissions (
    role_id       uuid                     NOT NULL,
    permission_id uuid                     NOT NULL,
    granted_at    timestamp with time zone NOT NULL,
    granted_by    uuid,
    CONSTRAINT pk_role_permissions PRIMARY KEY (role_id, permission_id),
    CONSTRAINT fk_role_permissions_roles_role_id
        FOREIGN KEY (role_id) REFERENCES roles (id) ON DELETE CASCADE,
    CONSTRAINT fk_role_permissions_permissions_permission_id
        FOREIGN KEY (permission_id) REFERENCES permissions (id) ON DELETE CASCADE
);

CREATE INDEX ix_role_permissions_permission_id ON role_permissions (permission_id);

-- ============================================================
-- 6. refresh_tokens  (uses bigint row_version, not xmin)
-- ============================================================

CREATE TABLE refresh_tokens (
    id                uuid                     NOT NULL DEFAULT gen_random_uuid(),
    user_id           uuid                     NOT NULL,
    token             character varying(512)   NOT NULL,
    expires_at        timestamp with time zone NOT NULL,
    is_revoked        boolean                  NOT NULL,
    revoked_at        timestamp with time zone,
    revoked_reason    character varying(500),
    replaced_by_token text,
    created_by_ip     character varying(50),
    revoked_by_ip     character varying(50),
    created_at        timestamp with time zone NOT NULL,
    created_by        uuid,
    updated_at        timestamp with time zone,
    updated_by        uuid,
    is_deleted        boolean                  NOT NULL,
    deleted_at        timestamp with time zone,
    deleted_by        uuid,
    row_version       bigint                   NOT NULL DEFAULT 0,
    CONSTRAINT pk_refresh_tokens PRIMARY KEY (id),
    CONSTRAINT fk_refresh_tokens_users_user_id
        FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE CASCADE
);

CREATE UNIQUE INDEX ix_refresh_tokens_token      ON refresh_tokens (token);
CREATE INDEX        ix_refresh_tokens_user_id    ON refresh_tokens (user_id);
CREATE INDEX        ix_refresh_tokens_expires_at ON refresh_tokens (expires_at);

-- ============================================================
-- 7. categories
-- ============================================================

CREATE TABLE categories (
    id          uuid                     NOT NULL DEFAULT gen_random_uuid(),
    name        character varying(200)   NOT NULL,
    slug        character varying(250)   NOT NULL,
    description character varying(1000),
    icon_url    character varying(2048),
    sort_order  integer                  NOT NULL,
    is_active   boolean                  NOT NULL,
    created_at  timestamp with time zone NOT NULL,
    created_by  uuid,
    updated_at  timestamp with time zone,
    updated_by  uuid,
    is_deleted  boolean                  NOT NULL,
    deleted_at  timestamp with time zone,
    deleted_by  uuid,
    CONSTRAINT pk_categories PRIMARY KEY (id)
);

CREATE UNIQUE INDEX ix_categories_slug ON categories (slug) WHERE is_deleted = false;

-- ============================================================
-- 8. subcategories
-- ============================================================

CREATE TABLE subcategories (
    id          uuid                     NOT NULL DEFAULT gen_random_uuid(),
    name        character varying(200)   NOT NULL,
    slug        character varying(250)   NOT NULL,
    description character varying(1000),
    sort_order  integer                  NOT NULL,
    is_active   boolean                  NOT NULL,
    category_id uuid                     NOT NULL,
    created_at  timestamp with time zone NOT NULL,
    created_by  uuid,
    updated_at  timestamp with time zone,
    updated_by  uuid,
    is_deleted  boolean                  NOT NULL,
    deleted_at  timestamp with time zone,
    deleted_by  uuid,
    CONSTRAINT pk_subcategories PRIMARY KEY (id),
    CONSTRAINT fk_subcategories_categories_category_id
        FOREIGN KEY (category_id) REFERENCES categories (id) ON DELETE CASCADE
);

CREATE UNIQUE INDEX ix_subcategories_slug        ON subcategories (slug) WHERE is_deleted = false;
CREATE INDEX        ix_subcategories_category_id ON subcategories (category_id);

-- ============================================================
-- 9. tags
-- ============================================================

CREATE TABLE tags (
    id              uuid                     NOT NULL DEFAULT gen_random_uuid(),
    name            character varying(150)   NOT NULL,
    slug            character varying(200)   NOT NULL,
    description     character varying(500),
    is_ai_generated boolean                  NOT NULL,
    created_at      timestamp with time zone NOT NULL,
    created_by      uuid,
    updated_at      timestamp with time zone,
    updated_by      uuid,
    is_deleted      boolean                  NOT NULL,
    deleted_at      timestamp with time zone,
    deleted_by      uuid,
    CONSTRAINT pk_tags PRIMARY KEY (id)
);

CREATE UNIQUE INDEX ix_tags_slug ON tags (slug) WHERE is_deleted = false;

-- ============================================================
-- 10. workflow_definitions
-- ============================================================

CREATE TABLE workflow_definitions (
    id          uuid                     NOT NULL DEFAULT gen_random_uuid(),
    name        character varying(200)   NOT NULL,
    description character varying(1000),
    is_default  boolean                  NOT NULL,
    is_active   boolean                  NOT NULL,
    version     integer                  NOT NULL,
    created_at  timestamp with time zone NOT NULL,
    created_by  uuid,
    updated_at  timestamp with time zone,
    updated_by  uuid,
    is_deleted  boolean                  NOT NULL,
    deleted_at  timestamp with time zone,
    deleted_by  uuid,
    CONSTRAINT pk_workflow_definitions PRIMARY KEY (id)
);

CREATE INDEX ix_workflow_definitions_is_default ON workflow_definitions (is_default);

-- ============================================================
-- 11. workflow_steps
-- ("order" is quoted because ORDER is a SQL reserved word)
-- ============================================================

CREATE TABLE workflow_steps (
    id                     uuid                     NOT NULL DEFAULT gen_random_uuid(),
    workflow_definition_id uuid                     NOT NULL,
    name                   character varying(200)   NOT NULL,
    description            character varying(500),
    maps_to_status         integer                  NOT NULL,
    "order"                integer                  NOT NULL,
    is_initial             boolean                  NOT NULL,
    is_final               boolean                  NOT NULL,
    requires_reviewer      boolean                  NOT NULL,
    required_permission    character varying(200),
    sla_hours              integer,
    created_at             timestamp with time zone NOT NULL,
    created_by             uuid,
    updated_at             timestamp with time zone,
    updated_by             uuid,
    is_deleted             boolean                  NOT NULL,
    deleted_at             timestamp with time zone,
    deleted_by             uuid,
    CONSTRAINT pk_workflow_steps PRIMARY KEY (id),
    CONSTRAINT fk_workflow_steps_workflow_definitions_workflow_definition_id
        FOREIGN KEY (workflow_definition_id) REFERENCES workflow_definitions (id) ON DELETE CASCADE
);

CREATE INDEX ix_workflow_steps_workflow_definition_id
    ON workflow_steps (workflow_definition_id);
CREATE INDEX ix_workflow_steps_workflow_definition_id_order
    ON workflow_steps (workflow_definition_id, "order");

-- ============================================================
-- 12. workflow_transitions
-- ============================================================

CREATE TABLE workflow_transitions (
    id                     uuid                     NOT NULL DEFAULT gen_random_uuid(),
    workflow_definition_id uuid                     NOT NULL,
    from_step_id           uuid                     NOT NULL,
    to_step_id             uuid                     NOT NULL,
    action_name            character varying(200)   NOT NULL,
    description            character varying(500),
    required_permission    character varying(200),
    requires_comment       boolean                  NOT NULL,
    is_active              boolean                  NOT NULL,
    created_at             timestamp with time zone NOT NULL,
    created_by             uuid,
    updated_at             timestamp with time zone,
    updated_by             uuid,
    is_deleted             boolean                  NOT NULL,
    deleted_at             timestamp with time zone,
    deleted_by             uuid,
    CONSTRAINT pk_workflow_transitions PRIMARY KEY (id),
    CONSTRAINT fk_workflow_transitions_workflow_definitions_workflow_definition_id
        FOREIGN KEY (workflow_definition_id) REFERENCES workflow_definitions (id) ON DELETE CASCADE,
    CONSTRAINT fk_workflow_transitions_workflow_steps_from_step_id
        FOREIGN KEY (from_step_id) REFERENCES workflow_steps (id) ON DELETE RESTRICT,
    CONSTRAINT fk_workflow_transitions_workflow_steps_to_step_id
        FOREIGN KEY (to_step_id) REFERENCES workflow_steps (id) ON DELETE RESTRICT
);

CREATE UNIQUE INDEX ix_workflow_transitions_from_step_id_to_step_id
    ON workflow_transitions (from_step_id, to_step_id);
CREATE INDEX ix_workflow_transitions_to_step_id
    ON workflow_transitions (to_step_id);
CREATE INDEX ix_workflow_transitions_workflow_definition_id
    ON workflow_transitions (workflow_definition_id);

-- ============================================================
-- 13. contents
-- ============================================================

CREATE TABLE contents (
    id                       uuid                     NOT NULL DEFAULT gen_random_uuid(),
    title                    character varying(500)   NOT NULL,
    slug                     character varying(600)   NOT NULL,
    summary                  character varying(2000),
    body                     text,
    language                 character varying(10)    NOT NULL DEFAULT 'en',
    status                   integer                  NOT NULL DEFAULT 1,
    is_featured              boolean                  NOT NULL,
    allow_comments           boolean                  NOT NULL,
    view_count               integer                  NOT NULL,
    seo_title                character varying(300),
    seo_description          character varying(500),
    seo_keywords             character varying(500),
    canonical_url            character varying(2048),
    published_at             timestamp with time zone,
    published_by             uuid,
    scheduled_publish_at     timestamp with time zone,
    archived_at              timestamp with time zone,
    category_id              uuid,
    subcategory_id           uuid,
    current_workflow_step_id uuid,
    assigned_reviewer_id     uuid,
    created_at               timestamp with time zone NOT NULL,
    created_by               uuid,
    updated_at               timestamp with time zone,
    updated_by               uuid,
    is_deleted               boolean                  NOT NULL,
    deleted_at               timestamp with time zone,
    deleted_by               uuid,
    CONSTRAINT pk_contents PRIMARY KEY (id),
    CONSTRAINT fk_contents_categories_category_id
        FOREIGN KEY (category_id) REFERENCES categories (id) ON DELETE SET NULL,
    CONSTRAINT fk_contents_subcategories_subcategory_id
        FOREIGN KEY (subcategory_id) REFERENCES subcategories (id) ON DELETE SET NULL,
    CONSTRAINT fk_contents_workflow_steps_current_workflow_step_id
        FOREIGN KEY (current_workflow_step_id) REFERENCES workflow_steps (id) ON DELETE SET NULL
);

CREATE UNIQUE INDEX ix_contents_slug
    ON contents (slug) WHERE is_deleted = false;
CREATE INDEX ix_contents_status               ON contents (status);
CREATE INDEX ix_contents_language             ON contents (language);
CREATE INDEX ix_contents_is_featured          ON contents (is_featured);
CREATE INDEX ix_contents_published_at         ON contents (published_at);
CREATE INDEX ix_contents_category_id          ON contents (category_id);
CREATE INDEX ix_contents_created_at           ON contents (created_at);
CREATE INDEX ix_contents_subcategory_id       ON contents (subcategory_id);
CREATE INDEX ix_contents_current_workflow_step_id ON contents (current_workflow_step_id);
CREATE INDEX ix_contents_status_language_is_deleted ON contents (status, language, is_deleted);

-- ============================================================
-- 14. content_tags
-- ============================================================

CREATE TABLE content_tags (
    content_id      uuid                     NOT NULL,
    tag_id          uuid                     NOT NULL,
    tagged_at       timestamp with time zone NOT NULL,
    is_ai_generated boolean                  NOT NULL,
    CONSTRAINT pk_content_tags PRIMARY KEY (content_id, tag_id),
    CONSTRAINT fk_content_tags_contents_content_id
        FOREIGN KEY (content_id) REFERENCES contents (id) ON DELETE CASCADE,
    CONSTRAINT fk_content_tags_tags_tag_id
        FOREIGN KEY (tag_id) REFERENCES tags (id) ON DELETE CASCADE
);

CREATE INDEX ix_content_tags_tag_id ON content_tags (tag_id);

-- ============================================================
-- 15. media_assets
-- ============================================================

CREATE TABLE media_assets (
    id                      uuid                     NOT NULL DEFAULT gen_random_uuid(),
    original_file_name      character varying(512)   NOT NULL,
    storage_key             character varying(1024)  NOT NULL,
    public_url              character varying(2048),
    cdn_url                 character varying(2048),
    content_type            character varying(255)   NOT NULL,
    file_size_bytes         bigint                   NOT NULL,
    media_type              integer                  NOT NULL,
    status                  integer                  NOT NULL DEFAULT 1,
    storage_provider        integer                  NOT NULL DEFAULT 1,
    title                   character varying(500),
    description             character varying(2000),
    alt_text                character varying(500),
    language                character varying(10)    NOT NULL DEFAULT 'en',
    duration_seconds        integer,
    width                   integer,
    height                  integer,
    aspect_ratio            double precision,
    codec                   character varying(100),
    bitrate                 integer,
    frame_rate              double precision,
    thumbnail_url           character varying(2048),
    preview_url             character varying(2048),
    hls_manifest_url        character varying(2048),
    is_transcoding_complete boolean                  NOT NULL,
    is_thumbnail_generated  boolean                  NOT NULL,
    is_metadata_extracted   boolean                  NOT NULL,
    antivirus_scan_passed   boolean                  NOT NULL,
    antivirus_scanned_at    timestamp with time zone,
    extracted_metadata      jsonb,
    ai_generated_tags       jsonb,
    ocr_text                text,
    is_watermarked          boolean                  NOT NULL,
    sort_order              integer                  NOT NULL,
    is_primary              boolean                  NOT NULL,
    content_id              uuid,
    created_at              timestamp with time zone NOT NULL,
    created_by              uuid,
    updated_at              timestamp with time zone,
    updated_by              uuid,
    is_deleted              boolean                  NOT NULL,
    deleted_at              timestamp with time zone,
    deleted_by              uuid,
    CONSTRAINT pk_media_assets PRIMARY KEY (id),
    CONSTRAINT fk_media_assets_contents_content_id
        FOREIGN KEY (content_id) REFERENCES contents (id) ON DELETE SET NULL
);

CREATE INDEX        ix_media_assets_content_id            ON media_assets (content_id);
CREATE INDEX        ix_media_assets_media_type            ON media_assets (media_type);
CREATE INDEX        ix_media_assets_status                ON media_assets (status);
CREATE UNIQUE INDEX ix_media_assets_storage_key           ON media_assets (storage_key) WHERE is_deleted = false;
CREATE INDEX        ix_media_assets_content_id_is_primary ON media_assets (content_id, is_primary);

-- ============================================================
-- 16. media_versions
-- ============================================================

CREATE TABLE media_versions (
    id               uuid                     NOT NULL DEFAULT gen_random_uuid(),
    media_asset_id   uuid                     NOT NULL,
    storage_key      character varying(1024)  NOT NULL,
    public_url       character varying(2048),
    cdn_url          character varying(2048),
    content_type     character varying(255)   NOT NULL,
    file_size_bytes  bigint                   NOT NULL,
    width            integer,
    height           integer,
    bitrate          integer,
    duration_seconds integer,
    quality          integer,
    version_label    character varying(100)   NOT NULL,
    is_hls           boolean                  NOT NULL,
    hls_manifest_url character varying(2048),
    is_default       boolean                  NOT NULL,
    storage_provider integer                  NOT NULL DEFAULT 1,
    created_at       timestamp with time zone NOT NULL,
    created_by       uuid,
    updated_at       timestamp with time zone,
    updated_by       uuid,
    is_deleted       boolean                  NOT NULL,
    deleted_at       timestamp with time zone,
    deleted_by       uuid,
    CONSTRAINT pk_media_versions PRIMARY KEY (id),
    CONSTRAINT fk_media_versions_media_assets_media_asset_id
        FOREIGN KEY (media_asset_id) REFERENCES media_assets (id) ON DELETE CASCADE
);

CREATE INDEX ix_media_versions_media_asset_id ON media_versions (media_asset_id);
CREATE INDEX ix_media_versions_quality        ON media_versions (quality);
CREATE INDEX ix_media_versions_is_default     ON media_versions (is_default);

-- ============================================================
-- 17. localizations
-- ============================================================

CREATE TABLE localizations (
    id              uuid                     NOT NULL DEFAULT gen_random_uuid(),
    content_id      uuid                     NOT NULL,
    language        character varying(10)    NOT NULL,
    title           character varying(500)   NOT NULL,
    summary         character varying(2000),
    body            text,
    seo_title       character varying(300),
    seo_description character varying(500),
    is_approved     boolean                  NOT NULL,
    approved_at     timestamp with time zone,
    approved_by     uuid,
    created_at      timestamp with time zone NOT NULL,
    created_by      uuid,
    updated_at      timestamp with time zone,
    updated_by      uuid,
    is_deleted      boolean                  NOT NULL,
    deleted_at      timestamp with time zone,
    deleted_by      uuid,
    CONSTRAINT pk_localizations PRIMARY KEY (id),
    CONSTRAINT fk_localizations_contents_content_id
        FOREIGN KEY (content_id) REFERENCES contents (id) ON DELETE CASCADE
);

CREATE UNIQUE INDEX ix_localizations_content_id_language
    ON localizations (content_id, language)
    WHERE is_deleted = false;

-- ============================================================
-- 18. attachments
-- ============================================================

CREATE TABLE attachments (
    id              uuid                     NOT NULL DEFAULT gen_random_uuid(),
    content_id      uuid                     NOT NULL,
    file_name       character varying(512)   NOT NULL,
    storage_key     character varying(1024)  NOT NULL,
    public_url      character varying(2048),
    content_type    character varying(255)   NOT NULL,
    file_size_bytes bigint                   NOT NULL,
    description     character varying(1000),
    sort_order      integer                  NOT NULL,
    created_at      timestamp with time zone NOT NULL,
    created_by      uuid,
    updated_at      timestamp with time zone,
    updated_by      uuid,
    is_deleted      boolean                  NOT NULL,
    deleted_at      timestamp with time zone,
    deleted_by      uuid,
    CONSTRAINT pk_attachments PRIMARY KEY (id),
    CONSTRAINT fk_attachments_contents_content_id
        FOREIGN KEY (content_id) REFERENCES contents (id) ON DELETE CASCADE
);

CREATE INDEX ix_attachments_content_id ON attachments (content_id);

-- ============================================================
-- 19. scheduled_publications
-- ============================================================

CREATE TABLE scheduled_publications (
    id              uuid                     NOT NULL DEFAULT gen_random_uuid(),
    content_id      uuid                     NOT NULL,
    scheduled_at    timestamp with time zone NOT NULL,
    is_executed     boolean                  NOT NULL,
    executed_at     timestamp with time zone,
    is_successful   boolean                  NOT NULL,
    error_message   character varying(2000),
    hangfire_job_id character varying(200),
    created_at      timestamp with time zone NOT NULL,
    created_by      uuid,
    updated_at      timestamp with time zone,
    updated_by      uuid,
    is_deleted      boolean                  NOT NULL,
    deleted_at      timestamp with time zone,
    deleted_by      uuid,
    CONSTRAINT pk_scheduled_publications PRIMARY KEY (id),
    CONSTRAINT fk_scheduled_publications_contents_content_id
        FOREIGN KEY (content_id) REFERENCES contents (id) ON DELETE CASCADE
);

CREATE INDEX ix_scheduled_publications_content_id ON scheduled_publications (content_id);
CREATE INDEX ix_scheduled_publications_scheduled_at_is_executed
    ON scheduled_publications (scheduled_at, is_executed);

-- ============================================================
-- 20. review_comments
-- ============================================================

CREATE TABLE review_comments (
    id                uuid                     NOT NULL DEFAULT gen_random_uuid(),
    content_id        uuid                     NOT NULL,
    reviewer_id       uuid                     NOT NULL,
    comment           character varying(5000)  NOT NULL,
    is_resolved       boolean                  NOT NULL,
    resolved_at       timestamp with time zone,
    resolved_by       uuid,
    parent_comment_id uuid,
    created_at        timestamp with time zone NOT NULL,
    created_by        uuid,
    updated_at        timestamp with time zone,
    updated_by        uuid,
    is_deleted        boolean                  NOT NULL,
    deleted_at        timestamp with time zone,
    deleted_by        uuid,
    CONSTRAINT pk_review_comments PRIMARY KEY (id),
    CONSTRAINT fk_review_comments_contents_content_id
        FOREIGN KEY (content_id) REFERENCES contents (id) ON DELETE CASCADE,
    CONSTRAINT fk_review_comments_users_reviewer_id
        FOREIGN KEY (reviewer_id) REFERENCES users (id) ON DELETE RESTRICT,
    CONSTRAINT fk_review_comments_review_comments_parent_comment_id
        FOREIGN KEY (parent_comment_id) REFERENCES review_comments (id) ON DELETE RESTRICT
);

CREATE INDEX ix_review_comments_content_id        ON review_comments (content_id);
CREATE INDEX ix_review_comments_reviewer_id       ON review_comments (reviewer_id);
CREATE INDEX ix_review_comments_parent_comment_id ON review_comments (parent_comment_id);

-- ============================================================
-- 21. content_workflow_histories  (uses bigint row_version, not xmin)
-- ============================================================

CREATE TABLE content_workflow_histories (
    id                  uuid                     NOT NULL DEFAULT gen_random_uuid(),
    content_id          uuid                     NOT NULL,
    from_step_id        uuid,
    to_step_id          uuid                     NOT NULL,
    transitioned_by_id  uuid                     NOT NULL,
    from_status         integer                  NOT NULL,
    to_status           integer                  NOT NULL,
    comment             character varying(2000),
    action_name         character varying(200),
    transitioned_at     timestamp with time zone NOT NULL,
    created_at          timestamp with time zone NOT NULL,
    created_by          uuid,
    updated_at          timestamp with time zone,
    updated_by          uuid,
    is_deleted          boolean                  NOT NULL,
    deleted_at          timestamp with time zone,
    deleted_by          uuid,
    row_version         bigint                   NOT NULL DEFAULT 0,
    CONSTRAINT pk_content_workflow_histories PRIMARY KEY (id),
    CONSTRAINT fk_content_workflow_histories_contents_content_id
        FOREIGN KEY (content_id) REFERENCES contents (id) ON DELETE CASCADE,
    CONSTRAINT fk_content_workflow_histories_users_transitioned_by_id
        FOREIGN KEY (transitioned_by_id) REFERENCES users (id) ON DELETE RESTRICT,
    CONSTRAINT fk_content_workflow_histories_workflow_steps_from_step_id
        FOREIGN KEY (from_step_id) REFERENCES workflow_steps (id) ON DELETE SET NULL,
    CONSTRAINT fk_content_workflow_histories_workflow_steps_to_step_id
        FOREIGN KEY (to_step_id) REFERENCES workflow_steps (id) ON DELETE RESTRICT
);

CREATE INDEX ix_content_workflow_histories_content_id
    ON content_workflow_histories (content_id);
CREATE INDEX ix_content_workflow_histories_transitioned_at
    ON content_workflow_histories (transitioned_at);
CREATE INDEX ix_content_workflow_histories_from_step_id
    ON content_workflow_histories (from_step_id);
CREATE INDEX ix_content_workflow_histories_to_step_id
    ON content_workflow_histories (to_step_id);
CREATE INDEX ix_content_workflow_histories_transitioned_by_id
    ON content_workflow_histories (transitioned_by_id);

-- ============================================================
-- 22. notifications
-- ============================================================

CREATE TABLE notifications (
    id             uuid                     NOT NULL DEFAULT gen_random_uuid(),
    user_id        uuid                     NOT NULL,
    type           integer                  NOT NULL,
    title          character varying(300)   NOT NULL,
    message        character varying(2000)  NOT NULL,
    is_read        boolean                  NOT NULL,
    read_at        timestamp with time zone,
    reference_id   uuid,
    reference_type character varying(100),
    action_url     character varying(2048),
    metadata       jsonb,
    created_at     timestamp with time zone NOT NULL,
    created_by     uuid,
    updated_at     timestamp with time zone,
    updated_by     uuid,
    is_deleted     boolean                  NOT NULL,
    deleted_at     timestamp with time zone,
    deleted_by     uuid,
    CONSTRAINT pk_notifications PRIMARY KEY (id),
    CONSTRAINT fk_notifications_users_user_id
        FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE CASCADE
);

CREATE INDEX ix_notifications_user_id         ON notifications (user_id);
CREATE INDEX ix_notifications_user_id_is_read ON notifications (user_id, is_read);
CREATE INDEX ix_notifications_created_at      ON notifications (created_at);

-- ============================================================
-- 23. audit_logs  (uses bigint row_version, not xmin)
-- ============================================================

CREATE TABLE audit_logs (
    id              uuid                     NOT NULL DEFAULT gen_random_uuid(),
    user_id         uuid,
    user_email      character varying(256),
    action          integer                  NOT NULL,
    entity_type     character varying(200)   NOT NULL,
    entity_id       character varying(100),
    old_values      jsonb,
    new_values      jsonb,
    ip_address      character varying(50),
    user_agent      character varying(512),
    additional_data jsonb,
    is_successful   boolean                  NOT NULL,
    error_message   character varying(2000),
    created_at      timestamp with time zone NOT NULL,
    created_by      uuid,
    updated_at      timestamp with time zone,
    updated_by      uuid,
    is_deleted      boolean                  NOT NULL,
    deleted_at      timestamp with time zone,
    deleted_by      uuid,
    row_version     bigint                   NOT NULL DEFAULT 0,
    CONSTRAINT pk_audit_logs PRIMARY KEY (id)
);

CREATE INDEX ix_audit_logs_user_id               ON audit_logs (user_id);
CREATE INDEX ix_audit_logs_action                ON audit_logs (action);
CREATE INDEX ix_audit_logs_entity_type           ON audit_logs (entity_type);
CREATE INDEX ix_audit_logs_created_at            ON audit_logs (created_at);
CREATE INDEX ix_audit_logs_entity_type_entity_id ON audit_logs (entity_type, entity_id);

-- ============================================================
-- Seed data — 7 Roles
-- (timestamps from model snapshot)
-- ============================================================

INSERT INTO roles (id, name, normalized_name, description, is_system, created_at, is_deleted)
VALUES
  ('10000000-0000-0000-0000-000000000001', 'Administrator',    'ADMINISTRATOR',    'Full system access',               true, '2026-05-20 13:33:35.539031+00', false),
  ('10000000-0000-0000-0000-000000000002', 'ContentCreator',   'CONTENTCREATOR',   'Create and edit content',          true, '2026-05-20 13:33:35.539107+00', false),
  ('10000000-0000-0000-0000-000000000003', 'Reviewer',         'REVIEWER',         'Review and approve content',       true, '2026-05-20 13:33:35.539107+00', false),
  ('10000000-0000-0000-0000-000000000004', 'LanguageReviewer', 'LANGUAGEREVIEWER', 'Review language and translations', true, '2026-05-20 13:33:35.539107+00', false),
  ('10000000-0000-0000-0000-000000000005', 'Designer',         'DESIGNER',         'Manage media assets',              true, '2026-05-20 13:33:35.539108+00', false),
  ('10000000-0000-0000-0000-000000000006', 'Publisher',        'PUBLISHER',        'Publish and schedule content',     true, '2026-05-20 13:33:35.539108+00', false),
  ('10000000-0000-0000-0000-000000000007', 'Archivist',        'ARCHIVIST',        'Archive and restore content',      true, '2026-05-20 13:33:35.539108+00', false);

-- ============================================================
-- Seed data — 17 Permissions
-- (timestamps from model snapshot)
-- ============================================================

INSERT INTO permissions (id, name, normalized_name, description, module, created_at, is_deleted)
VALUES
  ('20000000-0000-0000-0000-000000000001', 'CreateContent',      'CREATECONTENT',      'CreateContent permission',      'Content',  '2026-05-20 13:33:35.533353+00', false),
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
-- Mark both EF migrations as applied
-- ============================================================

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion") VALUES
  ('20260518000000_InitialCreate',   '9.0.4'),
  ('20260520133336_InitialDatabase', '9.0.4');

COMMIT;
