using Xunit;
using LifeyLife.Api.Controllers;
using LifeyLife.Core.Contracts;
using LifeyLife.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LifeyLife.Tests.Controllers;

public class TodoControllerTests
{
    private readonly Mock<ITodoDataService> _todoService = new();

    private TodoController CreateSut() => new(_todoService.Object);

    // ── GetToday ──────────────────────────────────────────────────────

    [Fact]
    public async Task GetToday_MissingClaim_ReturnsBadRequest()
    {
        var sut = CreateSut();
        sut.ControllerContext = ControllerHelper.AnonContext();

        var result = await sut.GetToday();

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task GetToday_ValidUser_ReturnsItems()
    {
        var userId = Guid.NewGuid();
        var items = new List<TodoItem>
        {
            new() { Uuid = Guid.NewGuid(), UserUuid = userId, Title = "Buy milk", IsCompleted = false, CreatedAt = DateTime.UtcNow, DayDate = DateTime.Today },
            new() { Uuid = Guid.NewGuid(), UserUuid = userId, Title = "Go for a run", IsCompleted = true,  CreatedAt = DateTime.UtcNow, DayDate = DateTime.Today }
        };

        _todoService.Setup(s => s.GetTodayItems(userId)).ReturnsAsync(items);

        var sut = CreateSut();
        sut.ControllerContext = ControllerHelper.AuthContext(userId);

        var result = await sut.GetToday();

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(items, ok.Value);
    }

    [Fact]
    public async Task GetToday_ValidUser_EmptyList_ReturnsOkWithEmpty()
    {
        var userId = Guid.NewGuid();
        _todoService.Setup(s => s.GetTodayItems(userId)).ReturnsAsync(new List<TodoItem>());

        var sut = CreateSut();
        sut.ControllerContext = ControllerHelper.AuthContext(userId);

        var result = await sut.GetToday();

        var ok = Assert.IsType<OkObjectResult>(result);
        var list = Assert.IsType<List<TodoItem>>(ok.Value);
        Assert.Empty(list);
    }

    // ── Add ───────────────────────────────────────────────────────────

    [Fact]
    public async Task Add_MissingClaim_ReturnsBadRequest()
    {
        var sut = CreateSut();
        sut.ControllerContext = ControllerHelper.AnonContext();

        var result = await sut.Add(new AddTodoRequest { Title = "Task" });

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Add_EmptyTitle_ReturnsBadRequest()
    {
        var userId = Guid.NewGuid();
        var sut = CreateSut();
        sut.ControllerContext = ControllerHelper.AuthContext(userId);

        var result = await sut.Add(new AddTodoRequest { Title = "   " });

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Add_ValidRequest_ReturnsCreatedItem()
    {
        var userId = Guid.NewGuid();
        var item = new TodoItem
        {
            Uuid        = Guid.NewGuid(),
            UserUuid    = userId,
            Title       = "Write tests",
            IsCompleted = false,
            CreatedAt   = DateTime.UtcNow,
            DayDate     = DateTime.Today
        };

        _todoService.Setup(s => s.AddItem(userId, "Write tests")).ReturnsAsync(item);

        var sut = CreateSut();
        sut.ControllerContext = ControllerHelper.AuthContext(userId);

        var result = await sut.Add(new AddTodoRequest { Title = "Write tests" });

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(item, ok.Value);
    }

    // ── Toggle ────────────────────────────────────────────────────────

    [Fact]
    public async Task Toggle_MissingClaim_ReturnsBadRequest()
    {
        var sut = CreateSut();
        sut.ControllerContext = ControllerHelper.AnonContext();

        var result = await sut.Toggle(Guid.NewGuid());

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Toggle_ValidUser_ReturnsOk()
    {
        var userId = Guid.NewGuid();
        var itemId = Guid.NewGuid();

        _todoService.Setup(s => s.ToggleComplete(userId, itemId)).Returns(Task.CompletedTask);

        var sut = CreateSut();
        sut.ControllerContext = ControllerHelper.AuthContext(userId);

        var result = await sut.Toggle(itemId);

        Assert.IsType<OkResult>(result);
        _todoService.Verify(s => s.ToggleComplete(userId, itemId), Times.Once);
    }

    // ── Delete ────────────────────────────────────────────────────────

    [Fact]
    public async Task Delete_MissingClaim_ReturnsBadRequest()
    {
        var sut = CreateSut();
        sut.ControllerContext = ControllerHelper.AnonContext();

        var result = await sut.Delete(Guid.NewGuid());

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Delete_ValidUser_ReturnsOk()
    {
        var userId = Guid.NewGuid();
        var itemId = Guid.NewGuid();

        _todoService.Setup(s => s.DeleteItem(userId, itemId)).Returns(Task.CompletedTask);

        var sut = CreateSut();
        sut.ControllerContext = ControllerHelper.AuthContext(userId);

        var result = await sut.Delete(itemId);

        Assert.IsType<OkResult>(result);
        _todoService.Verify(s => s.DeleteItem(userId, itemId), Times.Once);
    }

    // ── FinishDay ─────────────────────────────────────────────────────

    [Fact]
    public async Task FinishDay_MissingClaim_ReturnsBadRequest()
    {
        var sut = CreateSut();
        sut.ControllerContext = ControllerHelper.AnonContext();

        var result = await sut.FinishDay();

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task FinishDay_ValidUser_ReturnsHistoryRecord()
    {
        var userId = Guid.NewGuid();
        var historyRecord = new TodoDayHistory
        {
            Uuid              = Guid.NewGuid(),
            UserUuid          = userId,
            DayDate           = DateTime.Today,
            FinishedAt        = DateTime.UtcNow,
            CompletedCount    = 3,
            TotalCount        = 5,
            ExperienceAwarded = 60
        };

        _todoService.Setup(s => s.FinishDay(userId)).ReturnsAsync(historyRecord);

        var sut = CreateSut();
        sut.ControllerContext = ControllerHelper.AuthContext(userId);

        var result = await sut.FinishDay();

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(historyRecord, ok.Value);
    }

    // ── GetHistory ────────────────────────────────────────────────────

    [Fact]
    public async Task GetHistory_MissingClaim_ReturnsBadRequest()
    {
        var sut = CreateSut();
        sut.ControllerContext = ControllerHelper.AnonContext();

        var result = await sut.GetHistory();

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task GetHistory_ValidUser_ReturnsHistoryList()
    {
        var userId = Guid.NewGuid();
        var history = new List<TodoDayHistory>
        {
            new() { Uuid = Guid.NewGuid(), UserUuid = userId, DayDate = DateTime.Today.AddDays(-1), FinishedAt = DateTime.UtcNow.AddDays(-1), CompletedCount = 4, TotalCount = 4, ExperienceAwarded = 80 },
            new() { Uuid = Guid.NewGuid(), UserUuid = userId, DayDate = DateTime.Today.AddDays(-2), FinishedAt = DateTime.UtcNow.AddDays(-2), CompletedCount = 2, TotalCount = 3, ExperienceAwarded = 40 }
        };

        _todoService.Setup(s => s.GetHistory(userId)).ReturnsAsync(history);

        var sut = CreateSut();
        sut.ControllerContext = ControllerHelper.AuthContext(userId);

        var result = await sut.GetHistory();

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(history, ok.Value);
    }
}
