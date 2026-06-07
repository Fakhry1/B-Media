-- ============================================================
-- BMedia — Complete Roles, Permissions & Admin User Setup
-- يعمل مع الجداول الموجودة بأسماء PascalCase
--
-- !!! استبدل <PASTE_HASH_HERE> بالـ BCrypt hash من:
--     dotnet run --project tools/GenerateHash
-- كلمة المرور: Admin@123
-- ============================================================

BEGIN;

-- ============================================================
-- 1. ROLES (7 أدوار)
-- ============================================================

INSERT INTO "Roles" ("Id", "Name", "NormalizedName", "Description", "IsSystem", "CreatedAt", "IsDeleted")
VALUES
  ('10000000-0000-0000-0000-000000000001', 'Administrator',    'ADMINISTRATOR',    'وصول كامل للنظام',                 true,  NOW(), false),
  ('10000000-0000-0000-0000-000000000002', 'ContentCreator',   'CONTENTCREATOR',   'إنشاء وتعديل المحتوى',             true,  NOW(), false),
  ('10000000-0000-0000-0000-000000000003', 'Reviewer',         'REVIEWER',         'مراجعة واعتماد المحتوى',           true,  NOW(), false),
  ('10000000-0000-0000-0000-000000000004', 'LanguageReviewer', 'LANGUAGEREVIEWER', 'مراجعة اللغة والترجمات',           true,  NOW(), false),
  ('10000000-0000-0000-0000-000000000005', 'Designer',         'DESIGNER',         'إدارة الوسائط والتصميم',           true,  NOW(), false),
  ('10000000-0000-0000-0000-000000000006', 'Publisher',        'PUBLISHER',        'نشر وجدولة المحتوى',               true,  NOW(), false),
  ('10000000-0000-0000-0000-000000000007', 'Archivist',        'ARCHIVIST',        'أرشفة المحتوى واسترجاعه',          true,  NOW(), false)
ON CONFLICT ("Id") DO NOTHING;

-- ============================================================
-- 2. PERMISSIONS (17 صلاحية)
-- ============================================================

INSERT INTO "Permissions" ("Id", "Name", "NormalizedName", "Description", "Module", "CreatedAt", "IsDeleted")
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
ON CONFLICT ("Id") DO NOTHING;

-- ============================================================
-- 3. ROLE PERMISSIONS
-- ============================================================

-- Administrator ← كل الصلاحيات
INSERT INTO "RolePermissions" ("RoleId", "PermissionId", "GrantedAt")
SELECT '10000000-0000-0000-0000-000000000001', "Id", NOW()
FROM   "Permissions" WHERE "IsDeleted" = false
ON CONFLICT ("RoleId", "PermissionId") DO NOTHING;

-- ContentCreator ← إنشاء / تعديل / عرض + وسائط
INSERT INTO "RolePermissions" ("RoleId", "PermissionId", "GrantedAt")
VALUES
  ('10000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000001', NOW()),
  ('10000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002', NOW()),
  ('10000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000006', NOW()),
  ('10000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000007', NOW()),
  ('10000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000009', NOW())
ON CONFLICT ("RoleId", "PermissionId") DO NOTHING;

-- Reviewer ← مراجعة / اعتماد / رفض
INSERT INTO "RolePermissions" ("RoleId", "PermissionId", "GrantedAt")
VALUES
  ('10000000-0000-0000-0000-000000000003', '20000000-0000-0000-0000-000000000006', NOW()),
  ('10000000-0000-0000-0000-000000000003', '20000000-0000-0000-0000-000000000010', NOW()),
  ('10000000-0000-0000-0000-000000000003', '20000000-0000-0000-0000-000000000011', NOW()),
  ('10000000-0000-0000-0000-000000000003', '20000000-0000-0000-0000-000000000012', NOW())
ON CONFLICT ("RoleId", "PermissionId") DO NOTHING;

