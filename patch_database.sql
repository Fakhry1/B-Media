-- ============================================================
--  BMedia Database Patch Script v2
--  يضيف/يصحح الأعمدة المفقودة دون حذف البيانات
--  Run against: bmedia_db
-- ============================================================

-- ===================== contents =====================
ALTER TABLE contents
    ADD COLUMN IF NOT EXISTS body                     text,
    ADD COLUMN IF NOT EXISTS seo_title                varchar(300),
    ADD COLUMN IF NOT EXISTS seo_description          varchar(500),
    ADD COLUMN IF NOT EXISTS seo_keywords             varchar(500),
    ADD COLUMN IF NOT EXISTS canonical_url            varchar(2048),
    ADD COLUMN IF NOT EXISTS published_by             uuid,
    ADD COLUMN IF NOT EXISTS archived_at              timestamptz,
    ADD COLUMN IF NOT EXISTS current_workflow_step_id uuid,
    ADD COLUMN IF NOT EXISTS assigned_reviewer_id     uuid;

-- حذف العمود الخاطئ
ALTER TABLE contents DROP COLUMN IF EXISTS current_workflow_step;

DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM information_schema.table_constraints
        WHERE constraint_name = 'fk_contents_workflow_steps_current_workflow_step_id'
    ) THEN
        ALTER TABLE contents
            ADD CONSTRAINT fk_contents_workflow_steps_current_workflow_step_id
            FOREIGN KEY (current_workflow_step_id)
            REFERENCES workflow_steps(id) ON DELETE SET NULL;
    END IF;
END $$;

-- ===================== content_tags =====================
ALTER TABLE content_tags
    ADD COLUMN IF NOT EXISTS tagged_at       timestamptz NOT NULL DEFAULT now(),
    ADD COLUMN IF NOT EXISTS is_ai_generated boolean     NOT NULL DEFAULT false;

-- ===================== localizations =====================
ALTER TABLE localizations
    ADD COLUMN IF NOT EXISTS seo_title       varchar(300),
    ADD COLUMN IF NOT EXISTS seo_description varchar(500),
    ADD COLUMN IF NOT EXISTS is_approved     boolean     NOT NULL DEFAULT false,
    ADD COLUMN IF NOT EXISTS approved_at     timestamptz,
    ADD COLUMN IF NOT EXISTS approved_by     uuid;

-- ===================== review_comments =====================
-- إعادة تسمية user_id إلى reviewer_id إن وجدت
DO $$
BEGIN
    IF EXISTS (
        SELECT 1 FROM information_schema.columns
        WHERE table_name = 'review_comments' AND column_name = 'user_id'
    ) AND NOT EXISTS (
        SELECT 1 FROM information_schema.columns
        WHERE table_name = 'review_comments' AND column_name = 'reviewer_id'
    ) THEN
        ALTER TABLE review_comments RENAME COLUMN user_id TO reviewer_id;
    END IF;
END $$;

ALTER TABLE review_comments
    ADD COLUMN IF NOT EXISTS reviewer_id       uuid,
    ADD COLUMN IF NOT EXISTS is_resolved       boolean     NOT NULL DEFAULT false,
    ADD COLUMN IF NOT EXISTS resolved_at       timestamptz,
    ADD COLUMN IF NOT EXISTS resolved_by       uuid,
    ADD COLUMN IF NOT EXISTS parent_comment_id uuid;

ALTER TABLE review_comments DROP COLUMN IF EXISTS step_name;

DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM information_schema.table_constraints
        WHERE constraint_name = 'fk_review_comments_users_reviewer_id'
    ) THEN
        ALTER TABLE review_comments
            ADD CONSTRAINT fk_review_comments_users_reviewer_id
            FOREIGN KEY (reviewer_id) REFERENCES users(id) ON DELETE RESTRICT;
    END IF;
    IF NOT EXISTS (
        SELECT 1 FROM information_schema.table_constraints
        WHERE constraint_name = 'fk_review_comments_review_comments_parent_comment_id'
    ) THEN
        ALTER TABLE review_comments
            ADD CONSTRAINT fk_review_comments_review_comments_parent_comment_id
            FOREIGN KEY (parent_comment_id) REFERENCES review_comments(id) ON DELETE RESTRICT;
    END IF;
