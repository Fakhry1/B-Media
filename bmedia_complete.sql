-- ============================================================
--  BMedia — Complete Database Setup Script
--  يحذف كل شيء ويُنشئ الجداول الصحيحة مع بيانات الـ Seed
--  Run against: bmedia_db
--  بعد التشغيل: شغّل الـ backend مباشرة (لا حاجة لـ migrations)
-- ============================================================

CREATE EXTENSION IF NOT EXISTS pgcrypto;

-- ============================================================
-- 1. حذف الجداول القديمة
-- ============================================================
DROP TABLE IF EXISTS audit_logs                  CASCADE;
DROP TABLE IF EXISTS content_workflow_histories  CASCADE;
DROP TABLE IF EXISTS review_comments             CASCADE;
DROP TABLE IF EXISTS content_tags                CASCADE;
DROP TABLE IF EXISTS notifications               CASCADE;
DROP TABLE IF EXISTS scheduled_publications      CASCADE;
DROP TABLE IF EXISTS attachments                 CASCADE;
DROP TABLE IF EXISTS localizations               CASCADE;
DROP TABLE IF EXISTS media_versions              CASCADE;
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
DROP TABLE IF EXISTS "__EFMigrationsHistory"     CASCADE;

-- ============================================================
-- 2. إنشاء الجداول
-- ============================================================

-- ===================== ROLES =====================
CREATE TABLE roles (
    id               uuid         NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
    name             varchar(100) NOT NULL,
    normalized_name  varchar(100) NOT NULL,
    description      varchar(500),
    is_system        boolean      NOT NULL DEFAULT false,
    created_at       timestamptz  NOT NULL,
    created_by       uuid,
    updated_at       timestamptz,
    updated_by       uuid,
    is_deleted       boolean      NOT NULL DEFAULT false,
    deleted_at       timestamptz,
    deleted_by       uuid,
    row_version      xid          NOT NULL DEFAULT '0'::xid
);
CREATE UNIQUE INDEX ix_roles_normalized_name ON roles(normalized_name) WHERE is_deleted = false;

-- ===================== PERMISSIONS =====================
CREATE TABLE permissions (
    id               uuid         NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
    name             varchar(100) NOT NULL,
    normalized_name  varchar(100) NOT NULL,
    description      varchar(500),
    module           varchar(100) NOT NULL,
    created_at       timestamptz  NOT NULL,
    created_by       uuid,
    updated_at       timestamptz,
    updated_by       uuid,
    is_deleted       boolean      NOT NULL DEFAULT false,
    deleted_at       timestamptz,
    deleted_by       uuid,
    row_version      xid          NOT NULL DEFAULT '0'::xid
);
CREATE UNIQUE INDEX ix_permissions_normalized_name ON permissions(normalized_name) WHERE is_deleted = false;
CREATE INDEX ix_permissions_module ON permissions(module);

-- ===================== USERS =====================
CREATE TABLE users (
    id                              uuid          NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
    username                        varchar(100)  NOT NULL,
    email                           varchar(256)  NOT NULL,
    password_hash                   text          NOT NULL,
    first_name                      varchar(100)  NOT NULL,
    last_name                       varchar(100)  NOT NULL,
    phone_number                    varchar(30),
    profile_picture_url             varchar(2048),
    is_active                       boolean       NOT NULL DEFAULT true,
    is_email_verified               boolean       NOT NULL DEFAULT false,
    last_login_at                   timestamptz,
    last_login_ip                   text,
    failed_login_attempts           integer       NOT NULL DEFAULT 0,
    lockout_end                     timestamptz,
    email_verification_token        text,
    email_verification_token_expiry timestamptz,
    password_reset_token            text,
    password_reset_token_expiry     timestamptz,
    preferred_language              varchar(10)   NOT NULL DEFAULT 'en',
    time_zone                       varchar(100),
    created_at                      timestamptz   NOT NULL,
    created_by                      uuid,
    updated_at                      timestamptz,
    updated_by                      uuid,
    is_deleted                      boolean       NOT NULL DEFAULT false,
    deleted_at                      timestamptz,
    deleted_by                      uuid,
    row_version                     xid           NOT NULL DEFAULT '0'::xid
);
CREATE UNIQUE INDEX ix_users_email    ON users(email)    WHERE is_deleted = false;
CREATE UNIQUE INDEX ix_users_username ON users(username) WHERE is_deleted = false;
CREATE INDEX ix_users_is_deleted ON users(is_deleted);
CREATE INDEX ix_users_is_active  ON users(is_active);

