-- ============================================================
--  BMedia Database Fix Script
--  تشغيل هذا الـ script يحذف كل الجداول ويعيد إنشاءها بشكل صحيح
--  Run against: bmedia_db
-- ============================================================

-- تفعيل pgcrypto لتشفير كلمة المرور
CREATE EXTENSION IF NOT EXISTS pgcrypto;

-- ============================================================
-- 1. حذف الجداول القديمة (بالترتيب العكسي للـ FK)
-- ============================================================
DROP TABLE IF EXISTS audit_logs                CASCADE;
DROP TABLE IF EXISTS content_workflow_histories CASCADE;
DROP TABLE IF EXISTS review_comments           CASCADE;
DROP TABLE IF EXISTS content_tags              CASCADE;
DROP TABLE IF EXISTS notifications             CASCADE;
DROP TABLE IF EXISTS scheduled_publications    CASCADE;
DROP TABLE IF EXISTS attachments               CASCADE;
DROP TABLE IF EXISTS localizations             CASCADE;
DROP TABLE IF EXISTS media_versions            CASCADE;
DROP TABLE IF EXISTS media_assets              CASCADE;
DROP TABLE IF EXISTS contents                  CASCADE;
DROP TABLE IF EXISTS workflow_transitions      CASCADE;
DROP TABLE IF EXISTS workflow_steps            CASCADE;
DROP TABLE IF EXISTS workflow_definitions      CASCADE;
DROP TABLE IF EXISTS tags                      CASCADE;
DROP TABLE IF EXISTS subcategories             CASCADE;
DROP TABLE IF EXISTS categories                CASCADE;
DROP TABLE IF EXISTS refresh_tokens            CASCADE;
DROP TABLE IF EXISTS user_roles                CASCADE;
DROP TABLE IF EXISTS role_permissions          CASCADE;
DROP TABLE IF EXISTS users                     CASCADE;
DROP TABLE IF EXISTS roles                     CASCADE;
DROP TABLE IF EXISTS permissions               CASCADE;

-- حذف جدول migrations لإعادة تسجيلها
DROP TABLE IF EXISTS "__EFMigrationsHistory";

-- ============================================================
-- 2. إنشاء الجداول الأساسية
-- ============================================================

CREATE TABLE roles (
    id                uuid          NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
    name              varchar(100)  NOT NULL,
    normalized_name   varchar(100)  NOT NULL,
    description       varchar(500),
    is_system         boolean       NOT NULL DEFAULT false,
    created_at        timestamptz   NOT NULL DEFAULT now(),
    created_by        uuid,
    updated_at        timestamptz,
    updated_by        uuid,
    is_deleted        boolean       NOT NULL DEFAULT false,
    deleted_at        timestamptz,
    deleted_by        uuid,
    row_version       xid           NOT NULL DEFAULT '0'::xid
);

CREATE TABLE permissions (
    id                uuid          NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
    name              varchar(100)  NOT NULL,
    normalized_name   varchar(100)  NOT NULL,
    description       varchar(500),
    module            varchar(100)  NOT NULL,
    created_at        timestamptz   NOT NULL DEFAULT now(),
    created_by        uuid,
    updated_at        timestamptz,
    updated_by        uuid,
    is_deleted        boolean       NOT NULL DEFAULT false,
    deleted_at        timestamptz,
    deleted_by        uuid,
    row_version       xid           NOT NULL DEFAULT '0'::xid
);

