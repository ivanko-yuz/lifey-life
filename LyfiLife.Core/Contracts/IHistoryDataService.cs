using LyfiLife.Core.Models;

namespace LyfiLife.Core.Contracts;

public interface IHistoryDataService
{
    Task<List<HistoryRecord>> GetAllHistory();
}