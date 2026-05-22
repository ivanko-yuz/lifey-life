using Xunit;
using LifeyLife.Api.Controllers;
using LifeyLife.Core.Contracts;
using LifeyLife.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LifeyLife.Tests.Controllers;

public class LeaderboardControllerTests
{
    private readonly Mock<ILeaderboardDataService> _leaderboardService = new();

    private LeaderboardController CreateSut() =>
        new(_leaderboardService.Object);

    [Fact]
    public async Task Get_ReturnsOkWithEntries()
    {
        var entries = new List<LeaderboardEntry>
        {
            new() { Rank = 1, DisplayName = "alice",   TotalExperience = 500, TotalLevel = 1 },
            new() { Rank = 2, DisplayName = "bob",     TotalExperience = 300, TotalLevel = 1 },
            new() { Rank = 3, DisplayName = "charlie", TotalExperience = 100, TotalLevel = 1 }
        };

        _leaderboardService.Setup(s => s.GetTopPlayers(50)).ReturnsAsync(entries);

        var sut = CreateSut();
        var result = await sut.Get();

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(entries, ok.Value);
    }

    [Fact]
    public async Task Get_EmptyBoard_ReturnsOkWithEmptyList()
    {
        _leaderboardService.Setup(s => s.GetTopPlayers(50))
                           .ReturnsAsync(new List<LeaderboardEntry>());

        var sut = CreateSut();
        var result = await sut.Get();

        var ok = Assert.IsType<OkObjectResult>(result);
        var list = Assert.IsType<List<LeaderboardEntry>>(ok.Value);
        Assert.Empty(list);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    [InlineData(200)]
    public async Task Get_OutOfRangeLimit_ClampsTo50(int badLimit)
    {
        _leaderboardService.Setup(s => s.GetTopPlayers(50))
                           .ReturnsAsync(new List<LeaderboardEntry>());

        var sut = CreateSut();
        await sut.Get(badLimit);

        // Ensure the service was called with the clamped value of 50
        _leaderboardService.Verify(s => s.GetTopPlayers(50), Times.Once);
    }
}
