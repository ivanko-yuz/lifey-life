using Xunit;
using LifeyLife.Api.Controllers;
using LifeyLife.Core.Contracts;
using LifeyLife.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LifeyLife.Tests.Controllers;

public class RandomDareHistoryControllerTests
{
    private readonly Mock<IHistoryDataService> _historyService = new();

    private RandomDareHistoryController CreateSut() =>
        new(_historyService.Object);

    [Fact]
    public async Task Get_MissingClaim_ReturnsUnauthorized()
    {
        var sut = CreateSut();
        sut.ControllerContext = ControllerHelper.AnonContext();

        var result = await sut.Get();

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async Task Get_ValidUser_ReturnsHistoryList()
    {
        var userId = Guid.NewGuid();
        var history = new List<RandomDareHistory>
        {
            new()
            {
                RandomDareUuid   = Guid.NewGuid(),
                UserUuid         = userId,
                Context          = "Meet 3 strangers",
                CompletedAt      = DateTime.UtcNow,
                Completed        = true,
                ExperienceGained = 30
            }
        };

        _historyService.Setup(s => s.ListHistory(userId)).ReturnsAsync(history);

        var sut = CreateSut();
        sut.ControllerContext = ControllerHelper.AuthContext(userId);

        var result = await sut.Get();

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(history, ok.Value);
    }

    [Fact]
    public async Task Get_ValidUser_EmptyHistory_ReturnsOkWithEmptyList()
    {
        var userId = Guid.NewGuid();
        _historyService.Setup(s => s.ListHistory(userId)).ReturnsAsync(new List<RandomDareHistory>());

        var sut = CreateSut();
        sut.ControllerContext = ControllerHelper.AuthContext(userId);

        var result = await sut.Get();

        var ok = Assert.IsType<OkObjectResult>(result);
        var list = Assert.IsType<List<RandomDareHistory>>(ok.Value);
        Assert.Empty(list);
    }
}
