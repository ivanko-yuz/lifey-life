-- ============================================================
-- V6: Character stats system
-- ============================================================

-- 1. New category enum for dares
CREATE TYPE dare_category AS ENUM (
    'physical',
    'social',
    'mental',
    'creative',
    'wellness',
    'discipline'
);

-- 2. Add category column to random_dare (default = physical)
ALTER TABLE public.random_dare
    ADD COLUMN category dare_category NOT NULL DEFAULT 'physical'::dare_category;

-- 3. Back-fill categories for existing seed dares
UPDATE public.random_dare
SET category = 'social'::dare_category
WHERE context IN (
    'Meet 3 strangers',
    'Познайомтесь з 3-ма незнайомцями',
    'Have a beer with a stranger',
    'Випийте пива з незнайомцем'
);

UPDATE public.random_dare
SET category = 'wellness'::dare_category
WHERE context IN (
    'Eat only healthy food for a one day',
    'Їжте лише здорову їжу протягом одного дня'
);

-- physical is already the default so workout/run dares need no update

-- 4. Character stats table (one row per user, created lazily on first use)
CREATE TABLE public.character_stats (
    user_uuid        uuid    NOT NULL PRIMARY KEY
        REFERENCES public.user(uuid) ON DELETE CASCADE,
    strength         integer NOT NULL DEFAULT 0,
    intelligence     integer NOT NULL DEFAULT 0,
    charisma         integer NOT NULL DEFAULT 0,
    dexterity        integer NOT NULL DEFAULT 0,
    vitality         integer NOT NULL DEFAULT 0,
    willpower        integer NOT NULL DEFAULT 0,
    total_experience integer NOT NULL DEFAULT 0
);