CREATE TABLE users (
    id                              uuid           NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
    username                        varchar(100)   NOT NULL,
    email                           varchar(256)   NOT NULL,
    password_hash                   text           NOT NULL,
    first_name                      varchar(100)   NOT NULL,
    last_name                       varchar(100)   NOT NULL,
    phone_number                    varchar(30),
    profile_picture_url             varchar(2048),
    is_active                       boolean        NOT NULL DEFAULT true,
    is_email_verified               boolean        NOT NULL DEFAULT false,
    last_login_at                   timestamptz,
    last_login_ip                   text,
    failed_login_attempts           integer        NOT NULL DEFAULT 0,
    lockout_end                     timestamptz,
    email_verification_token        text,
    email_verification_token_expiry timestamptz,
    password_reset_token            text,
    password_reset_token_expiry     timestamptz,
    preferred_language              varchar(10)    NOT NULL DEFAULT 'en',
    time_zone                       varchar(100),
    created_at                      timestamptz    NOT NULL DEFAULT now(),
    created_by                      uuid,
    updated_at                      timestamptz,
    updated_by                      uuid,
    is_deleted                      boolean        NOT NULL DEFAULT false,
    deleted_at                      timestamptz,
    deleted_by                      uuid,
    row_version                     xid            NOT NULL DEFAULT '0'::xid
);

CREATE TABLE user_roles (
    user_id     uuid        NOT NULL,
    role_id     uuid        NOT NULL,
    assigned_at timestamptz NOT NULL DEFAULT now(),
    assigned_by uuid,
    PRIMARY KEY (user_id, role_id),
    FOREIGN KEY (user_id) REFERENCES users(id)  ON DELETE CASCADE,
    FOREIGN KEY (role_id) REFERENCES roles(id)  ON DELETE CASCADE
);

CREATE TABLE role_permissions (
    role_id       uuid        NOT NULL,
    permission_id uuid        NOT NULL,
    granted_at    timestamptz NOT NULL DEFAULT now(),
    granted_by    uuid,
    PRIMARY KEY (role_id, permission_id),
    FOREIGN KEY (role_id)       REFERENCES roles(id)       ON DELETE CASCADE,
    FOREIGN KEY (permission_id) REFERENCES permissions(id) ON DELETE CASCADE
);

CREATE TABLE refresh_tokens (
    id                uuid           NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
    user_id           uuid           NOT NULL,
    token             varchar(512)   NOT NULL,
    expires_at        timestamptz    NOT NULL,
    is_revoked        boolean        NOT NULL DEFAULT false,
    revoked_at        timestamptz,
    revoked_reason    varchar(500),
    replaced_by_token text,
    created_by_ip     varchar(50),
    revoked_by_ip     varchar(50),
    created_at        timestamptz    NOT NULL DEFAULT now(),
    created_by        uuid,
    updated_at        timestamptz,
    updated_by        uuid,
    is_deleted        boolean        NOT NULL DEFAULT false,
    deleted_at        timestamptz,
    deleted_by        uuid,
    row_version       xid            NOT NULL DEFAULT '0'::xid,
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
);

CREATE TABLE categories (
    id          uuid           NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
    name        varchar(200)   NOT NULL,
    slug        varchar(250)   NOT NULL,
    description varchar(1000),
    icon_url    varchar(2048),
    sort_order  integer        NOT NULL DEFAULT 0,
    is_active   boolean        NOT NULL DEFAULT true,
    created_at  timestamptz    NOT NULL DEFAULT now(),
    created_by  uuid,
    updated_at  timestamptz,
    updated_by  uuid,
    is_deleted  boolean        NOT NULL DEFAULT false,
    deleted_at  timestamptz,
    deleted_by  uuid,
    row_version xid            NOT NULL DEFAULT '0'::xid
);

CREATE TABLE subcategories (
    id          uuid           NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
    name        varchar(200)   NOT NULL,
    slug        varchar(250)   NOT NULL,
    description varchar(1000),
    sort_order  integer        NOT NULL DEFAULT 0,
    is_active   boolean        NOT NULL DEFAULT true,
    category_id uuid           NOT NULL,
    created_at  timestamptz    NOT NULL DEFAULT now(),
    created_by  uuid,
    updated_at  timestamptz,
    updated_by  uuid,
    is_deleted  boolean        NOT NULL DEFAULT false,
    deleted_at  timestamptz,
    deleted_by  uuid,
    row_version xid            NOT NULL DEFAULT '0'::xid,
    FOREIGN KEY (category_id) REFERENCES categories(id) ON DELETE CASCADE
);

