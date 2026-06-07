-- ============================================================
-- BMedia — Complete Roles, Permissions & Admin User Setup
-- يعمل مع الجداول snake_case (بعد تشغيل BMedia_Database_Setup.sql)
--
-- كلمة مرور المستخدم admin: Admin@123
-- الـ Hash محسوب مسبقاً بـ BCrypt workFactor=12
-- ============================================================

BEGIN;

-- ============================================================
-- 1. ROLES (7 أدوار)
-- ============================================================

INSERT INTO roles (id, name, normalized_name, description, is_system, created_at, is_deleted)
VALUES
  ('10000000-0000-0000-0000-000000000001', 'Administrator',    'ADMINISTRATOR',    'وصول كامل للنظام',                 true,  NOW(), false),
  ('10000000-0000-0000-0000-000000000002', 'ContentCreator',   'CONTENTCREATOR',   'إنشاء وتعديل المحتوى',             true,  NOW(), false),
  ('10000000-0000-0000-0000-000000000003', 'Reviewer',         'REVIEWER',         'مراجعة واعتماد المحتوى',           true,  NOW(), false),
  ('10000000-0000-0000-0000-000000000004', 'LanguageReviewer', 'LANGUAGEREVIEWER', 'مراجعة اللغة والترجمات',           true,  NOW(), false),
  ('10000000-0000-0000-0000-000000000005', 'Designer',         'DESIGNER',         'إدارة الوسائط والتصميم',           true,  NOW(), false),
  ('10000000-0000-0000-0000-000000000006', 'Publisher',        'PUBLISHER',        'نشر وجدولة المحتوى',               true,  NOW(), false),
  ('10000000-0000-0000-0000-000000000007', 'Archivist',        'ARCHIVIST',        'أرشفة المحتوى واسترجاعه',          true,  NOW(), false)
ON CONFLICT (id) DO NOTHING;

-- ============================================================
-- 2. PERMISSIONS (17 صلاحية)
-- ============================================================

INSERT INTO permissions (id, name, normalized_name, description, module, created_at, is_deleted)
VALUES
  ('20000000-0000-0000-0000-000000000001', 'CreateContent',      'CREATECONTENT',      'إنشاء محتوى جديد',         'Content',  NOW(), false),
  ('20000000-0000-0000-0000-000000000002', 'EditContent',        'EDITCONTENT',        'تعديل المحتوى',            'Content',  NOW(), false),
  ('20000000-0000-0000-0000-000000000003', 'DeleteContent',      'DELETECONTENT',      'حذف المحتوى',              'Content',  NOW(), false),
  ('20000000-0000-0000-0000-000000000004', 'PublishContent',     'PUBLISHCONTENT',     'نشر المحتوى',              'Content',  NOW(), false),
  ('20000000-0000-0000-0000-000000000005', 'ArchiveContent',     'ARCHIVECONTENT',     'أرشفة المحتوى',            'Content',  NOW(), false),
  ('20000000-0000-0000-0000-000000000006', 'ViewContent',        'VIEWCONTENT',        'عرض المحتوى',              'Content',  NOW(), false),
  ('20000000-0000-0000-0000-000000000007', 'UploadMedia',        'UPLOADMEDIA',        'رفع الوسائط',              'Media',    NOW(), false),
  ('20000000-0000-0000-0000-000000000008', 'DeleteMedia',        'DELETEMEDIA',        'حذف الوسائط',              'Media',    NOW(), false),
  ('20000000-0000-0000-0000-000000000009', 'ManageMedia',        'MANAGEMEDIA',        'إدارة الوسائط',            'Media',    NOW(), false),
  ('20000000-0000-0000-0000-000000000010', 'ApproveReview',      'APPROVEREVIEW',      'اعتماد المراجعة',          'Workflow', NOW(), false),
  ('20000000-0000-0000-0000-000000000011', 'RejectReview',       'REJECTREVIEW',       'رفض المراجعة',             'Workflow', NOW(), false),
  ('20000000-0000-0000-0000-000000000012', 'TransitionWorkflow', 'TRANSITIONWORKFLOW', 'تحريك سير العمل',          'Workflow', NOW(), false),
  ('20000000-0000-0000-0000-000000000013', 'ManageUsers',        'MANAGEUSERS',        'إدارة المستخدمين',         'Admin',    NOW(), false),
  ('20000000-0000-0000-0000-000000000014', 'ManageRoles',        'MANAGEROLES',        'إدارة الأدوار',            'Admin',    NOW(), false),
  ('20000000-0000-0000-0000-000000000015', 'ViewAuditLogs',      'VIEWAUDITLOGS',      'عرض سجلات التدقيق',        'Admin',    NOW(), false),
  ('20000000-0000-0000-0000-000000000016', 'ManageCategories',   'MANAGECATEGORIES',   'إدارة التصنيفات',          'Taxonomy', NOW(), false),
  ('20000000-0000-0000-0000-000000000017', 'ManageTags',         'MANAGETAGS',         'إدارة الوسوم',             'Taxonomy', NOW(), false)
ON CONFLICT (id) DO NOTHING;

-- ============================================================
-- 3. ROLE PERMISSIONS
-- ============================================================