-- ===================== USER ROLES =====================
CREATE TABLE user_roles (
    user_id     uuid        NOT NULL,
    role_id     uuid        NOT NULL,
    assigned_at timestamptz NOT NULL,
    assigned_by uuid,
    PRIMARY KEY (user_id, role_id),
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE,
    FOREIGN KEY (role_id) REFERENCES roles(id) ON DELETE CASCADE
);
CREATE INDEX ix_user_roles_user_id ON user_roles(user_id);
CREATE INDEX ix_user_roles_role_id ON user_roles(role_id);

-- ===================== ROLE PERMISSIONS =====================
CREATE TABLE role_permissions (
    role_id       uuid        NOT NULL,
    permission_id uuid        NOT NULL,
    granted_at    timestamptz NOT NULL,
    granted_by    uuid,
    PRIMARY KEY (role_id, permission_id),
    FOREIGN KEY (role_id)       REFERENCES roles(id)       ON DELETE CASCADE,
    FOREIGN KEY (permission_id) REFERENCES permissions(id) ON DELETE CASCADE
);

-- ===================== REFRESH TOKENS =====================
CREATE TABLE refresh_tokens (
    id                uuid         NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
    user_id           uuid         NOT NULL,
    token             varchar(512) NOT NULL,
    expires_at        timestamptz  NOT NULL,
    is_revoked        boolean      NOT NULL DEFAULT false,
    revoked_at        timestamptz,
    revoked_reason    varchar(500),
    replaced_by_token text,
    created_by_ip     varchar(50),
    revoked_by_ip     varchar(50),
    created_at        timestamptz  NOT NULL,
    created_by        uuid,
    updated_at        timestamptz,
    updated_by        uuid,
    is_deleted        boolean      NOT NULL DEFAULT false,
    deleted_at        timestamptz,
    deleted_by        uuid,
    row_version       xid          NOT NULL DEFAULT '0'::xid,
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
);
CREATE UNIQUE INDEX ix_refresh_tokens_token ON refresh_tokens(token);
CREATE INDEX ix_refresh_tokens_user_id    ON refresh_tokens(user_id);
CREATE INDEX ix_refresh_tokens_expires_at ON refresh_tokens(expires_at);

-- ===================== CATEGORIES =====================
CREATE TABLE categories (
    id          uuid          NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
    name        varchar(200)  NOT NULL,
    slug        varchar(250)  NOT NULL,
    description varchar(1000),
    icon_url    varchar(2048),
    sort_order  integer       NOT NULL DEFAULT 0,
    is_active   boolean       NOT NULL DEFAULT true,
    created_at  timestamptz   NOT NULL,
    created_by  uuid,
    updated_at  timestamptz,
    updated_by  uuid,
    is_deleted  boolean       NOT NULL DEFAULT false,
    deleted_at  timestamptz,
    deleted_by  uuid,
    row_version xid           NOT NULL DEFAULT '0'::xid
);
CREATE UNIQUE INDEX ix_categories_slug ON categories(slug) WHERE is_deleted = false;

-- ===================== SUBCATEGORIES =====================
CREATE TABLE subcategories (
    id          uuid          NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
    name        varchar(200)  NOT NULL,
    slug        varchar(250)  NOT NULL,
    description varchar(1000),
    sort_order  integer       NOT NULL DEFAULT 0,
    is_active   boolean       NOT NULL DEFAULT true,
    category_id uuid          NOT NULL,
    created_at  timestamptz   NOT NULL,
    created_by  uuid,
    updated_at  timestamptz,
    updated_by  uuid,
    is_deleted  boolean       NOT NULL DEFAULT false,
    deleted_at  timestamptz,
    deleted_by  uuid,
    row_version xid           NOT NULL DEFAULT '0'::xid,
    FOREIGN KEY (category_id) REFERENCES categories(id) ON DELETE CASCADE
);
CREATE UNIQUE INDEX ix_subcategories_slug        ON subcategories(slug)        WHERE is_deleted = false;
CREATE        INDEX ix_subcategories_category_id ON subcategories(category_id);

-- ===================== TAGS =====================
CREATE TABLE tags (
    id              uuid         NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
    name            varchar(150) NOT NULL,
    slug            varchar(200) NOT NULL,
    description     varchar(500),
    is_ai_generated boolean      NOT NULL DEFAULT false,
    created_at      timestamptz  NOT NULL,
    created_by      uuid,
    updated_at      timestamptz,
    updated_by      uuid,
    is_deleted      boolean      NOT NULL DEFAULT false,
    deleted_at      timestamptz,
    deleted_by      uuid,
    row_version     xid          NOT NULL DEFAULT '0'::xid
);
CREATE UNIQUE INDEX ix_tags_slug ON tags(slug) WHERE is_deleted = false;

