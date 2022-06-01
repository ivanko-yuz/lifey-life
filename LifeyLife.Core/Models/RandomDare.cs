namespace LifeyLife.Core.Models;

public record RandomDare
{
    public Guid Uuid { get; init; }
    public LocalizationType Language { get; init; }
    public string Context { get; init; }
    public int ExperienceGained { get; init; }
    public int GivenTime { get; init; }
}