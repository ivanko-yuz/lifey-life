namespace LifeyLife.Core.Models;

public record User
{
    public Guid Uuid { get; init; }
    public LocalizationType DefaultLanguage { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public int CurrentLevel { get; init; }
    public int CurrentExperience { get; init; }
}