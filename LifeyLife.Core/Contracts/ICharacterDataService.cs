using LifeyLife.Core.Models;

namespace LifeyLife.Core.Contracts;

public interface ICharacterDataService
{
    /// <summary>Returns existing stats or creates a zeroed row for this user.</summary>
    Task<CharacterStats> GetOrCreateStats(Guid userUuid);

    /// <summary>Awards <paramref name="points"/> to the stat tied to the dare category
    /// and increments total_experience by the same amount.</summary>
    Task AwardStatPoints(Guid userUuid, DareCategory category, int points);

    /// <summary>Awards <paramref name="points"/> directly to the Systematization stat
    /// and increments total_experience by the same amount.</summary>
    Task AwardSystematizationPoints(Guid userUuid, int points);
}