-- ===================== WORKFLOW DEFINITIONS =====================
CREATE TABLE workflow_definitions (
    id          uuid          NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
    name        varchar(200)  NOT NULL,
    description varchar(1000),
    is_default  boolean       NOT NULL DEFAULT false,
    is_active   boolean       NOT NULL DEFAULT true,
    version     integer       NOT NULL DEFAULT 1,
    created_at  timestamptz   NOT NULL,
    created_by  uuid,
    updated_at  timestamptz,
    updated_by  uuid,
    is_deleted  boolean       NOT NULL DEFAULT false,
    deleted_at  timestamptz,
    deleted_by  uuid,
    row_version xid           NOT NULL DEFAULT '0'::xid
);
CREATE INDEX ix_workflow_definitions_is_default ON workflow_definitions(is_default);

-- ===================== WORKFLOW STEPS =====================
CREATE TABLE workflow_steps (
    id                     uuid         NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
    workflow_definition_id uuid         NOT NULL,
    name                   varchar(200) NOT NULL,
    description            varchar(500),
    maps_to_status         integer      NOT NULL DEFAULT 0,
    "order"                integer      NOT NULL DEFAULT 0,
    is_initial             boolean      NOT NULL DEFAULT false,
    is_final               boolean      NOT NULL DEFAULT false,
    requires_reviewer      boolean      NOT NULL DEFAULT false,
    required_permission    varchar(200),
    sla_hours              integer,
    created_at             timestamptz  NOT NULL,
    created_by             uuid,
    updated_at             timestamptz,
    updated_by             uuid,
    is_deleted             boolean      NOT NULL DEFAULT false,
    deleted_at             timestamptz,
    deleted_by             uuid,
    row_version            xid          NOT NULL DEFAULT '0'::xid,
    FOREIGN KEY (workflow_definition_id) REFERENCES workflow_definitions(id) ON DELETE CASCADE
);
CREATE INDEX ix_workflow_steps_workflow_definition_id       ON workflow_steps(workflow_definition_id);
CREATE INDEX ix_workflow_steps_workflow_definition_id_order ON workflow_steps(workflow_definition_id, "order");

-- ===================== WORKFLOW TRANSITIONS =====================
CREATE TABLE workflow_transitions (
    id                     uuid         NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
    workflow_definition_id uuid         NOT NULL,
    from_step_id           uuid         NOT NULL,
    to_step_id             uuid         NOT NULL,
    action_name            varchar(200) NOT NULL,
    description            varchar(500),
    required_permission    varchar(200),
    requires_comment       boolean      NOT NULL DEFAULT false,
    is_active              boolean      NOT NULL DEFAULT true,
    created_at             timestamptz  NOT NULL,
    created_by             uuid,
    updated_at             timestamptz,
    updated_by             uuid,
    is_deleted             boolean      NOT NULL DEFAULT false,
    deleted_at             timestamptz,
    deleted_by             uuid,
    row_version            xid          NOT NULL DEFAULT '0'::xid,
    FOREIGN KEY (workflow_definition_id) REFERENCES workflow_definitions(id) ON DELETE CASCADE,
    FOREIGN KEY (from_step_id)           REFERENCES workflow_steps(id)       ON DELETE RESTRICT,
    FOREIGN KEY (to_step_id)             REFERENCES workflow_steps(id)       ON DELETE RESTRICT
);
CREATE UNIQUE INDEX ix_workflow_transitions_from_to         ON workflow_transitions(from_step_id, to_step_id);
CREATE        INDEX ix_workflow_transitions_definition_id   ON workflow_transitions(workflow_definition_id);

-- ===================== CONTENTS =====================
CREATE TABLE contents (
    id                       uuid          NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
    title                    varchar(500)  NOT NULL,
    slug                     varchar(600)  NOT NULL,
    summary                  varchar(2000),
    language                 varchar(10)   NOT NULL DEFAULT 'en',
    status                   integer       NOT NULL DEFAULT 1,
    is_featured              boolean       NOT NULL DEFAULT false,
    allow_comments           boolean       NOT NULL DEFAULT true,
    view_count               integer       NOT NULL DEFAULT 0,
    published_at             timestamptz,
    published_by             uuid,
    scheduled_publish_at     timestamptz,
    archived_at              timestamptz,
    category_id              uuid,
    subcategory_id           uuid,
    current_workflow_step_id uuid,
    assigned_reviewer_id     uuid,
    created_at               timestamptz   NOT NULL,
    created_by               uuid,
    updated_at               timestamptz,
    updated_by               uuid,
    is_deleted               boolean       NOT NULL DEFAULT false,
    deleted_at               timestamptz,
    deleted_by               uuid,
    row_version              xid           NOT NULL DEFAULT '0'::xid,
    FOREIGN KEY (category_id)              REFERENCES categories(id)     ON DELETE SET NULL,
    FOREIGN KEY (subcategory_id)           REFERENCES subcategories(id)  ON DELETE SET NULL,
    FOREIGN KEY (current_workflow_step_id) REFERENCES workflow_steps(id) ON DELETE SET NULL
);
CREATE UNIQUE INDEX ix_contents_slug                   ON contents(slug)                                WHERE is_deleted = false;
CREATE        INDEX ix_contents_status                 ON contents(status);
CREATE        INDEX ix_contents_language               ON contents(language);
CREATE        INDEX ix_contents_is_featured            ON contents(is_featured);
CREATE        INDEX ix_contents_published_at           ON contents(published_at);
CREATE        INDEX ix_contents_category_id            ON contents(category_id);
CREATE        INDEX ix_contents_created_at             ON contents(created_at);
CREATE        INDEX ix_contents_status_language_deleted ON contents(status, language, is_deleted);

