namespace LifeyLife.Core.Models.Auth;

public record RegistrationUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
        
    public string? Email { get; set; }
        
    public string? Password { get; set; }
        
    public string? ConfirmPassword { get; set; }
}