-- Add preferred_language column to user table
ALTER TABLE public.user
    ADD COLUMN preferred_language language NOT NULL DEFAULT 'ua'::language;

-- Create index for better query performance when filtering by language
CREATE INDEX idx_user_preferred_language ON public.user(preferred_language);