-- ===================== CONTENT TAGS =====================
CREATE TABLE content_tags (
    content_id      uuid        NOT NULL,
    tag_id          uuid        NOT NULL,
    tagged_at       timestamptz NOT NULL DEFAULT now(),
    is_ai_generated boolean     NOT NULL DEFAULT false,
    PRIMARY KEY (content_id, tag_id),
    FOREIGN KEY (content_id) REFERENCES contents(id) ON DELETE CASCADE,
    FOREIGN KEY (tag_id)     REFERENCES tags(id)     ON DELETE CASCADE
);

-- ===================== MEDIA ASSETS =====================
-- (بعد migration 3 التي حذفت الأعمدة غير الضرورية)
CREATE TABLE media_assets (
    id               uuid          NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
    original_file_name varchar(512) NOT NULL,
    storage_key      varchar(1024) NOT NULL,
    public_url       varchar(2048),
    content_type     varchar(255)  NOT NULL,
    file_size_bytes  bigint        NOT NULL DEFAULT 0,
    media_type       integer       NOT NULL DEFAULT 0,
    status           integer       NOT NULL DEFAULT 1,
    storage_provider integer       NOT NULL DEFAULT 1,
    title            varchar(500),
    description      varchar(2000),
    alt_text         varchar(500),
    language         varchar(10)   NOT NULL DEFAULT 'en',
    duration_seconds integer,
    width            integer,
    height           integer,
    thumbnail_url    varchar(2048),
    sort_order       integer       NOT NULL DEFAULT 0,
    is_primary       boolean       NOT NULL DEFAULT false,
    content_id       uuid,
    created_at       timestamptz   NOT NULL,
    created_by       uuid,
    updated_at       timestamptz,
    updated_by       uuid,
    is_deleted       boolean       NOT NULL DEFAULT false,
    deleted_at       timestamptz,
    deleted_by       uuid,
    row_version      xid           NOT NULL DEFAULT '0'::xid,
    FOREIGN KEY (content_id) REFERENCES contents(id) ON DELETE SET NULL
);
CREATE        INDEX ix_media_assets_content_id        ON media_assets(content_id);
CREATE        INDEX ix_media_assets_media_type        ON media_assets(media_type);
CREATE        INDEX ix_media_assets_status            ON media_assets(status);
CREATE UNIQUE INDEX ix_media_assets_storage_key       ON media_assets(storage_key) WHERE is_deleted = false;
CREATE        INDEX ix_media_assets_content_is_primary ON media_assets(content_id, is_primary);

-- ===================== MEDIA VERSIONS =====================
CREATE TABLE media_versions (
    id               uuid          NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
    media_asset_id   uuid          NOT NULL,
    storage_key      varchar(1024) NOT NULL,
    public_url       varchar(2048),
    cdn_url          varchar(2048),
    content_type     varchar(255)  NOT NULL,
    file_size_bytes  bigint        NOT NULL DEFAULT 0,
    width            integer,
    height           integer,
    bitrate          integer,
    duration_seconds integer,
    quality          integer,
    version_label    varchar(100)  NOT NULL DEFAULT 'original',
    is_hls           boolean       NOT NULL DEFAULT false,
    hls_manifest_url varchar(2048),
    is_default       boolean       NOT NULL DEFAULT false,
    storage_provider integer       NOT NULL DEFAULT 1,
    created_at       timestamptz   NOT NULL,
    created_by       uuid,
    updated_at       timestamptz,
    updated_by       uuid,
    is_deleted       boolean       NOT NULL DEFAULT false,
    deleted_at       timestamptz,
    deleted_by       uuid,
    row_version      xid           NOT NULL DEFAULT '0'::xid,
    FOREIGN KEY (media_asset_id) REFERENCES media_assets(id) ON DELETE CASCADE
);
CREATE INDEX ix_media_versions_media_asset_id ON media_versions(media_asset_id);
CREATE INDEX ix_media_versions_quality        ON media_versions(quality);
CREATE INDEX ix_media_versions_is_default     ON media_versions(is_default);

