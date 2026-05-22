using Xunit;
using LifeyLife.Api.Controllers;
using LifeyLife.Core.Contracts;
using LifeyLife.Core.Contracts.Authentication;
using LifeyLife.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace LifeyLife.Tests.Controllers;

public class RandomDareControllerTests
{
    private readonly Mock<IRandomDareDataService> _dareService          = new();
    private readonly Mock<IHistoryDataService>    _historyService        = new();
    private readonly Mock<IAccountsDataService>   _accountsDataService   = new();
    private readonly Mock<ICharacterDataService>  _characterService      = new();

    private RandomDareController CreateSut() => new(
        NullLogger<RandomDareController>.Instance,
        _dareService.Object,
        _historyService.Object,
        _accountsDataService.Object,
        _characterService.Object
    );

    // ── Get ──────────────────────────────────────────────────────────

    [Fact]
    public async Task Get_Unauthenticated_FallsBackToUkrainian()
    {
        var dare = SampleDare();
        _dareService.Setup(s => s.GetRandomDareByLanguage(LocalizationType.ua)).ReturnsAsync(dare);

        var sut = CreateSut();
        sut.ControllerContext = ControllerHelper.AnonContext();

        var result = await sut.Get();

        Assert.Equal(dare, result);
        _dareService.Verify(s => s.GetRandomDareByLanguage(LocalizationType.ua), Times.Once);
    }

    [Fact]
    public async Task Get_WithExplicitLanguageParam_UsesRequestedLanguage()
    {
        var dare = SampleDare();
        _dareService.Setup(s => s.GetRandomDareByLanguage(LocalizationType.en)).ReturnsAsync(dare);

        var sut = CreateSut();
        sut.ControllerContext = ControllerHelper.AnonContext();

        await sut.Get(LocalizationType.en);

        _dareService.Verify(s => s.GetRandomDareByLanguage(LocalizationType.en), Times.Once);
    }

    [Fact]
    public async Task Get_AuthenticatedUser_UsesPreferredLanguage()
    {
        var userId = Guid.NewGuid();
        var user = new User { Uuid = userId, PreferredLanguage = LocalizationType.en };
        user.SetEmail("u@test.com");
        var dare = SampleDare();

        _accountsDataService.Setup(s => s.FindById(userId)).ReturnsAsync(user);
        _dareService.Setup(s => s.GetRandomDareByLanguage(LocalizationType.en)).ReturnsAsync(dare);

        var sut = CreateSut();
        sut.ControllerContext = ControllerHelper.AuthContext(userId);

        await sut.Get();

        _dareService.Verify(s => s.GetRandomDareByLanguage(LocalizationType.en), Times.Once);
    }

    [Fact]
    public async Task Get_ExplicitParamOverridesAuthenticatedUserLanguage()
    {
        // Even if the user prefers UA, an explicit en param takes priority.
        var dare = SampleDare();
        _dareService.Setup(s => s.GetRandomDareByLanguage(LocalizationType.en)).ReturnsAsync(dare);

        var sut = CreateSut();
        sut.ControllerContext = ControllerHelper.AnonContext();

        await sut.Get(LocalizationType.en);

        _dareService.Verify(s => s.GetRandomDareByLanguage(LocalizationType.en), Times.Once);
        // The accounts data service should NOT be called when a language param is supplied
        _accountsDataService.Verify(s => s.FindById(It.IsAny<Guid>()), Times.Never);
    }

    // ── Complete ─────────────────────────────────────────────────────

    [Fact]
    public async Task Complete_MissingClaim_ReturnsBadRequest()
    {
        var sut = CreateSut();
        sut.ControllerContext = ControllerHelper.AnonContext();

        var result = await sut.Complete(SampleDare());

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Complete_ValidUser_SavesHistoryAndAwardsPoints()
    {
        var userId = Guid.NewGuid();
        var dare = SampleDare();

        _historyService
            .Setup(s => s.SaveCompletedRandomDareInHistory(userId, dare.Uuid))
            .Returns(Task.CompletedTask);
        _characterService
            .Setup(s => s.AwardStatPoints(userId, dare.Category, dare.ExperienceGained))
            .Returns(Task.CompletedTask);

        var sut = CreateSut();
        sut.ControllerContext = ControllerHelper.AuthContext(userId);

        var result = await sut.Complete(dare);

        Assert.IsType<OkResult>(result);
        _historyService.Verify(
            s => s.SaveCompletedRandomDareInHistory(userId, dare.Uuid), Times.Once);
        _characterService.Verify(
            s => s.AwardStatPoints(userId, dare.Category, dare.ExperienceGained), Times.Once);
    }

    // ── helpers ──────────────────────────────────────────────────────

    private static RandomDare SampleDare(Guid? id = null) => new()
    {
        Uuid             = id ?? Guid.NewGuid(),
        Context          = "Do something brave",
        ExperienceGained = 50,
        GivenTime        = 10,
        Language         = LocalizationType.ua,
        Category         = DareCategory.physical
    };
}
