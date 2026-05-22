-- ============================================================
-- V9: Todo feature + Systematization character stat
-- ============================================================

-- 1. Add systematization stat column to character_stats
ALTER TABLE public.character_stats
    ADD COLUMN systematization integer NOT NULL DEFAULT 0;

-- 2. Active todo items for the current day
CREATE TABLE public.todo_item (
    uuid         uuid      NOT NULL PRIMARY KEY DEFAULT uuid_generate_v4(),
    user_uuid    uuid      NOT NULL REFERENCES public.user(uuid) ON DELETE CASCADE,
    title        text      NOT NULL,
    is_completed boolean   NOT NULL DEFAULT false,
    created_at   timestamp NOT NULL DEFAULT now(),
    completed_at timestamp,
    day_date     date      NOT NULL DEFAULT CURRENT_DATE
);

CREATE INDEX idx_todo_item_user_date ON public.todo_item (user_uuid, day_date);

-- 3. One summary row per finished day (archived when user clicks "Finish Day")
CREATE TABLE public.todo_history (
    uuid               uuid      NOT NULL PRIMARY KEY DEFAULT uuid_generate_v4(),
    user_uuid          uuid      NOT NULL REFERENCES public.user(uuid) ON DELETE CASCADE,
    day_date           date      NOT NULL,
    finished_at        timestamp NOT NULL DEFAULT now(),
    completed_count    integer   NOT NULL DEFAULT 0,
    total_count        integer   NOT NULL DEFAULT 0,
    experience_awarded integer   NOT NULL DEFAULT 0
);

CREATE INDEX idx_todo_history_user ON public.todo_history (user_uuid, finished_at DESC);
