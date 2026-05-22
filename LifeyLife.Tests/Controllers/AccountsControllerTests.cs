using Xunit;
using LifeyLife.Api.Controllers;
using LifeyLife.Core.Contracts.Authentication;
using LifeyLife.Core.Models;
using LifeyLife.Core.Models.Auth;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LifeyLife.Tests.Controllers;

public class AccountsControllerTests
{
    private readonly Mock<IAccountsService> _accountsService = new();

    private AccountsController CreateSut() =>
        new(_accountsService.Object, ControllerHelper.CreateJwtHandler());

    // ── Register ─────────────────────────────────────────────────────

    [Fact]
    public async Task Register_PasswordMismatch_ReturnsBadRequest()
    {
        var sut = CreateSut();

        var result = await sut.Register(new RegistrationUser
        {
            Email = "user@test.com",
            Password = "pass1",
            ConfirmPassword = "pass2"
        });

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Register_ServiceFails_ReturnsBadRequest()
    {
        _accountsService
            .Setup(s => s.CreateUser(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(false);

        var sut = CreateSut();

        var result = await sut.Register(new RegistrationUser
        {
            Email = "user@test.com",
            Password = "pass1",
            ConfirmPassword = "pass1"
        });

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Register_Success_ReturnsOk()
    {
        _accountsService
            .Setup(s => s.CreateUser(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(true);

        var sut = CreateSut();

        var result = await sut.Register(new RegistrationUser
        {
            Email = "user@test.com",
            Password = "pass1",
            ConfirmPassword = "pass1"
        });

        Assert.IsType<OkObjectResult>(result);
    }

    // ── Login ────────────────────────────────────────────────────────

    [Fact]
    public async Task Login_UserNotFound_ReturnsUnauthorized()
    {
        _accountsService
            .Setup(s => s.FindByName(It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        var sut = CreateSut();

        var result = await sut.Login(new AuthenticationUser { Email = "nobody@test.com", Password = "x" });

        Assert.IsType<UnauthorizedObjectResult>(result);
    }

    [Fact]
    public async Task Login_WrongPassword_ReturnsUnauthorized()
    {
        var user = MakeUser();
        _accountsService.Setup(s => s.FindByName(It.IsAny<string>())).ReturnsAsync(user);
        _accountsService.Setup(s => s.CheckPassword(user, It.IsAny<string>())).ReturnsAsync(false);

        var sut = CreateSut();

        var result = await sut.Login(new AuthenticationUser { Email = "user@test.com", Password = "wrong" });

        Assert.IsType<UnauthorizedObjectResult>(result);
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsOkWithToken()
    {
        var user = MakeUser();
        _accountsService.Setup(s => s.FindByName(It.IsAny<string>())).ReturnsAsync(user);
        _accountsService.Setup(s => s.CheckPassword(user, It.IsAny<string>())).ReturnsAsync(true);

        var sut = CreateSut();

        var result = await sut.Login(new AuthenticationUser { Email = "user@test.com", Password = "pass" });

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(ok.Value);
    }

    // ── GetProfile ───────────────────────────────────────────────────

    [Fact]
    public async Task GetProfile_MissingClaim_ReturnsUnauthorized()
    {
        var sut = CreateSut();
        sut.ControllerContext = ControllerHelper.AnonContext();

        var result = await sut.GetProfile();

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async Task GetProfile_UserNotFound_ReturnsNotFound()
    {
        var userId = Guid.NewGuid();
        _accountsService.Setup(s => s.FindById(userId)).ReturnsAsync((User?)null);

        var sut = CreateSut();
        sut.ControllerContext = ControllerHelper.AuthContext(userId);

        var result = await sut.GetProfile();

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task GetProfile_ValidUser_ReturnsOk()
    {
        var userId = Guid.NewGuid();
        _accountsService.Setup(s => s.FindById(userId)).ReturnsAsync(MakeUser(userId));

        var sut = CreateSut();
        sut.ControllerContext = ControllerHelper.AuthContext(userId);

        var result = await sut.GetProfile();

        Assert.IsType<OkObjectResult>(result);
    }

    // ── UpdateLanguage ───────────────────────────────────────────────

    [Fact]
    public async Task UpdateLanguage_MissingClaim_ReturnsUnauthorized()
    {
        var sut = CreateSut();
        sut.ControllerContext = ControllerHelper.AnonContext();

        var result = await sut.UpdateLanguage(new UpdateLanguageRequest { Language = LocalizationType.en });

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async Task UpdateLanguage_UserNotFound_ReturnsNotFound()
    {
        var userId = Guid.NewGuid();
        _accountsService.Setup(s => s.FindById(userId)).ReturnsAsync((User?)null);

        var sut = CreateSut();
        sut.ControllerContext = ControllerHelper.AuthContext(userId);

        var result = await sut.UpdateLanguage(new UpdateLanguageRequest { Language = LocalizationType.en });

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task UpdateLanguage_UpdateFails_ReturnsBadRequest()
    {
        var userId = Guid.NewGuid();
        _accountsService.Setup(s => s.FindById(userId)).ReturnsAsync(MakeUser(userId));
        _accountsService.Setup(s => s.UpdateUser(It.IsAny<User>())).ReturnsAsync(false);

        var sut = CreateSut();
        sut.ControllerContext = ControllerHelper.AuthContext(userId);

        var result = await sut.UpdateLanguage(new UpdateLanguageRequest { Language = LocalizationType.en });

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task UpdateLanguage_Success_ReturnsOk()
    {
        var userId = Guid.NewGuid();
        _accountsService.Setup(s => s.FindById(userId)).ReturnsAsync(MakeUser(userId));
        _accountsService.Setup(s => s.UpdateUser(It.IsAny<User>())).ReturnsAsync(true);

        var sut = CreateSut();
        sut.ControllerContext = ControllerHelper.AuthContext(userId);

        var result = await sut.UpdateLanguage(new UpdateLanguageRequest { Language = LocalizationType.en });

        Assert.IsType<OkObjectResult>(result);
    }

    // ── helpers ──────────────────────────────────────────────────────

    private static User MakeUser(Guid? id = null)
    {
        var user = new User { Uuid = id ?? Guid.NewGuid() };
        user.SetEmail("user@test.com");
        return user;
    }
}
