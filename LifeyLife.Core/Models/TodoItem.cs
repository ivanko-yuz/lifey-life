namespace LifeyLife.Core.Models;

public record TodoItem
{
    public Guid      Uuid        { get; init; }
    public Guid      UserUuid    { get; init; }
    public string    Title       { get; init; } = string.Empty;
    public bool      IsCompleted { get; init; }
    public DateTime  CreatedAt   { get; init; }
    public DateTime? CompletedAt { get; init; }
    public DateTime  DayDate     { get; init; }
}
