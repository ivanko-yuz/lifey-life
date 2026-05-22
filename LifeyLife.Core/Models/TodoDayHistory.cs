namespace LifeyLife.Core.Models;

public record TodoDayHistory
{
    public Guid     Uuid              { get; init; }
    public Guid     UserUuid          { get; init; }
    public DateTime DayDate           { get; init; }
    public DateTime FinishedAt        { get; init; }
    public int      CompletedCount    { get; init; }
    public int      TotalCount        { get; init; }
    public int      ExperienceAwarded { get; init; }
}
