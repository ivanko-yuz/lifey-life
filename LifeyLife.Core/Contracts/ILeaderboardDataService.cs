using LifeyLife.Core.Models;

namespace LifeyLife.Core.Contracts;

public interface ILeaderboardDataService
{
    Task<List<LeaderboardEntry>> GetTopPlayers(int limit = 50);
}