-- ===================== LOCALIZATIONS =====================
-- (بعد migration 3 التي حذفت body, seo_title, seo_description)
CREATE TABLE localizations (
    id          uuid          NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
    content_id  uuid          NOT NULL,
    language    varchar(10)   NOT NULL,
    title       varchar(500)  NOT NULL,
    summary     varchar(2000),
    is_approved boolean       NOT NULL DEFAULT false,
    approved_at timestamptz,
    approved_by uuid,
    created_at  timestamptz   NOT NULL,
    created_by  uuid,
    updated_at  timestamptz,
    updated_by  uuid,
    is_deleted  boolean       NOT NULL DEFAULT false,
    deleted_at  timestamptz,
    deleted_by  uuid,
    row_version xid           NOT NULL DEFAULT '0'::xid,
    FOREIGN KEY (content_id) REFERENCES contents(id) ON DELETE CASCADE
);
CREATE UNIQUE INDEX ix_localizations_content_id_language ON localizations(content_id, language) WHERE is_deleted = false;

-- ===================== ATTACHMENTS =====================
CREATE TABLE attachments (
    id              uuid          NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
    content_id      uuid          NOT NULL,
    file_name       varchar(512)  NOT NULL,
    storage_key     varchar(1024) NOT NULL,
    public_url      varchar(2048),
    content_type    varchar(255)  NOT NULL,
    file_size_bytes bigint        NOT NULL DEFAULT 0,
    description     varchar(1000),
    sort_order      integer       NOT NULL DEFAULT 0,
    created_at      timestamptz   NOT NULL,
    created_by      uuid,
    updated_at      timestamptz,
    updated_by      uuid,
    is_deleted      boolean       NOT NULL DEFAULT false,
    deleted_at      timestamptz,
    deleted_by      uuid,
    row_version     xid           NOT NULL DEFAULT '0'::xid,
    FOREIGN KEY (content_id) REFERENCES contents(id) ON DELETE CASCADE
);
CREATE INDEX ix_attachments_content_id ON attachments(content_id);

-- ===================== SCHEDULED PUBLICATIONS =====================
CREATE TABLE scheduled_publications (
    id              uuid          NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
    content_id      uuid          NOT NULL,
    scheduled_at    timestamptz   NOT NULL,
    is_executed     boolean       NOT NULL DEFAULT false,
    executed_at     timestamptz,
    is_successful   boolean       NOT NULL DEFAULT false,
    error_message   varchar(2000),
    hangfire_job_id varchar(200),
    created_at      timestamptz   NOT NULL,
    created_by      uuid,
    updated_at      timestamptz,
    updated_by      uuid,
    is_deleted      boolean       NOT NULL DEFAULT false,
    deleted_at      timestamptz,
    deleted_by      uuid,
    row_version     xid           NOT NULL DEFAULT '0'::xid,
    FOREIGN KEY (content_id) REFERENCES contents(id) ON DELETE CASCADE
);
CREATE INDEX ix_scheduled_publications_content_id   ON scheduled_publications(content_id);
CREATE INDEX ix_scheduled_publications_scheduled_at ON scheduled_publications(scheduled_at, is_executed);

-- ===================== REVIEW COMMENTS =====================
CREATE TABLE review_comments (
    id                uuid          NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
    content_id        uuid          NOT NULL,
    reviewer_id       uuid          NOT NULL,
    comment           varchar(5000) NOT NULL,
    is_resolved       boolean       NOT NULL DEFAULT false,
    resolved_at       timestamptz,
    resolved_by       uuid,
    parent_comment_id uuid,
    created_at        timestamptz   NOT NULL,
    created_by        uuid,
    updated_at        timestamptz,
    updated_by        uuid,
    is_deleted        boolean       NOT NULL DEFAULT false,
    deleted_at        timestamptz,
    deleted_by        uuid,
    row_version       xid           NOT NULL DEFAULT '0'::xid,
    FOREIGN KEY (content_id)        REFERENCES contents(id)        ON DELETE CASCADE,
    FOREIGN KEY (reviewer_id)       REFERENCES users(id)           ON DELETE RESTRICT,
    FOREIGN KEY (parent_comment_id) REFERENCES review_comments(id) ON DELETE RESTRICT
);
CREATE INDEX ix_review_comments_content_id  ON review_comments(content_id);
CREATE INDEX ix_review_comments_reviewer_id ON review_comments(reviewer_id);