CREATE TABLE tags (
    id              uuid          NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
    name            varchar(150)  NOT NULL,
    slug            varchar(200)  NOT NULL,
    description     varchar(500),
    is_ai_generated boolean       NOT NULL DEFAULT false,
    created_at      timestamptz   NOT NULL DEFAULT now(),
    created_by      uuid,
    updated_at      timestamptz,
    updated_by      uuid,
    is_deleted      boolean       NOT NULL DEFAULT false,
    deleted_at      timestamptz,
    deleted_by      uuid,
    row_version     xid           NOT NULL DEFAULT '0'::xid
);

CREATE TABLE workflow_definitions (
    id          uuid           NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
    name        varchar(200)   NOT NULL,
    description varchar(1000),
    is_default  boolean        NOT NULL DEFAULT false,
    is_active   boolean        NOT NULL DEFAULT true,
    version     integer        NOT NULL DEFAULT 1,
    created_at  timestamptz    NOT NULL DEFAULT now(),
    created_by  uuid,
    updated_at  timestamptz,
    updated_by  uuid,
    is_deleted  boolean        NOT NULL DEFAULT false,
    deleted_at  timestamptz,
    deleted_by  uuid,
    row_version xid            NOT NULL DEFAULT '0'::xid
);

CREATE TABLE workflow_steps (
    id                    uuid           NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
    workflow_definition_id uuid          NOT NULL,
    name                  varchar(200)   NOT NULL,
    description           varchar(1000),
    "order"               integer        NOT NULL,
    is_initial            boolean        NOT NULL DEFAULT false,
    is_final              boolean        NOT NULL DEFAULT false,
    required_permission   varchar(200),
    created_at            timestamptz    NOT NULL DEFAULT now(),
    created_by            uuid,
    updated_at            timestamptz,
    updated_by            uuid,
    is_deleted            boolean        NOT NULL DEFAULT false,
    deleted_at            timestamptz,
    deleted_by            uuid,
    row_version           xid            NOT NULL DEFAULT '0'::xid,
    FOREIGN KEY (workflow_definition_id) REFERENCES workflow_definitions(id) ON DELETE CASCADE
);

CREATE TABLE workflow_transitions (
    id                    uuid           NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
    workflow_definition_id uuid          NOT NULL,
    from_step_id          uuid           NOT NULL,
    to_step_id            uuid           NOT NULL,
    action_name           varchar(200)   NOT NULL,
    description           varchar(1000),
    required_permission   varchar(200),
    requires_comment      boolean        NOT NULL DEFAULT false,
    created_at            timestamptz    NOT NULL DEFAULT now(),
    created_by            uuid,
    updated_at            timestamptz,
    updated_by            uuid,
    is_deleted            boolean        NOT NULL DEFAULT false,
    deleted_at            timestamptz,
    deleted_by            uuid,
    row_version           xid            NOT NULL DEFAULT '0'::xid,
    FOREIGN KEY (workflow_definition_id) REFERENCES workflow_definitions(id) ON DELETE CASCADE,
    FOREIGN KEY (from_step_id) REFERENCES workflow_steps(id) ON DELETE CASCADE,
    FOREIGN KEY (to_step_id)   REFERENCES workflow_steps(id) ON DELETE CASCADE
);

CREATE TABLE contents (
    id                    uuid           NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
    title                 varchar(500)   NOT NULL,
    slug                  varchar(600)   NOT NULL,
    summary               varchar(2000),
    language              varchar(10)    NOT NULL DEFAULT 'ar',
    status                integer        NOT NULL DEFAULT 0,
    is_featured           boolean        NOT NULL DEFAULT false,
    allow_comments        boolean        NOT NULL DEFAULT true,
    view_count            bigint         NOT NULL DEFAULT 0,
    published_at          timestamptz,
    scheduled_publish_at  timestamptz,
    current_workflow_step varchar(200),
    category_id           uuid,
    subcategory_id        uuid,
    created_at            timestamptz    NOT NULL DEFAULT now(),
    created_by            uuid,
    updated_at            timestamptz,
    updated_by            uuid,
    is_deleted            boolean        NOT NULL DEFAULT false,
    deleted_at            timestamptz,
    deleted_by            uuid,
    row_version           xid            NOT NULL DEFAULT '0'::xid,
    FOREIGN KEY (category_id)    REFERENCES categories(id)    ON DELETE SET NULL,
    FOREIGN KEY (subcategory_id) REFERENCES subcategories(id) ON DELETE SET NULL
);

