namespace LyfiLife.Core.Models;

public record RandomDareHistory
{
    public Guid RandomDareUuid { get; init; }
    public Guid UserUuid { get; init; }
    public string Context { get; init; } = string.Empty;
    public DateTime CompletedAt { get; init; }
    public bool Completed { get; init; }
}