-- ===================== CONTENT WORKFLOW HISTORIES =====================
CREATE TABLE content_workflow_histories (
    id                 uuid          NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
    content_id         uuid          NOT NULL,
    from_step_id       uuid,
    to_step_id         uuid          NOT NULL,
    transitioned_by_id uuid          NOT NULL,
    from_status        integer       NOT NULL DEFAULT 0,
    to_status          integer       NOT NULL DEFAULT 0,
    comment            varchar(2000),
    action_name        varchar(200),
    transitioned_at    timestamptz   NOT NULL DEFAULT now(),
    created_at         timestamptz   NOT NULL,
    created_by         uuid,
    updated_at         timestamptz,
    updated_by         uuid,
    is_deleted         boolean       NOT NULL DEFAULT false,
    deleted_at         timestamptz,
    deleted_by         uuid,
    row_version        xid           NOT NULL DEFAULT '0'::xid,
    FOREIGN KEY (content_id)         REFERENCES contents(id)       ON DELETE CASCADE,
    FOREIGN KEY (transitioned_by_id) REFERENCES users(id)          ON DELETE RESTRICT,
    FOREIGN KEY (from_step_id)       REFERENCES workflow_steps(id) ON DELETE SET NULL,
    FOREIGN KEY (to_step_id)         REFERENCES workflow_steps(id) ON DELETE RESTRICT
);
CREATE INDEX ix_content_workflow_histories_content_id      ON content_workflow_histories(content_id);
CREATE INDEX ix_content_workflow_histories_transitioned_at ON content_workflow_histories(transitioned_at);

-- ===================== NOTIFICATIONS =====================
CREATE TABLE notifications (
    id             uuid          NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
    user_id        uuid          NOT NULL,
    type           integer       NOT NULL DEFAULT 0,
    title          varchar(300)  NOT NULL,
    message        varchar(2000) NOT NULL,
    is_read        boolean       NOT NULL DEFAULT false,
    read_at        timestamptz,
    reference_id   uuid,
    reference_type varchar(100),
    action_url     varchar(2048),
    metadata       jsonb,
    created_at     timestamptz   NOT NULL,
    created_by     uuid,
    updated_at     timestamptz,
    updated_by     uuid,
    is_deleted     boolean       NOT NULL DEFAULT false,
    deleted_at     timestamptz,
    deleted_by     uuid,
    row_version    xid           NOT NULL DEFAULT '0'::xid,
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
);
CREATE INDEX ix_notifications_user_id      ON notifications(user_id);
CREATE INDEX ix_notifications_user_is_read ON notifications(user_id, is_read);
CREATE INDEX ix_notifications_created_at   ON notifications(created_at);

-- ===================== AUDIT LOGS =====================
CREATE TABLE audit_logs (
    id              uuid          NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
    user_id         uuid,
    user_email      varchar(256),
    action          integer       NOT NULL DEFAULT 0,
    entity_type     varchar(200)  NOT NULL,
    entity_id       varchar(100),
    old_values      jsonb,
    new_values      jsonb,
    ip_address      varchar(50),
    user_agent      varchar(512),
    additional_data jsonb,
    is_successful   boolean       NOT NULL DEFAULT true,
    error_message   varchar(2000),
    created_at      timestamptz   NOT NULL,
    created_by      uuid,
    updated_at      timestamptz,
    updated_by      uuid,
    is_deleted      boolean       NOT NULL DEFAULT false,
    deleted_at      timestamptz,
    deleted_by      uuid,
    row_version     xid           NOT NULL DEFAULT '0'::xid
);
CREATE INDEX ix_audit_logs_user_id        ON audit_logs(user_id);
CREATE INDEX ix_audit_logs_action         ON audit_logs(action);
CREATE INDEX ix_audit_logs_entity_type    ON audit_logs(entity_type);
CREATE INDEX ix_audit_logs_created_at     ON audit_logs(created_at);
CREATE INDEX ix_audit_logs_entity_type_id ON audit_logs(entity_type, entity_id);

-- ============================================================
-- 3. Seed البيانات الأساسية
-- ============================================================