CREATE TABLE media_assets (
    id                 uuid           NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
    content_id         uuid,
    original_file_name varchar(500)   NOT NULL,
    content_type       varchar(200)   NOT NULL,
    media_type         integer        NOT NULL DEFAULT 0,
    status             integer        NOT NULL DEFAULT 0,
    storage_key        varchar(1000)  NOT NULL,
    public_url         varchar(2048),
    thumbnail_url      varchar(2048),
    file_size_bytes    bigint         NOT NULL DEFAULT 0,
    duration_seconds   double precision,
    width              integer,
    height             integer,
    is_primary         boolean        NOT NULL DEFAULT false,
    sort_order         integer        NOT NULL DEFAULT 0,
    language           varchar(10)    NOT NULL DEFAULT 'ar',
    title              varchar(500),
    description        text,
    alt_text           varchar(500),
    created_at         timestamptz    NOT NULL DEFAULT now(),
    created_by         uuid,
    updated_at         timestamptz,
    updated_by         uuid,
    is_deleted         boolean        NOT NULL DEFAULT false,
    deleted_at         timestamptz,
    deleted_by         uuid,
    row_version        xid            NOT NULL DEFAULT '0'::xid,
    FOREIGN KEY (content_id) REFERENCES contents(id) ON DELETE SET NULL
);

CREATE TABLE media_versions (
    id             uuid          NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
    asset_id       uuid          NOT NULL,
    version_number integer       NOT NULL,
    storage_key    varchar(1000) NOT NULL,
    file_size_bytes bigint       NOT NULL DEFAULT 0,
    created_at     timestamptz   NOT NULL DEFAULT now(),
    created_by     uuid,
    updated_at     timestamptz,
    updated_by     uuid,
    is_deleted     boolean       NOT NULL DEFAULT false,
    deleted_at     timestamptz,
    deleted_by     uuid,
    row_version    xid           NOT NULL DEFAULT '0'::xid,
    FOREIGN KEY (asset_id) REFERENCES media_assets(id) ON DELETE CASCADE
);

CREATE TABLE content_tags (
    content_id uuid NOT NULL,
    tag_id     uuid NOT NULL,
    PRIMARY KEY (content_id, tag_id),
    FOREIGN KEY (content_id) REFERENCES contents(id)  ON DELETE CASCADE,
    FOREIGN KEY (tag_id)     REFERENCES tags(id)      ON DELETE CASCADE
);

CREATE TABLE localizations (
    id          uuid          NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
    content_id  uuid          NOT NULL,
    language    varchar(10)   NOT NULL,
    title       varchar(500)  NOT NULL,
    summary     varchar(2000),
    body        text,
    created_at  timestamptz   NOT NULL DEFAULT now(),
    created_by  uuid,
    updated_at  timestamptz,
    updated_by  uuid,
    is_deleted  boolean       NOT NULL DEFAULT false,
    deleted_at  timestamptz,
    deleted_by  uuid,
    row_version xid           NOT NULL DEFAULT '0'::xid,
    FOREIGN KEY (content_id) REFERENCES contents(id) ON DELETE CASCADE
);

