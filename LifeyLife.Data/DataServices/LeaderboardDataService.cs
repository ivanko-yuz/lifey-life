using LifeyLife.Core.Contracts;
using LifeyLife.Core.Models;

namespace LifeyLife.Data.DataServices;

public class LeaderboardDataService : ILeaderboardDataService
{
    private readonly IDbAdapter _dbAdapter;

    public LeaderboardDataService(IDbAdapter dbAdapter)
    {
        _dbAdapter = dbAdapter;
    }

    public async Task<List<LeaderboardEntry>> GetTopPlayers(int limit = 50)
    {
        const string query = @"
            SELECT
                u.email              AS Email,
                cs.total_experience  AS TotalExperience,
                cs.strength          AS Strength,
                cs.intelligence      AS Intelligence,
                cs.charisma          AS Charisma,
                cs.dexterity         AS Dexterity,
                cs.vitality          AS Vitality,
                cs.willpower         AS Willpower,
                cs.systematization   AS Systematization
            FROM public.character_stats cs
            JOIN public.user u ON u.uuid = cs.user_uuid
            ORDER BY cs.total_experience DESC
            LIMIT @Limit";

        var rows = await _dbAdapter.GetMany<LeaderboardRow>(query, new { Limit = limit });

        return rows
            .Select((row, i) => new LeaderboardEntry
            {
                Rank            = i + 1,
                DisplayName     = EmailPrefix(row.Email),
                TotalExperience = row.TotalExperience,
                TotalLevel      = TotalLevel(row)
            })
            .ToList();
    }

    // ── helpers ──────────────────────────────────────────────────────

    private static string EmailPrefix(string email)
    {
        var at = email.IndexOf('@');
        return at > 0 ? email[..at] : email;
    }

    private static int StatLevel(int points) => points / 100 + 1;

    private static int TotalLevel(LeaderboardRow r)
    {
        var sum = StatLevel(r.Strength)
                + StatLevel(r.Intelligence)
                + StatLevel(r.Charisma)
                + StatLevel(r.Dexterity)
                + StatLevel(r.Vitality)
                + StatLevel(r.Willpower)
                + StatLevel(r.Systematization);
        return sum / 7;   // integer division == Math.floor(average), 7 stats total
    }

    // ── inner row DTO ─────────────────────────────────────────────────

    private record LeaderboardRow
    {
        public string Email           { get; init; } = string.Empty;
        public int    TotalExperience  { get; init; }
        public int    Strength         { get; init; }
        public int    Intelligence     { get; init; }
        public int    Charisma         { get; init; }
        public int    Dexterity        { get; init; }
        public int    Vitality         { get; init; }
        public int    Willpower        { get; init; }
        public int    Systematization  { get; init; }
    }
}
