using LifeyLife.Core.Contracts;
using LifeyLife.Core.Extensions;
using LifeyLife.Core.Models;

namespace LifeyLife.Data.DataServices;

public class HistoryDataService : IHistoryDataService
{
    private readonly IDbAdapter _dbAdapter;

    public HistoryDataService(IDbAdapter dbAdapter)
    {
        _dbAdapter = dbAdapter;
    }
    public async Task<List<RandomDareHistory>> ListHistory(Guid userUuid)
    {
        const string query = $@"SELECT
                               rdh.user_uuid              AS {nameof(RandomDareHistory.UserUuid)},
                               rdh.random_dares_uuid      AS {nameof(RandomDareHistory.RandomDareUuid)},
                               to_timestamp(rdh.received_at_unix_utc_timestamp) AS {nameof(RandomDareHistory.CompletedAt)},
                               rdh.completed              AS {nameof(RandomDareHistory.Completed)},
                               rd.context                 AS {nameof(RandomDareHistory.Context)},
                               rd.experience_gained       AS {nameof(RandomDareHistory.ExperienceGained)}
                        FROM public.random_dare_history rdh
                        JOIN public.random_dare rd ON rdh.random_dares_uuid = rd.uuid
                        WHERE rdh.user_uuid = @UserUuid
                        ORDER BY rdh.received_at_unix_utc_timestamp DESC;";

        return (await _dbAdapter.GetMany<RandomDareHistory>(query, new { UserUuid = userUuid })).ToList();
    }

    public async Task SaveCompletedRandomDareInHistory(Guid userUuid, Guid randomDareUuid)
    {
        const string query = $@"INSERT INTO public.random_dare_history
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
    
    public async Task SaveSkippedRandomDareInHistory(Guid userUuid, Guid randomDareUuid)
    {
        const string query = $@"INSERT INTO public.random_dare_history
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