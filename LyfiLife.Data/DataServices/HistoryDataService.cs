using LyfiLife.Core.Contracts;
using LyfiLife.Core.Models;
using LyfiLife.Data.Extensions;

namespace LyfiLife.Data.DataServices;

public class HistoryDataService : IHistoryDataService
{
    private readonly IDbAdapter _dbAdapter;

    public HistoryDataService(IDbAdapter dbAdapter)
    {
        _dbAdapter = dbAdapter;
    }
    public async Task<List<RandomDareHistory>> ListHistory(Guid userUuid)
    {
        var query = $@"SELECT 
                               rdh.user_uuid as {nameof(RandomDareHistory.UserUuid)}, 
                               rdh.random_dares_uuid as {nameof(RandomDareHistory.RandomDareUuid)}, 
                               to_timestamp(rdh.received_at_unix_utc_timestamp) as {nameof(RandomDareHistory.CompletedAt)}, 
                               rdh.completed as {nameof(RandomDareHistory.Completed)}, 
                               rd.context as {nameof(RandomDareHistory.Context)} 
                        FROM public.random_dares_history rdh
                        JOIN public.random_dares rd ON rdh.random_dares_uuid = rd.uuid;";

        return (await _dbAdapter.GetMany<RandomDareHistory>(query, new { })).ToList();
    }

    public async Task CompleteRandomDare(Guid userUuid, Guid randomDareUuid)
    {
        var query = $@"INSERT INTO public.random_dares_history
                        (
                             user_uuid,
                             random_dares_uuid,
                             received_at_unix_utc_timestamp,
                             completed
                         ) VALUES (
                               @{nameof(userUuid)}, 
                               @{nameof(randomDareUuid)}, 
                               @Timestamp,
                               true
                        );";

        await _dbAdapter.ExecuteCommand(query, new
        {
            userUuid,
            randomDareUuid,
            Timestamp = DateTime.UtcNow.ToUnixUtcTimeStamp()
        });
    }
    
    public async Task SkipRandomDare(Guid userUuid, Guid randomDareUuid)
    {
        var query = $@"INSERT INTO public.random_dares_history
                        (
                             user_uuid,
                             random_dares_uuid,
                             received_at_unix_utc_timestamp,
                             completed
                         ) VALUES (
                               @{nameof(userUuid)}, 
                               @{nameof(randomDareUuid)}, 
                               @Timestamp,
                               false
                        );";

        await _dbAdapter.ExecuteCommand(query, new
        {
            userUuid,
            randomDareUuid,
            Timestamp = DateTime.UtcNow.ToUnixUtcTimeStamp()
        });
    }
}