-- الأدوار
INSERT INTO roles (id, name, normalized_name, description, is_system, created_at, is_deleted) VALUES
    ('10000000-0000-0000-0000-000000000001', 'Administrator',    'ADMINISTRATOR',    'Full system access',                true,  '2026-01-01 00:00:00+00', false),
    ('10000000-0000-0000-0000-000000000002', 'ContentCreator',   'CONTENTCREATOR',   'Create and edit content',           true,  '2026-01-01 00:00:00+00', false),
    ('10000000-0000-0000-0000-000000000003', 'Reviewer',         'REVIEWER',         'Review and approve content',        true,  '2026-01-01 00:00:00+00', false),
    ('10000000-0000-0000-0000-000000000004', 'LanguageReviewer', 'LANGUAGEREVIEWER', 'Review language and translations',  true,  '2026-01-01 00:00:00+00', false),
    ('10000000-0000-0000-0000-000000000005', 'Designer',         'DESIGNER',         'Manage media assets',               true,  '2026-01-01 00:00:00+00', false),
    ('10000000-0000-0000-0000-000000000006', 'Publisher',        'PUBLISHER',        'Publish and schedule content',      true,  '2026-01-01 00:00:00+00', false),
    ('10000000-0000-0000-0000-000000000007', 'Archivist',        'ARCHIVIST',        'Archive and restore content',       true,  '2026-01-01 00:00:00+00', false);

-- الصلاحيات
INSERT INTO permissions (id, name, normalized_name, description, module, created_at, is_deleted) VALUES
    ('20000000-0000-0000-0000-000000000001', 'CreateContent',      'CREATECONTENT',      'CreateContent permission',      'Content',  '2026-01-01 00:00:00+00', false),
    ('20000000-0000-0000-0000-000000000002', 'EditContent',        'EDITCONTENT',        'EditContent permission',        'Content',  '2026-01-01 00:00:00+00', false),
    ('20000000-0000-0000-0000-000000000003', 'DeleteContent',      'DELETECONTENT',      'DeleteContent permission',      'Content',  '2026-01-01 00:00:00+00', false),
    ('20000000-0000-0000-0000-000000000004', 'PublishContent',     'PUBLISHCONTENT',     'PublishContent permission',     'Content',  '2026-01-01 00:00:00+00', false),
    ('20000000-0000-0000-0000-000000000005', 'ArchiveContent',     'ARCHIVECONTENT',     'ArchiveContent permission',     'Content',  '2026-01-01 00:00:00+00', false),
    ('20000000-0000-0000-0000-000000000006', 'ViewContent',        'VIEWCONTENT',        'ViewContent permission',        'Content',  '2026-01-01 00:00:00+00', false),
    ('20000000-0000-0000-0000-000000000007', 'UploadMedia',        'UPLOADMEDIA',        'UploadMedia permission',        'Media',    '2026-01-01 00:00:00+00', false),
    ('20000000-0000-0000-0000-000000000008', 'DeleteMedia',        'DELETEMEDIA',        'DeleteMedia permission',        'Media',    '2026-01-01 00:00:00+00', false),
    ('20000000-0000-0000-0000-000000000009', 'ManageMedia',        'MANAGEMEDIA',        'ManageMedia permission',        'Media',    '2026-01-01 00:00:00+00', false),
    ('20000000-0000-0000-0000-000000000010', 'ApproveReview',      'APPROVEREVIEW',      'ApproveReview permission',      'Workflow', '2026-01-01 00:00:00+00', false),
    ('20000000-0000-0000-0000-000000000011', 'RejectReview',       'REJECTREVIEW',       'RejectReview permission',       'Workflow', '2026-01-01 00:00:00+00', false),
    ('20000000-0000-0000-0000-000000000012', 'TransitionWorkflow', 'TRANSITIONWORKFLOW', 'TransitionWorkflow permission', 'Workflow', '2026-01-01 00:00:00+00', false),
    ('20000000-0000-0000-0000-000000000013', 'ManageUsers',        'MANAGEUSERS',        'ManageUsers permission',        'Admin',    '2026-01-01 00:00:00+00', false),
    ('20000000-0000-0000-0000-000000000014', 'ManageRoles',        'MANAGEROLES',        'ManageRoles permission',        'Admin',    '2026-01-01 00:00:00+00', false),
    ('20000000-0000-0000-0000-000000000015', 'ViewAuditLogs',      'VIEWAUDITLOGS',      'ViewAuditLogs permission',      'Admin',    '2026-01-01 00:00:00+00', false),
    ('20000000-0000-0000-0000-000000000016', 'ManageCategories',   'MANAGECATEGORIES',   'ManageCategories permission',   'Taxonomy', '2026-01-01 00:00:00+00', false),
    ('20000000-0000-0000-0000-000000000017', 'ManageTags',         'MANAGETAGS',         'ManageTags permission',         'Taxonomy', '2026-01-01 00:00:00+00', false);

-- Administrator: كل الصلاحيات
INSERT INTO role_permissions (role_id, permission_id, granted_at)
SELECT '10000000-0000-0000-0000-000000000001', id, '2026-01-01 00:00:00+00' FROM permissions;

