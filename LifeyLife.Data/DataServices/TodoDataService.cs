using LifeyLife.Core.Contracts;
using LifeyLife.Core.Models;

namespace LifeyLife.Data.DataServices;

public class TodoDataService : ITodoDataService
{
    private const int XpPerCompletedItem = 20;

    private readonly IDbAdapter _dbAdapter;
    private readonly ICharacterDataService _characterDataService;

    public TodoDataService(IDbAdapter dbAdapter, ICharacterDataService characterDataService)
    {
        _dbAdapter = dbAdapter;
        _characterDataService = characterDataService;
    }

    public async Task<List<TodoItem>> GetTodayItems(Guid userUuid)
    {
        const string sql = $@"
            SELECT
                uuid         AS {nameof(TodoItem.Uuid)},
                user_uuid    AS {nameof(TodoItem.UserUuid)},
                title        AS {nameof(TodoItem.Title)},
                is_completed AS {nameof(TodoItem.IsCompleted)},
                created_at   AS {nameof(TodoItem.CreatedAt)},
                completed_at AS {nameof(TodoItem.CompletedAt)},
                day_date     AS {nameof(TodoItem.DayDate)}
            FROM public.todo_item
            WHERE user_uuid = @UserUuid
              AND day_date = CURRENT_DATE
            ORDER BY created_at";

        var rows = await _dbAdapter.GetMany<TodoItem>(sql, new { UserUuid = userUuid });
        return rows.ToList();
    }

    public async Task<TodoItem> AddItem(Guid userUuid, string title)
    {
        const string sql = $@"
            INSERT INTO public.todo_item (user_uuid, title)
            VALUES (@UserUuid, @Title)
            RETURNING
                uuid         AS {nameof(TodoItem.Uuid)},
                user_uuid    AS {nameof(TodoItem.UserUuid)},
                title        AS {nameof(TodoItem.Title)},
                is_completed AS {nameof(TodoItem.IsCompleted)},
                created_at   AS {nameof(TodoItem.CreatedAt)},
                completed_at AS {nameof(TodoItem.CompletedAt)},
                day_date     AS {nameof(TodoItem.DayDate)}";

        return await _dbAdapter.GetSingle<TodoItem>(sql, new { UserUuid = userUuid, Title = title });
    }

    public async Task ToggleComplete(Guid userUuid, Guid itemUuid)
    {
        const string sql = @"
            UPDATE public.todo_item
            SET is_completed = NOT is_completed,
                completed_at = CASE
                                   WHEN NOT is_completed THEN now()
                                   ELSE NULL
                               END
            WHERE uuid      = @ItemUuid
              AND user_uuid = @UserUuid";

        await _dbAdapter.ExecuteCommand(sql, new { ItemUuid = itemUuid, UserUuid = userUuid });
    }

    public async Task DeleteItem(Guid userUuid, Guid itemUuid)
    {
        const string sql = @"
            DELETE FROM public.todo_item
            WHERE uuid      = @ItemUuid
              AND user_uuid = @UserUuid";

        await _dbAdapter.ExecuteCommand(sql, new { ItemUuid = itemUuid, UserUuid = userUuid });
    }

    public async Task<TodoDayHistory> FinishDay(Guid userUuid)
    {
        // 1. Count today's items (cast to int — counts will never overflow int)
        const string countSql = @"
            SELECT
                COUNT(*)::int                              AS Total,
                COUNT(*) FILTER (WHERE is_completed)::int AS Completed
            FROM public.todo_item
            WHERE user_uuid = @UserUuid
              AND day_date  = CURRENT_DATE";

        var counts = await _dbAdapter.GetSingle<DayCount>(countSql, new { UserUuid = userUuid });
        var xp = counts.Completed * XpPerCompletedItem;

        // 2. Archive to history
        const string insertSql = $@"
            INSERT INTO public.todo_history
                (user_uuid, day_date, completed_count, total_count, experience_awarded)
            VALUES
                (@UserUuid, CURRENT_DATE, @CompletedCount, @TotalCount, @ExperienceAwarded)
            RETURNING
                uuid               AS {nameof(TodoDayHistory.Uuid)},
                user_uuid          AS {nameof(TodoDayHistory.UserUuid)},
                day_date           AS {nameof(TodoDayHistory.DayDate)},
                finished_at        AS {nameof(TodoDayHistory.FinishedAt)},
                completed_count    AS {nameof(TodoDayHistory.CompletedCount)},
                total_count        AS {nameof(TodoDayHistory.TotalCount)},
                experience_awarded AS {nameof(TodoDayHistory.ExperienceAwarded)}";

        var historyRecord = await _dbAdapter.GetSingle<TodoDayHistory>(insertSql, new
        {
            UserUuid         = userUuid,
            CompletedCount   = counts.Completed,
            TotalCount       = counts.Total,
            ExperienceAwarded = xp
        });

        // 3. Delete today's active items
        const string deleteSql = @"
            DELETE FROM public.todo_item
            WHERE user_uuid = @UserUuid
              AND day_date  = CURRENT_DATE";

        await _dbAdapter.ExecuteCommand(deleteSql, new { UserUuid = userUuid });

        // 4. Award XP to the Systematization stat
        if (xp > 0)
            await _characterDataService.AwardSystematizationPoints(userUuid, xp);

        return historyRecord;
    }

    public async Task<List<TodoDayHistory>> GetHistory(Guid userUuid)
    {
        const string sql = $@"
            SELECT
                uuid               AS {nameof(TodoDayHistory.Uuid)},
                user_uuid          AS {nameof(TodoDayHistory.UserUuid)},
                day_date           AS {nameof(TodoDayHistory.DayDate)},
                finished_at        AS {nameof(TodoDayHistory.FinishedAt)},
                completed_count    AS {nameof(TodoDayHistory.CompletedCount)},
                total_count        AS {nameof(TodoDayHistory.TotalCount)},
                experience_awarded AS {nameof(TodoDayHistory.ExperienceAwarded)}
            FROM public.todo_history
            WHERE user_uuid = @UserUuid
            ORDER BY finished_at DESC";

        var rows = await _dbAdapter.GetMany<TodoDayHistory>(sql, new { UserUuid = userUuid });
        return rows.ToList();
    }

    // ── inner DTO ─────────────────────────────────────────────────────

    private record DayCount
    {
        public int Total     { get; init; }
        public int Completed { get; init; }
    }
}
