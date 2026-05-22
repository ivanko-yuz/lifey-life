namespace LifeyLife.Core.Models;

public record RandomDare
{
    public Guid            Uuid            { get; init; }
    public LocalizationType Language       { get; init; }
    public string          Context         { get; init; } = string.Empty;
    public int             ExperienceGained { get; init; }
    public int             GivenTime       { get; init; }
    public DareCategory    Category        { get; init; }
}