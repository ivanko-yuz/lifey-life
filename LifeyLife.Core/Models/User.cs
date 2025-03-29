namespace LifeyLife.Core.Models;

public class User
{
    public Guid Uuid { get; init; }
    public string Email { get; init; }
    public string? PasswordHash { get; private set; }

    public void SetHashedPassword(string hash)
    {
        PasswordHash = hash;
    }
}