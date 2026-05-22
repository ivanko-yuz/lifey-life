namespace LifeyLife.Core.Models;

public record CharacterStats
{
    public Guid   UserUuid        { get; init; }
    public int    Strength        { get; init; }
    public int    Intelligence    { get; init; }
    public int    Charisma        { get; init; }
    public int    Dexterity       { get; init; }
    public int    Vitality         { get; init; }
    public int    Willpower        { get; init; }
    public int    Systematization  { get; init; }
    public int    TotalExperience  { get; init; }
}
