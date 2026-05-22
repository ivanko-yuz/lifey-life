namespace LifeyLife.Core.Models;

public record LeaderboardEntry
{
    public int    Rank            { get; init; }
    public string DisplayName     { get; init; } = string.Empty;
    public int    TotalExperience { get; init; }
    public int    TotalLevel      { get; init; }
}
