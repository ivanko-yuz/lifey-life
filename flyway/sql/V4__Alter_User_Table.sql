-- Drop unnecessary columns from the user table
ALTER TABLE public.user
    DROP COLUMN IF EXISTS default_language,
    DROP COLUMN IF EXISTS first_name,
    DROP COLUMN IF EXISTS last_name,
    DROP COLUMN IF EXISTS user_name,
    DROP COLUMN IF EXISTS current_level,
    DROP COLUMN IF EXISTS current_experience;

-- Modify email column to be VARCHAR(255)
ALTER TABLE public.user
    ALTER COLUMN email TYPE VARCHAR(255);

-- Add unique constraint to email if it doesn't exist
DO $$ 
BEGIN
    IF NOT EXISTS (
        SELECT 1 
        FROM pg_constraint 
        WHERE conname = 'user_email_key'
    ) THEN
        ALTER TABLE public.user
            ADD CONSTRAINT user_email_key UNIQUE (email);
    END IF;
END $$; 