-- Administrator ← كل الصلاحيات
INSERT INTO role_permissions (role_id, permission_id, granted_at)
SELECT '10000000-0000-0000-0000-000000000001', id, NOW()
FROM   permissions WHERE is_deleted = false
ON CONFLICT (role_id, permission_id) DO NOTHING;

-- ContentCreator ← إنشاء / تعديل / عرض + وسائط
INSERT INTO role_permissions (role_id, permission_id, granted_at)
VALUES
  ('10000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000001', NOW()),
  ('10000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002', NOW()),
  ('10000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000006', NOW()),
  ('10000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000007', NOW()),
  ('10000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000009', NOW())
ON CONFLICT (role_id, permission_id) DO NOTHING;

-- Reviewer ← مراجعة / اعتماد / رفض
INSERT INTO role_permissions (role_id, permission_id, granted_at)
VALUES
  ('10000000-0000-0000-0000-000000000003', '20000000-0000-0000-0000-000000000006', NOW()),
  ('10000000-0000-0000-0000-000000000003', '20000000-0000-0000-0000-000000000010', NOW()),
  ('10000000-0000-0000-0000-000000000003', '20000000-0000-0000-0000-000000000011', NOW()),
  ('10000000-0000-0000-0000-000000000003', '20000000-0000-0000-0000-000000000012', NOW())
ON CONFLICT (role_id, permission_id) DO NOTHING;

-- LanguageReviewer ← عرض / تعديل
INSERT INTO role_permissions (role_id, permission_id, granted_at)
VALUES
  ('10000000-0000-0000-0000-000000000004', '20000000-0000-0000-0000-000000000006', NOW()),
  ('10000000-0000-0000-0000-000000000004', '20000000-0000-0000-0000-000000000002', NOW())
ON CONFLICT (role_id, permission_id) DO NOTHING;

-- Designer ← وسائط كاملة
INSERT INTO role_permissions (role_id, permission_id, granted_at)
VALUES
  ('10000000-0000-0000-0000-000000000005', '20000000-0000-0000-0000-000000000006', NOW()),
  ('10000000-0000-0000-0000-000000000005', '20000000-0000-0000-0000-000000000007', NOW()),
  ('10000000-0000-0000-0000-000000000005', '20000000-0000-0000-0000-000000000008', NOW()),
  ('10000000-0000-0000-0000-000000000005', '20000000-0000-0000-0000-000000000009', NOW())
ON CONFLICT (role_id, permission_id) DO NOTHING;

-- Publisher ← نشر / جدولة
INSERT INTO role_permissions (role_id, permission_id, granted_at)
VALUES
  ('10000000-0000-0000-0000-000000000006', '20000000-0000-0000-0000-000000000004', NOW()),
  ('10000000-0000-0000-0000-000000000006', '20000000-0000-0000-0000-000000000006', NOW()),
  ('10000000-0000-0000-0000-000000000006', '20000000-0000-0000-0000-000000000012', NOW())
ON CONFLICT (role_id, permission_id) DO NOTHING;

-- Archivist ← أرشفة
INSERT INTO role_permissions (role_id, permission_id, granted_at)
VALUES
  ('10000000-0000-0000-0000-000000000007', '20000000-0000-0000-0000-000000000005', NOW()),
  ('10000000-0000-0000-0000-000000000007', '20000000-0000-0000-0000-000000000006', NOW())
ON CONFLICT (role_id, permission_id) DO NOTHING;

-- ============================================================
-- 4. USER — مستخدم admin
-- كلمة المرور: Admin@123  (BCrypt workFactor=12)
-- ============================================================

INSERT INTO users (
    id, username, email, password_hash,
    first_name, last_name,
    is_active, is_email_verified, failed_login_attempts,
    preferred_language, created_at, is_deleted
)
VALUES (
    'a0000000-0000-0000-0000-000000000001',
    'admin',
    'admin@bmedia.io',
    '$2b$12$UHyZk9cezf5lVvo4VRBUSuQPwsAKnbQE3y5E/OuizqRva0yjsrE/G',
    'System', 'Admin',
    true, true, 0,
    'en', NOW(), false
)
ON CONFLICT (id) DO NOTHING;

-- ============================================================
-- 5. USER ROLE — تعيين دور Administrator
-- ============================================================

INSERT INTO user_roles (user_id, role_id, assigned_at)
VALUES (
    'a0000000-0000-0000-0000-000000000001',
    '10000000-0000-0000-0000-000000000001',
    NOW()
)
ON CONFLICT (user_id, role_id) DO NOTHING;

COMMIT;

-- ============================================================
-- تحقق من النتيجة
-- ============================================================
SELECT
    u.username,
    u.email,
    u.is_active,
    r.name                    AS role,
    COUNT(rp.permission_id)   AS permissions_count
FROM  users            u
JOIN  user_roles       ur ON ur.user_id      = u.id
JOIN  roles            r  ON r.id            = ur.role_id
JOIN  role_permissions rp ON rp.role_id      = r.id
WHERE u.username   = 'admin'
  AND u.is_deleted = false
GROUP BY u.username, u.email, u.is_active, r.name;
