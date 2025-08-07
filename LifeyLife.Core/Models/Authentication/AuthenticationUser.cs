namespace LifeyLife.Core.Models.Auth;

public record AuthenticationUser
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}