CREATE TABLE attachments (
    id               uuid          NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
    content_id       uuid          NOT NULL,
    file_name        varchar(500)  NOT NULL,
    storage_key      varchar(1000) NOT NULL,
    content_type     varchar(200)  NOT NULL,
    file_size_bytes  bigint        NOT NULL DEFAULT 0,
    sort_order       integer       NOT NULL DEFAULT 0,
    created_at       timestamptz   NOT NULL DEFAULT now(),
    created_by       uuid,
    updated_at       timestamptz,
    updated_by       uuid,
    is_deleted       boolean       NOT NULL DEFAULT false,
    deleted_at       timestamptz,
    deleted_by       uuid,
    row_version      xid           NOT NULL DEFAULT '0'::xid,
    FOREIGN KEY (content_id) REFERENCES contents(id) ON DELETE CASCADE
);

CREATE TABLE scheduled_publications (
    id           uuid        NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
    content_id   uuid        NOT NULL,
    scheduled_at timestamptz NOT NULL,
    processed_at timestamptz,
    is_processed boolean     NOT NULL DEFAULT false,
    created_at   timestamptz NOT NULL DEFAULT now(),
    created_by   uuid,
    updated_at   timestamptz,
    updated_by   uuid,
    is_deleted   boolean     NOT NULL DEFAULT false,
    deleted_at   timestamptz,
    deleted_by   uuid,
    row_version  xid         NOT NULL DEFAULT '0'::xid,
    FOREIGN KEY (content_id) REFERENCES contents(id) ON DELETE CASCADE
);

CREATE TABLE notifications (
    id         uuid          NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
    user_id    uuid          NOT NULL,
    title      varchar(500)  NOT NULL,
    message    text          NOT NULL,
    type       integer       NOT NULL DEFAULT 0,
    is_read    boolean       NOT NULL DEFAULT false,
    read_at    timestamptz,
    created_at timestamptz   NOT NULL DEFAULT now(),
    created_by uuid,
    updated_at timestamptz,
    updated_by uuid,
    is_deleted boolean       NOT NULL DEFAULT false,
    deleted_at timestamptz,
    deleted_by uuid,
    row_version xid          NOT NULL DEFAULT '0'::xid,
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
);

CREATE TABLE review_comments (
    id         uuid         NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
    content_id uuid         NOT NULL,
    user_id    uuid         NOT NULL,
    comment    text         NOT NULL,
    step_name  varchar(200),
    created_at timestamptz  NOT NULL DEFAULT now(),
    created_by uuid,
    updated_at timestamptz,
    updated_by uuid,
    is_deleted boolean      NOT NULL DEFAULT false,
    deleted_at timestamptz,
    deleted_by uuid,
    row_version xid         NOT NULL DEFAULT '0'::xid,
    FOREIGN KEY (content_id) REFERENCES contents(id) ON DELETE CASCADE,
    FOREIGN KEY (user_id)    REFERENCES users(id)    ON DELETE CASCADE
);

CREATE TABLE content_workflow_histories (
    id              uuid         NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
    content_id      uuid         NOT NULL,
    user_id         uuid,
    from_step       varchar(200),
    to_step         varchar(200) NOT NULL,
    action          varchar(200),
    comment         text,
    transition_date timestamptz  NOT NULL DEFAULT now(),
    created_at      timestamptz  NOT NULL DEFAULT now(),
    created_by      uuid,
    updated_at      timestamptz,
    updated_by      uuid,
    is_deleted      boolean      NOT NULL DEFAULT false,
    deleted_at      timestamptz,
    deleted_by      uuid,
    row_version     xid          NOT NULL DEFAULT '0'::xid,
    FOREIGN KEY (content_id) REFERENCES contents(id) ON DELETE CASCADE,
    FOREIGN KEY (user_id)    REFERENCES users(id)    ON DELETE SET NULL
);