-- ContentCreator
INSERT INTO role_permissions (role_id, permission_id, granted_at) VALUES
    ('10000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000001', '2026-01-01 00:00:00+00'),
    ('10000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002', '2026-01-01 00:00:00+00'),
    ('10000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000006', '2026-01-01 00:00:00+00'),
    ('10000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000007', '2026-01-01 00:00:00+00'),
    ('10000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000009', '2026-01-01 00:00:00+00');

-- Reviewer
INSERT INTO role_permissions (role_id, permission_id, granted_at) VALUES
    ('10000000-0000-0000-0000-000000000003', '20000000-0000-0000-0000-000000000006', '2026-01-01 00:00:00+00'),
    ('10000000-0000-0000-0000-000000000003', '20000000-0000-0000-0000-000000000010', '2026-01-01 00:00:00+00'),
    ('10000000-0000-0000-0000-000000000003', '20000000-0000-0000-0000-000000000011', '2026-01-01 00:00:00+00'),
    ('10000000-0000-0000-0000-000000000003', '20000000-0000-0000-0000-000000000012', '2026-01-01 00:00:00+00');

-- LanguageReviewer
INSERT INTO role_permissions (role_id, permission_id, granted_at) VALUES
    ('10000000-0000-0000-0000-000000000004', '20000000-0000-0000-0000-000000000002', '2026-01-01 00:00:00+00'),
    ('10000000-0000-0000-0000-000000000004', '20000000-0000-0000-0000-000000000006', '2026-01-01 00:00:00+00');

-- Designer
INSERT INTO role_permissions (role_id, permission_id, granted_at) VALUES
    ('10000000-0000-0000-0000-000000000005', '20000000-0000-0000-0000-000000000006', '2026-01-01 00:00:00+00'),
    ('10000000-0000-0000-0000-000000000005', '20000000-0000-0000-0000-000000000007', '2026-01-01 00:00:00+00'),
    ('10000000-0000-0000-0000-000000000005', '20000000-0000-0000-0000-000000000008', '2026-01-01 00:00:00+00'),
    ('10000000-0000-0000-0000-000000000005', '20000000-0000-0000-0000-000000000009', '2026-01-01 00:00:00+00');

-- Publisher
INSERT INTO role_permissions (role_id, permission_id, granted_at) VALUES
    ('10000000-0000-0000-0000-000000000006', '20000000-0000-0000-0000-000000000004', '2026-01-01 00:00:00+00'),
    ('10000000-0000-0000-0000-000000000006', '20000000-0000-0000-0000-000000000006', '2026-01-01 00:00:00+00'),
    ('10000000-0000-0000-0000-000000000006', '20000000-0000-0000-0000-000000000012', '2026-01-01 00:00:00+00');

-- Archivist
INSERT INTO role_permissions (role_id, permission_id, granted_at) VALUES
    ('10000000-0000-0000-0000-000000000007', '20000000-0000-0000-0000-000000000005', '2026-01-01 00:00:00+00'),
    ('10000000-0000-0000-0000-000000000007', '20000000-0000-0000-0000-000000000006', '2026-01-01 00:00:00+00');

-- ============================================================
-- 4. المستخدم admin الافتراضي
--    Email:    admin@bmedia.io
--    Password: Admin@1234
-- ============================================================
INSERT INTO users (
    id, username, email, password_hash,
    first_name, last_name,
    is_active, is_email_verified, preferred_language,
    created_at, is_deleted
) VALUES (
    'a0000000-0000-0000-0000-000000000001',
    'admin', 'admin@bmedia.io',
    crypt('Admin@1234', gen_salt('bf', 11)),
    'Admin', 'User',
    true, true, 'ar',
    '2026-01-01 00:00:00+00', false
);

INSERT INTO user_roles (user_id, role_id, assigned_at) VALUES
    ('a0000000-0000-0000-0000-000000000001', '10000000-0000-0000-0000-000000000001', '2026-01-01 00:00:00+00');

-- ============================================================
-- 5. تسجيل الـ Migrations (لمنع EF Core من إعادة تطبيقها)
-- ============================================================
CREATE TABLE "__EFMigrationsHistory" (
    "MigrationId"    varchar(150) NOT NULL PRIMARY KEY,
    "ProductVersion" varchar(32)  NOT NULL
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion") VALUES
    ('20260518000000_InitialCreate',               '9.0.4'),
    ('20260610000001_SeedRolePermissions',          '9.0.4'),
    ('20260611000001_DropRemovedMediaAssetColumns', '9.0.4');

-- ============================================================
-- تم! تقرير الحالة
-- ============================================================
SELECT
    (SELECT count(*) FROM users)       AS users,
    (SELECT count(*) FROM roles)       AS roles,
    (SELECT count(*) FROM permissions) AS permissions,
    (SELECT count(*) FROM role_permissions) AS role_permissions;