END $$;

-- ===================== content_workflow_histories =====================
-- إعادة تسمية الأعمدة الخاطئة
DO $$
BEGIN
    -- user_id → transitioned_by_id
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='content_workflow_histories' AND column_name='user_id')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='content_workflow_histories' AND column_name='transitioned_by_id')
    THEN
        ALTER TABLE content_workflow_histories RENAME COLUMN user_id TO transitioned_by_id;
    END IF;
    -- from_step → rename to from_step_name temporarily, then add uuid columns
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='content_workflow_histories' AND column_name='from_step') THEN
        ALTER TABLE content_workflow_histories DROP COLUMN from_step;
    END IF;
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='content_workflow_histories' AND column_name='to_step') THEN
        ALTER TABLE content_workflow_histories DROP COLUMN to_step;
    END IF;
    -- transition_date → transitioned_at
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='content_workflow_histories' AND column_name='transition_date')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='content_workflow_histories' AND column_name='transitioned_at')
    THEN
        ALTER TABLE content_workflow_histories RENAME COLUMN transition_date TO transitioned_at;
    END IF;
    -- action → action_name
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='content_workflow_histories' AND column_name='action')
       AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='content_workflow_histories' AND column_name='action_name')
    THEN
        ALTER TABLE content_workflow_histories RENAME COLUMN action TO action_name;
    END IF;
END $$;

ALTER TABLE content_workflow_histories
    ADD COLUMN IF NOT EXISTS from_step_id      uuid,
    ADD COLUMN IF NOT EXISTS to_step_id        uuid,
    ADD COLUMN IF NOT EXISTS transitioned_by_id uuid,
    ADD COLUMN IF NOT EXISTS from_status        integer NOT NULL DEFAULT 0,
    ADD COLUMN IF NOT EXISTS to_status          integer NOT NULL DEFAULT 0,
    ADD COLUMN IF NOT EXISTS action_name        varchar(200),
    ADD COLUMN IF NOT EXISTS transitioned_at    timestamptz NOT NULL DEFAULT now();

DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM information_schema.table_constraints
        WHERE constraint_name = 'fk_content_workflow_histories_users_transitioned_by_id'
    ) THEN
        ALTER TABLE content_workflow_histories
            ADD CONSTRAINT fk_content_workflow_histories_users_transitioned_by_id
            FOREIGN KEY (transitioned_by_id) REFERENCES users(id) ON DELETE RESTRICT;
    END IF;
    IF NOT EXISTS (
        SELECT 1 FROM information_schema.table_constraints
        WHERE constraint_name = 'fk_content_workflow_histories_workflow_steps_from_step_id'
    ) THEN
        ALTER TABLE content_workflow_histories
            ADD CONSTRAINT fk_content_workflow_histories_workflow_steps_from_step_id
            FOREIGN KEY (from_step_id) REFERENCES workflow_steps(id) ON DELETE SET NULL;
    END IF;
    IF NOT EXISTS (
        SELECT 1 FROM information_schema.table_constraints
        WHERE constraint_name = 'fk_content_workflow_histories_workflow_steps_to_step_id'
    ) THEN
        ALTER TABLE content_workflow_histories
            ADD CONSTRAINT fk_content_workflow_histories_workflow_steps_to_step_id
            FOREIGN KEY (to_step_id) REFERENCES workflow_steps(id) ON DELETE RESTRICT;
    END IF;
END $$;

-- ===================== تحقق =====================
SELECT table_name, column_name, data_type
FROM information_schema.columns
WHERE table_name IN ('contents','localizations','review_comments','content_workflow_histories')
ORDER BY table_name, ordinal_position;