CREATE TABLE audit_logs (
    id              uuid          NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
    user_id         uuid,
    user_email      varchar(256),
    action          integer       NOT NULL,
    entity_type     varchar(200)  NOT NULL,
    entity_id       varchar(100),
    old_values      jsonb,
    new_values      jsonb,
    ip_address      varchar(50),
    user_agent      varchar(512),
    additional_data jsonb,
    error_message   varchar(2000),
    created_at      timestamptz   NOT NULL DEFAULT now(),
    created_by      uuid,
    updated_at      timestamptz,
    updated_by      uuid,
    is_deleted      boolean       NOT NULL DEFAULT false,
    deleted_at      timestamptz,
    deleted_by      uuid,
    row_version     xid           NOT NULL DEFAULT '0'::xid
);

-- ============================================================
-- 3. إنشاء الـ Indexes (بفلاتر صحيحة)
-- ============================================================
CREATE UNIQUE INDEX ix_roles_normalized_name        ON roles(normalized_name)         WHERE is_deleted = false;
CREATE UNIQUE INDEX ix_permissions_normalized_name  ON permissions(normalized_name)   WHERE is_deleted = false;
CREATE        INDEX ix_permissions_module           ON permissions(module);
CREATE UNIQUE INDEX ix_users_email                  ON users(email)                   WHERE is_deleted = false;
CREATE UNIQUE INDEX ix_users_username               ON users(username)                WHERE is_deleted = false;
CREATE        INDEX ix_users_is_deleted             ON users(is_deleted);
CREATE        INDEX ix_users_is_active              ON users(is_active);
CREATE        INDEX ix_user_roles_user_id           ON user_roles(user_id);
CREATE        INDEX ix_user_roles_role_id           ON user_roles(role_id);
CREATE UNIQUE INDEX ix_refresh_tokens_token         ON refresh_tokens(token);
CREATE        INDEX ix_refresh_tokens_user_id       ON refresh_tokens(user_id);
CREATE        INDEX ix_refresh_tokens_expires_at    ON refresh_tokens(expires_at);
CREATE UNIQUE INDEX ix_categories_slug              ON categories(slug)               WHERE is_deleted = false;
CREATE UNIQUE INDEX ix_subcategories_slug           ON subcategories(slug)            WHERE is_deleted = false;
CREATE        INDEX ix_subcategories_category_id    ON subcategories(category_id);
CREATE UNIQUE INDEX ix_tags_slug                    ON tags(slug)                     WHERE is_deleted = false;
CREATE        INDEX ix_workflow_definitions_is_default ON workflow_definitions(is_default);
CREATE        INDEX ix_workflow_steps_wf_id         ON workflow_steps(workflow_definition_id);
CREATE UNIQUE INDEX ix_workflow_transitions_steps   ON workflow_transitions(from_step_id, to_step_id);
CREATE        INDEX ix_workflow_transitions_wf_id   ON workflow_transitions(workflow_definition_id);
CREATE UNIQUE INDEX ix_contents_slug                ON contents(slug)                 WHERE is_deleted = false;
CREATE        INDEX ix_contents_status              ON contents(status);
CREATE        INDEX ix_contents_language            ON contents(language);
CREATE        INDEX ix_contents_is_featured         ON contents(is_featured);
CREATE        INDEX ix_contents_published_at        ON contents(published_at);
CREATE        INDEX ix_contents_category_id         ON contents(category_id);
CREATE        INDEX ix_contents_created_at          ON contents(created_at);
CREATE        INDEX ix_contents_status_lang_del     ON contents(status, language, is_deleted);
CREATE        INDEX ix_media_assets_content_id      ON media_assets(content_id);
CREATE UNIQUE INDEX ix_media_assets_storage_key     ON media_assets(storage_key)      WHERE is_deleted = false;
CREATE UNIQUE INDEX ix_localizations_content_lang   ON localizations(content_id, language) WHERE is_deleted = false;
CREATE        INDEX ix_audit_logs_user_id           ON audit_logs(user_id);
CREATE        INDEX ix_audit_logs_action            ON audit_logs(action);
CREATE        INDEX ix_audit_logs_entity_type       ON audit_logs(entity_type);
CREATE        INDEX ix_audit_logs_created_at        ON audit_logs(created_at);
CREATE        INDEX ix_audit_logs_entity_type_id    ON audit_logs(entity_type, entity_id);

