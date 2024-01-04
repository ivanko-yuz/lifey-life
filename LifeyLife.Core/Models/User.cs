namespace LifeyLife.Core.Models;

public class User
{
    public Guid Uuid { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? UserName { get; init; }
    public string? Email { get; init; }
    public string? PasswordHash { get; private set; }

    public LocalizationType DefaultLanguage { get; init; }
    public int CurrentLevel { get; init; }
    public int CurrentExperience { get; init; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }

    public void SetHashedPassword(string hash)
    {
        PasswordHash = hash;
    }
}