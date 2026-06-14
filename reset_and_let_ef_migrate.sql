-- ============================================================
--  BMedia — Drop All & Let EF Core Recreate
--  يحذف كل الجداول ويمسح سجل migrations
--  بعد تشغيله: أعد تشغيل الـ backend وسيُنشئ EF Core كل شيء صحيحاً
-- ============================================================

-- 1. حذف الجداول بالترتيب الصحيح (FK أولاً)
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

-- 2. حذف سجل EF Core migrations حتى يعيد تطبيقها من الصفر
DROP TABLE IF EXISTS "__EFMigrationsHistory";

-- ✅ الآن أعد تشغيل الـ backend — سيُنشئ EF Core كل الجداول تلقائياً
--    بالمخطط الصحيح من ملف 20260518000000_InitialCreate.cs
