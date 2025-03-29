namespace LifeyLife.Core.Models.Auth;

public record RegistrationUser
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string ConfirmPassword { get; init; }
}