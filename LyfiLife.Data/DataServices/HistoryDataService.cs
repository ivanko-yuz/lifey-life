using LyfiLife.Core;
using LyfiLife.Core.Contracts;
using LyfiLife.Core.Models;

namespace LyfiLife.Data;

public class HistoryDataService : IHistoryDataService
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public Task<List<HistoryRecord>> GetAllHistory()
    {
        return Task.FromResult(new List<HistoryRecord>(Enumerable.Range(1, 5).Select(index => new HistoryRecord
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray()));
    }
}