-- ============================================================
-- 4. Seed البيانات الأساسية (الأدوار والصلاحيات)
-- ============================================================
INSERT INTO roles (id, name, normalized_name, description, is_system, created_at, is_deleted) VALUES
    ('10000000-0000-0000-0000-000000000001', 'Administrator',    'ADMINISTRATOR',    'Full system access',                  true, now(), false),
    ('10000000-0000-0000-0000-000000000002', 'ContentCreator',   'CONTENTCREATOR',   'Create and edit content',             true, now(), false),
    ('10000000-0000-0000-0000-000000000003', 'Reviewer',         'REVIEWER',         'Review and approve content',          true, now(), false),
    ('10000000-0000-0000-0000-000000000004', 'LanguageReviewer', 'LANGUAGEREVIEWER', 'Review language and translations',    true, now(), false),
    ('10000000-0000-0000-0000-000000000005', 'Designer',         'DESIGNER',         'Manage media assets',                 true, now(), false),
    ('10000000-0000-0000-0000-000000000006', 'Publisher',        'PUBLISHER',         'Publish and schedule content',       true, now(), false),
    ('10000000-0000-0000-0000-000000000007', 'Archivist',        'ARCHIVIST',         'Archive and restore content',        true, now(), false)
ON CONFLICT (id) DO NOTHING;

INSERT INTO permissions (id, name, normalized_name, description, module, created_at, is_deleted) VALUES
    ('20000000-0000-0000-0000-000000000001', 'CreateContent',       'CREATECONTENT',       'CreateContent permission',       'Content',  now(), false),
    ('20000000-0000-0000-0000-000000000002', 'EditContent',         'EDITCONTENT',         'EditContent permission',         'Content',  now(), false),
    ('20000000-0000-0000-0000-000000000003', 'DeleteContent',       'DELETECONTENT',       'DeleteContent permission',       'Content',  now(), false),
    ('20000000-0000-0000-0000-000000000004', 'PublishContent',      'PUBLISHCONTENT',      'PublishContent permission',      'Content',  now(), false),
    ('20000000-0000-0000-0000-000000000005', 'ArchiveContent',      'ARCHIVECONTENT',      'ArchiveContent permission',      'Content',  now(), false),
    ('20000000-0000-0000-0000-000000000006', 'ViewContent',         'VIEWCONTENT',         'ViewContent permission',         'Content',  now(), false),
    ('20000000-0000-0000-0000-000000000007', 'UploadMedia',         'UPLOADMEDIA',         'UploadMedia permission',         'Media',    now(), false),
    ('20000000-0000-0000-0000-000000000008', 'DeleteMedia',         'DELETEMEDIA',         'DeleteMedia permission',         'Media',    now(), false),
    ('20000000-0000-0000-0000-000000000009', 'ManageMedia',         'MANAGEMEDIA',         'ManageMedia permission',         'Media',    now(), false),
    ('20000000-0000-0000-0000-000000000010', 'ApproveReview',       'APPROVEREVIEW',       'ApproveReview permission',       'Workflow', now(), false),
    ('20000000-0000-0000-0000-000000000011', 'RejectReview',        'REJECTREVIEW',        'RejectReview permission',        'Workflow', now(), false),
    ('20000000-0000-0000-0000-000000000012', 'TransitionWorkflow',  'TRANSITIONWORKFLOW',  'TransitionWorkflow permission',  'Workflow', now(), false),
    ('20000000-0000-0000-0000-000000000013', 'ManageUsers',         'MANAGEUSERS',         'ManageUsers permission',         'Admin',    now(), false),
    ('20000000-0000-0000-0000-000000000014', 'ManageRoles',         'MANAGEROLES',         'ManageRoles permission',         'Admin',    now(), false),
    ('20000000-0000-0000-0000-000000000015', 'ViewAuditLogs',       'VIEWAUDITLOGS',       'ViewAuditLogs permission',       'Admin',    now(), false),
    ('20000000-0000-0000-0000-000000000016', 'ManageCategories',    'MANAGECATEGORIES',    'ManageCategories permission',    'Taxonomy', now(), false),
    ('20000000-0000-0000-0000-000000000017', 'ManageTags',          'MANAGETAGS',          'ManageTags permission',          'Taxonomy', now(), false)
