namespace LifeyLife.Core.Models.Auth;

public record AuthenticationUser
{
    public string? Email { get; set; }

    public string? Password { get; set; }
}