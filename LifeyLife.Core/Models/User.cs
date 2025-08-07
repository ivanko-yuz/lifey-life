namespace LifeyLife.Core.Models;

public class User
{
    public Guid Uuid { get; init; }
    public string Email { get; init; }
    public string? PasswordHash { get; private set; }
    public LocalizationType PreferredLanguage { get; set; } = LocalizationType.ua;

    public void SetHashedPassword(string hash)
    {
        PasswordHash = hash;
    }
}