ON CONFLICT (id) DO NOTHING;

-- صلاحيات الـ Administrator (كل الصلاحيات)
INSERT INTO role_permissions (role_id, permission_id, granted_at)
SELECT '10000000-0000-0000-0000-000000000001', id, now() FROM permissions
ON CONFLICT DO NOTHING;

-- ContentCreator
INSERT INTO role_permissions (role_id, permission_id, granted_at) VALUES
    ('10000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000001', now()),
    ('10000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002', now()),
    ('10000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000006', now()),
    ('10000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000007', now()),
    ('10000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000009', now())
ON CONFLICT DO NOTHING;

-- Reviewer
INSERT INTO role_permissions (role_id, permission_id, granted_at) VALUES
    ('10000000-0000-0000-0000-000000000003', '20000000-0000-0000-0000-000000000006', now()),
    ('10000000-0000-0000-0000-000000000003', '20000000-0000-0000-0000-000000000010', now()),
    ('10000000-0000-0000-0000-000000000003', '20000000-0000-0000-0000-000000000011', now()),
    ('10000000-0000-0000-0000-000000000003', '20000000-0000-0000-0000-000000000012', now())
ON CONFLICT DO NOTHING;

-- Publisher
INSERT INTO role_permissions (role_id, permission_id, granted_at) VALUES
    ('10000000-0000-0000-0000-000000000006', '20000000-0000-0000-0000-000000000004', now()),
    ('10000000-0000-0000-0000-000000000006', '20000000-0000-0000-0000-000000000006', now()),
    ('10000000-0000-0000-0000-000000000006', '20000000-0000-0000-0000-000000000012', now())
ON CONFLICT DO NOTHING;

-- ============================================================
-- 5. إنشاء مستخدم Admin افتراضي
--    Email: admin@bmedia.io
--    Password: Admin@1234
-- ============================================================
INSERT INTO users (
    id, username, email, password_hash,
    first_name, last_name,
    is_active, is_email_verified,
    preferred_language,
    created_at, is_deleted
) VALUES (
    'a0000000-0000-0000-0000-000000000001',
    'admin',
    'admin@bmedia.io',
    crypt('Admin@1234', gen_salt('bf', 11)),
    'Admin', 'User',
    true, true,
    'ar',
    now(), false
) ON CONFLICT (id) DO NOTHING;

-- ربط المستخدم بدور Administrator
INSERT INTO user_roles (user_id, role_id, assigned_at) VALUES
    ('a0000000-0000-0000-0000-000000000001', '10000000-0000-0000-0000-000000000001', now())
ON CONFLICT DO NOTHING;

-- ============================================================
-- 6. تسجيل الـ Migrations في __EFMigrationsHistory
--    (لمنع EF Core من إعادة تطبيقها)
-- ============================================================
CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId"    varchar(150) NOT NULL PRIMARY KEY,
    "ProductVersion" varchar(32)  NOT NULL
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion") VALUES
    ('20260518000000_InitialCreate',              '9.0.4'),
    ('20260610000001_SeedRolePermissions',         '9.0.4'),
    ('20260611000001_DropRemovedMediaAssetColumns','9.0.4')
ON CONFLICT DO NOTHING;

-- ============================================================
-- تم! يمكنك الآن تشغيل الـ backend وتسجيل الدخول بـ:
-- Email:    admin@bmedia.io
-- Password: Admin@1234
-- ============================================================
SELECT 'Database fixed successfully!' AS status,
       (SELECT count(*) FROM users)   AS users_count,
       (SELECT count(*) FROM roles)   AS roles_count,
       (SELECT count(*) FROM permissions) AS permissions_count;
