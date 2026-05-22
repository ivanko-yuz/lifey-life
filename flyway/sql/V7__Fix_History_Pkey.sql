-- The composite PK (user_uuid, random_dares_uuid) prevents a user from completing
-- the same dare more than once.  Replace it with a surrogate UUID key so the same
-- dare can be logged multiple times (one row per completion attempt).

ALTER TABLE public.random_dare_history
    DROP CONSTRAINT random_dare_history_pkey;

ALTER TABLE public.random_dare_history
    ADD COLUMN uuid uuid NOT NULL DEFAULT uuid_generate_v4();

ALTER TABLE public.random_dare_history
    ADD PRIMARY KEY (uuid);

-- Keep a non-unique index on the join columns that are used in queries / history list.
CREATE INDEX idx_history_user_dare
    ON public.random_dare_history (user_uuid, random_dares_uuid);
