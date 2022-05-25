namespace LyfiLife.Core.Models;

public record User
{
    public string uuid { get; set; }
    public string DefaultLanguage { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string CurrentLevel { get; set; }
    public string CurrentExperience { get; set; }
}