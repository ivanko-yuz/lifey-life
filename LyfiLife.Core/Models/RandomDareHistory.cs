namespace LyfiLife.Core.Models;

public record RandomDareHistory
{
    public Guid RandomDareUuid { get; set; }
    public Guid UserUuid { get; set; }
    public int ReceivedAtUnixUtcTimestamp { get; set; }
    public bool Completed { get; set; }
}