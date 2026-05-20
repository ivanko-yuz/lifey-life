using System.Globalization;

namespace LifeyLife.Core.Models;

public class User
{
    public Guid Uuid { get; init; }
    public string Email { get; private set; } = string.Empty;
    public string? PasswordHash { get; private set; }
    public LocalizationType PreferredLanguage { get; set; } = LocalizationType.ua;

    public void SetEmail(string email) =>
        Email = email.ToLower(CultureInfo.InvariantCulture);

    public void NormalizeEmail() =>
        Email = Email.ToLower(CultureInfo.InvariantCulture);

    public void SetHashedPassword(string hash) =>
        PasswordHash = hash;
}