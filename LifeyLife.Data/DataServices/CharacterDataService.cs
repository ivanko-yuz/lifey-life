using LifeyLife.Core.Contracts;
using LifeyLife.Core.Models;

namespace LifeyLife.Data.DataServices;

public class CharacterDataService : ICharacterDataService
{
    private readonly IDbAdapter _dbAdapter;

    public CharacterDataService(IDbAdapter dbAdapter)
    {
        _dbAdapter = dbAdapter;
    }

    public async Task<CharacterStats> GetOrCreateStats(Guid userUuid)
    {
        const string selectSql = $@"
            SELECT
                user_uuid        AS {nameof(CharacterStats.UserUuid)},
                strength         AS {nameof(CharacterStats.Strength)},
                intelligence     AS {nameof(CharacterStats.Intelligence)},
                charisma         AS {nameof(CharacterStats.Charisma)},
                dexterity        AS {nameof(CharacterStats.Dexterity)},
                vitality         AS {nameof(CharacterStats.Vitality)},
                willpower        AS {nameof(CharacterStats.Willpower)},
                systematization  AS {nameof(CharacterStats.Systematization)},
                total_experience AS {nameof(CharacterStats.TotalExperience)}
            FROM public.character_stats
            WHERE user_uuid = @UserUuid";

        var existing = await _dbAdapter.GetSingleOrDefault<CharacterStats>(
            selectSql, new { UserUuid = userUuid });

        if (existing is not null)
            return existing;

        // Lazily create the row for this user
        const string insertSql = @"
            INSERT INTO public.character_stats (user_uuid)
            VALUES (@UserUuid)
            ON CONFLICT (user_uuid) DO NOTHING";

        await _dbAdapter.ExecuteCommand(insertSql, new { UserUuid = userUuid });

        return await _dbAdapter.GetSingleOrDefault<CharacterStats>(
                   selectSql, new { UserUuid = userUuid })
               ?? new CharacterStats { UserUuid = userUuid };
    }

    public async Task AwardStatPoints(Guid userUuid, DareCategory category, int points)
    {
        // Ensure the row exists (idempotent insert)
        const string ensureRowSql = @"
            INSERT INTO public.character_stats (user_uuid)
            VALUES (@UserUuid)
            ON CONFLICT (user_uuid) DO NOTHING";

        await _dbAdapter.ExecuteCommand(ensureRowSql, new { UserUuid = userUuid });

        // Map category → column name.
        // statColumn is derived from a closed enum switch — not from user input.
        var statColumn = category switch
        {
            DareCategory.physical   => "strength",
            DareCategory.social     => "charisma",
            DareCategory.mental     => "intelligence",
            DareCategory.creative   => "dexterity",
            DareCategory.wellness   => "vitality",
            DareCategory.discipline => "willpower",
            _                       => "willpower"
        };

        var updateSql = $@"
            UPDATE public.character_stats
            SET {statColumn}        = {statColumn} + @Points,
                total_experience    = total_experience + @Points
            WHERE user_uuid = @UserUuid";

        await _dbAdapter.ExecuteCommand(updateSql, new { UserUuid = userUuid, Points = points });
    }

    public async Task AwardSystematizationPoints(Guid userUuid, int points)
    {
        // Ensure the row exists (idempotent insert)
        const string ensureRowSql = @"
            INSERT INTO public.character_stats (user_uuid)
            VALUES (@UserUuid)
            ON CONFLICT (user_uuid) DO NOTHING";

        await _dbAdapter.ExecuteCommand(ensureRowSql, new { UserUuid = userUuid });

        const string updateSql = @"
            UPDATE public.character_stats
            SET systematization  = systematization + @Points,
                total_experience = total_experience + @Points
            WHERE user_uuid = @UserUuid";

        await _dbAdapter.ExecuteCommand(updateSql, new { UserUuid = userUuid, Points = points });
    }
}
