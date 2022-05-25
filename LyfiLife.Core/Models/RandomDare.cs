namespace LyfiLife.Core.Models;

public record RandomDare
{
    public Guid Uuid { get; set; }
    public LocalizationType Language { get; set; }
    public string Context { get; set; }
    public int ExperienceGained { get; set; }
    public int GivenTime { get; set; }
}