-- LanguageReviewer ← عرض / تعديل
INSERT INTO "RolePermissions" ("RoleId", "PermissionId", "GrantedAt")
VALUES
  ('10000000-0000-0000-0000-000000000004', '20000000-0000-0000-0000-000000000006', NOW()),
  ('10000000-0000-0000-0000-000000000004', '20000000-0000-0000-0000-000000000002', NOW())
ON CONFLICT ("RoleId", "PermissionId") DO NOTHING;

-- Designer ← وسائط كاملة
INSERT INTO "RolePermissions" ("RoleId", "PermissionId", "GrantedAt")
VALUES
  ('10000000-0000-0000-0000-000000000005', '20000000-0000-0000-0000-000000000006', NOW()),
  ('10000000-0000-0000-0000-000000000005', '20000000-0000-0000-0000-000000000007', NOW()),
  ('10000000-0000-0000-0000-000000000005', '20000000-0000-0000-0000-000000000008', NOW()),
  ('10000000-0000-0000-0000-000000000005', '20000000-0000-0000-0000-000000000009', NOW())
ON CONFLICT ("RoleId", "PermissionId") DO NOTHING;

-- Publisher ← نشر / جدولة
INSERT INTO "RolePermissions" ("RoleId", "PermissionId", "GrantedAt")
VALUES
  ('10000000-0000-0000-0000-000000000006', '20000000-0000-0000-0000-000000000004', NOW()),
  ('10000000-0000-0000-0000-000000000006', '20000000-0000-0000-0000-000000000006', NOW()),
  ('10000000-0000-0000-0000-000000000006', '20000000-0000-0000-0000-000000000012', NOW())
ON CONFLICT ("RoleId", "PermissionId") DO NOTHING;

-- Archivist ← أرشفة
INSERT INTO "RolePermissions" ("RoleId", "PermissionId", "GrantedAt")
VALUES
  ('10000000-0000-0000-0000-000000000007', '20000000-0000-0000-0000-000000000005', NOW()),
  ('10000000-0000-0000-0000-000000000007', '20000000-0000-0000-0000-000000000006', NOW())
ON CONFLICT ("RoleId", "PermissionId") DO NOTHING;

-- ============================================================
-- 4. USER — مستخدم admin
-- !!! استبدل <PASTE_HASH_HERE> بالـ hash من GenerateHash
-- ============================================================

INSERT INTO "Users" (
    "Id", "Username", "Email", "PasswordHash",
    "FirstName", "LastName",
    "IsActive", "IsEmailVerified", "FailedLoginAttempts",
    "PreferredLanguage", "CreatedAt", "IsDeleted"
)
VALUES (
    'A0000000-0000-0000-0000-000000000001',
    'admin',
    'admin@bmedia.io',
    '<PASTE_HASH_HERE>',
    'System', 'Admin',
    true, true, 0,
    'en', NOW(), false
)
ON CONFLICT ("Id") DO NOTHING;

-- ============================================================
-- 5. USER ROLE — تعيين دور Administrator
-- ============================================================

INSERT INTO "UserRoles" ("UserId", "RoleId", "AssignedAt")
VALUES (
    'A0000000-0000-0000-0000-000000000001',
    '10000000-0000-0000-0000-000000000001',
    NOW()
)
ON CONFLICT ("UserId", "RoleId") DO NOTHING;

COMMIT;

-- ============================================================
-- تحقق من النتيجة
-- ============================================================
SELECT
    u."Username",
    u."Email",
    u."IsActive",
    r."Name"                      AS "Role",
    COUNT(rp."PermissionId")      AS "PermissionsCount"
FROM  "Users"           u
JOIN  "UserRoles"       ur ON ur."UserId"      = u."Id"
JOIN  "Roles"           r  ON r."Id"           = ur."RoleId"
JOIN  "RolePermissions" rp ON rp."RoleId"      = r."Id"
WHERE u."Username"   = 'admin'
  AND u."IsDeleted"  = false
GROUP BY u."Username", u."Email", u."IsActive", r."Name";
