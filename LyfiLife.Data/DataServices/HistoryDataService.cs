using LyfiLife.Core.Contracts;
using LyfiLife.Core.Models;

namespace LyfiLife.Data.DataServices;

public class HistoryDataService : IHistoryDataService
{
    public Task<List<RandomDareHistory>> ListHistory(Guid userUuid)
    {
        return Task.FromResult(new List<RandomDareHistory>(Enumerable.Range(1, 5).Select(index => new RandomDareHistory
            {
                UserUuid = Guid.NewGuid(),
                RandomDareUuid = Guid.NewGuid(),
                Completed = true,
                ReceivedAtUnixUtcTimestamp = 1
            })
            .ToArray()));
    }
}