using LifeyLife.Core.Models;

namespace LifeyLife.Core.Contracts;

public interface ITodoDataService
{
    /// <summary>Returns all active todo items for today (user-scoped).</summary>
    Task<List<TodoItem>> GetTodayItems(Guid userUuid);

    /// <summary>Adds a new todo item for today and returns the created record.</summary>
    Task<TodoItem> AddItem(Guid userUuid, string title);

    /// <summary>Flips the is_completed flag on the specified item (user-scoped).</summary>
    Task ToggleComplete(Guid userUuid, Guid itemUuid);

    /// <summary>Permanently deletes the specified item (user-scoped).</summary>
    Task DeleteItem(Guid userUuid, Guid itemUuid);

    /// <summary>
    /// Archives today's items into <c>todo_history</c>, awards XP for completed items,
    /// deletes the active items, and returns the resulting history record.
    /// </summary>
    Task<TodoDayHistory> FinishDay(Guid userUuid);

    /// <summary>Returns all finished-day history records for the user, newest first.</summary>
    Task<List<TodoDayHistory>> GetHistory(Guid userUuid);
}
