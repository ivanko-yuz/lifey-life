using LifeyLife.Core.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LifeyLife.Api.Controllers;

[ApiController]
[Route("api/todo")]
[Authorize]
public class TodoController : ControllerBase
{
    private readonly ITodoDataService _todoDataService;

    public TodoController(ITodoDataService todoDataService)
    {
        _todoDataService = todoDataService;
    }

    // ── helpers ──────────────────────────────────────────────────────

    private Guid? GetUserUuid()
    {
        var value = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(value, out var g) ? g : null;
    }

    // ── endpoints ────────────────────────────────────────────────────

    /// <summary>GET /api/todo — today's active items for the current user.</summary>
    [HttpGet]
    public async Task<IActionResult> GetToday()
    {
        var userId = GetUserUuid();
        if (userId is null) return BadRequest("Invalid user ID");

        var items = await _todoDataService.GetTodayItems(userId.Value);
        return Ok(items);
    }

    /// <summary>POST /api/todo — add a new todo item for today.</summary>
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddTodoRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            return BadRequest("Title is required");

        var userId = GetUserUuid();
        if (userId is null) return BadRequest("Invalid user ID");

        var item = await _todoDataService.AddItem(userId.Value, request.Title.Trim());
        return Ok(item);
    }

    /// <summary>PUT /api/todo/{uuid}/toggle — flip the completion flag.</summary>
    [HttpPut("{uuid:guid}/toggle")]
    public async Task<IActionResult> Toggle(Guid uuid)
    {
        var userId = GetUserUuid();
        if (userId is null) return BadRequest("Invalid user ID");

        await _todoDataService.ToggleComplete(userId.Value, uuid);
        return Ok();
    }

    /// <summary>DELETE /api/todo/{uuid} — remove a todo item.</summary>
    [HttpDelete("{uuid:guid}")]
    public async Task<IActionResult> Delete(Guid uuid)
    {
        var userId = GetUserUuid();
        if (userId is null) return BadRequest("Invalid user ID");

        await _todoDataService.DeleteItem(userId.Value, uuid);
        return Ok();
    }

    /// <summary>POST /api/todo/finish-day — archive today and award XP.</summary>
    [HttpPost("finish-day")]
    public async Task<IActionResult> FinishDay()
    {
        var userId = GetUserUuid();
        if (userId is null) return BadRequest("Invalid user ID");

        var result = await _todoDataService.FinishDay(userId.Value);
        return Ok(result);
    }

    /// <summary>GET /api/todo/history — all finished-day records, newest first.</summary>
    [HttpGet("history")]
    public async Task<IActionResult> GetHistory()
    {
        var userId = GetUserUuid();
        if (userId is null) return BadRequest("Invalid user ID");

        var history = await _todoDataService.GetHistory(userId.Value);
        return Ok(history);
    }
}

/// <summary>Request body for <see cref="TodoController.Add"/>.</summary>
public record AddTodoRequest
{
    public string Title { get; init; } = string.Empty;
}
