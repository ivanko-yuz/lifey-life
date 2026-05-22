using Xunit;
using LifeyLife.Api.Controllers;
using LifeyLife.Core.Contracts;
using LifeyLife.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LifeyLife.Tests.Controllers;

public class CharacterControllerTests
{
    private readonly Mock<ICharacterDataService> _characterService = new();

    private CharacterController CreateSut() =>
        new(_characterService.Object);

    [Fact]
    public async Task Get_MissingClaim_ReturnsBadRequest()
    {
        var sut = CreateSut();
        sut.ControllerContext = ControllerHelper.AnonContext();

        var result = await sut.Get();

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Get_ValidUser_ReturnsStats()
    {
        var userId = Guid.NewGuid();
        var stats = new CharacterStats
        {
            UserUuid        = userId,
            Strength        = 120,
            Intelligence    = 80,
            Charisma        = 50,
            Dexterity       = 30,
            Vitality        = 10,
            Willpower       = 200,
            TotalExperience = 490
        };

        _characterService.Setup(s => s.GetOrCreateStats(userId)).ReturnsAsync(stats);

        var sut = CreateSut();
        sut.ControllerContext = ControllerHelper.AuthContext(userId);

        var result = await sut.Get();

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(stats, ok.Value);
    }

    [Fact]
    public async Task Get_NewUser_ReturnsZeroedStats()
    {
        var userId = Guid.NewGuid();
        var empty = new CharacterStats { UserUuid = userId };

        _characterService.Setup(s => s.GetOrCreateStats(userId)).ReturnsAsync(empty);

        var sut = CreateSut();
        sut.ControllerContext = ControllerHelper.AuthContext(userId);

        var result = await sut.Get();

        var ok = Assert.IsType<OkObjectResult>(result);
        var returned = Assert.IsType<CharacterStats>(ok.Value);
        Assert.Equal(0, returned.TotalExperience);
        Assert.Equal(0, returned.Strength);
    }
}
