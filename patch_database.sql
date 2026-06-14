-- ============================================================
--  BMedia Database Patch Script
--  يضيف الأعمدة المفقودة إلى الجداول الموجودة دون حذف البيانات
--  Run against: bmedia_db
-- ============================================================

-- ===================== contents =====================
-- إضافة الأعمدة المفقودة
ALTER TABLE contents
    ADD COLUMN IF NOT EXISTS body                  text,
    ADD COLUMN IF NOT EXISTS seo_title             varchar(300),
    ADD COLUMN IF NOT EXISTS seo_description       varchar(500),
    ADD COLUMN IF NOT EXISTS seo_keywords          varchar(500),
    ADD COLUMN IF NOT EXISTS canonical_url         varchar(2048),
    ADD COLUMN IF NOT EXISTS published_by          uuid,
    ADD COLUMN IF NOT EXISTS archived_at           timestamptz,
    ADD COLUMN IF NOT EXISTS current_workflow_step_id uuid,
    ADD COLUMN IF NOT EXISTS assigned_reviewer_id  uuid;

-- حذف العمود الخاطئ إن وُجد
ALTER TABLE contents DROP COLUMN IF EXISTS current_workflow_step;

-- إضافة FK لـ current_workflow_step_id إن لم تكن موجودة
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM information_schema.table_constraints
        WHERE constraint_name = 'fk_contents_workflow_steps_current_workflow_step_id'
    ) THEN
        ALTER TABLE contents
            ADD CONSTRAINT fk_contents_workflow_steps_current_workflow_step_id
            FOREIGN KEY (current_workflow_step_id)
            REFERENCES workflow_steps(id)
            ON DELETE SET NULL;
    END IF;
END $$;

-- ===================== content_tags =====================
ALTER TABLE content_tags
    ADD COLUMN IF NOT EXISTS tagged_at      timestamptz NOT NULL DEFAULT now(),
    ADD COLUMN IF NOT EXISTS is_ai_generated boolean    NOT NULL DEFAULT false;

-- ===================== تحقق =====================
SELECT
    column_name,
    data_type
FROM information_schema.columns
WHERE table_name = 'contents'
ORDER BY ordinal_position;
