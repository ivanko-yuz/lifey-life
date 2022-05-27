using LyfiLife.Core.Models;

namespace LyfiLife.Core.Contracts;

public interface IHistoryDataService
{
    Task<List<RandomDareHistory>> ListHistory(Guid userUuid);
    Task CompleteRandomDare(Guid userUuid, Guid randomDareUuid);
    
    Task SkipRandomDare(Guid userUuid, Guid randomDareUuid);

}