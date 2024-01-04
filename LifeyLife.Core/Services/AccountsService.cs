using System.Security.Claims;
using LifeyLife.Core.Contracts.Authentication;
using LifeyLife.Core.Models;
using LifeyLife.Core.Utils;

namespace LifeyLife.Core.Services;

public class AccountsService : IAccountsService
{
    private readonly IAccountsDataService _accountsDataService;
    private readonly IPasswordHasher<User> _passwordHasher;
    private const string LoginProvider = "AspNetUserStore";
    private const string AuthenticatorKeyTokenName = "AuthenticatorKey";

    public AccountsService(
        IAccountsDataService accountsDataService,
        IPasswordHasher<User> passwordHasher)
    {
        _accountsDataService = accountsDataService;
        _passwordHasher = passwordHasher;
    }

    // protected UserLogin CreateUserLogin(User user, string key)
    // {
    //     return new UserLogin
    //     {
    //         UserId = user.Id,
    //         ProviderKey = "AspNetUserStore",
    //         LoginProvider = key,
    //         ProviderDisplayName = "AuthenticatorKey"
    //     };
    // }

    // public Task<string> GetAuthenticatorKey(User user, CancellationToken cancellationToken)
    //     => GetToken(user, InternalLoginProvider, AuthenticatorKeyTokenName, cancellationToken);

    public string GetUserName(ClaimsPrincipal principal)
    {
        if (principal == null)
        {
            throw new ArgumentNullException(nameof(principal));
        }

        return principal.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    public string GetUserId(ClaimsPrincipal principal)
    {
        if (principal == null)
        {
            throw new ArgumentNullException(nameof(principal));
        }

        return principal.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    public Task<User> GetUser(ClaimsPrincipal principal)
    {
        if (principal == null)
        {
            throw new ArgumentNullException(nameof(principal));
        }

        var id = GetUserId(principal);
        return id == null ? Task.FromResult<User>(null) : _accountsDataService.FindById(Guid.Parse(id));
    }
    
    public async Task<bool> CreateUser(User user, string password)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        if (password == null)
        {
            throw new ArgumentNullException(nameof(password));
        }
        var result = await UpdatePasswordHash(user, password);
        if (!result)
        {
            return result;
        }
        return await CreateUser(user);
    }

    public async Task<User> UpdateUser(User user)
    {
        // await UpdateNormalizedUserName(user);
        // await UpdateNormalizedEmail(user);

        return await _accountsDataService.Update(user);
    }

    public async Task<User> FindByName(string userName)
    {
        if (userName == null)
        {
            throw new ArgumentNullException(nameof(userName));
        }

        userName = KeyNormalizer.NormalizeName(userName);
        var user = await _accountsDataService.FindByName(userName);

        return user;
    }

    public async Task<bool> CheckPassword(User user, string password)
    {
        if (user == null)
        {
            return false;
        }

        var result = await VerifyPassword(user, password);

        return result != PasswordVerificationResult.Failed;
    }

    public async Task<string> GetAuthenticationToken(User user, string tokenName)
        => await GetAuthenticationToken(user, LoginProvider, tokenName);

    public async Task<User> SetAuthenticationToken(User user, string tokenValue)
        => await SetAuthenticationToken(user, LoginProvider, AuthenticatorKeyTokenName, tokenValue);

    public async Task<User> RemoveAuthenticationToken(User user, string tokenName)
        => await RemoveAuthenticationToken(user, LoginProvider, tokenName);

    private async Task<bool> UpdatePasswordHash(User user, string newPassword)
    {

        var hash = newPassword != null ? _passwordHasher.HashPassword(user, newPassword) : null;
        user.SetHashedPassword(hash);
        return true;
    }

    private async Task<bool> CreateUser(User user)
    {
        // await UpdateNormalizedUserName(user);
        // await UpdateNormalizedEmail(user);

        return await _accountsDataService.CreateUser(user);
    }

    private Task<string> GetAuthenticationToken(User user, string loginProvider, string tokenName)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        if (loginProvider == null)
        {
            throw new ArgumentNullException(nameof(loginProvider));
        }

        if (tokenName == null)
        {
            throw new ArgumentNullException(nameof(tokenName));
        }

        return _accountsDataService.GetToken(user, loginProvider, tokenName);
    }

    private async Task<User> SetAuthenticationToken(User user, string loginProvider, string tokenName, string tokenValue)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        if (loginProvider == null)
        {
            throw new ArgumentNullException(nameof(loginProvider));
        }

        if (tokenName == null)
        {
            throw new ArgumentNullException(nameof(tokenName));
        }

        // REVIEW: should updating any tokens affect the security stamp?
        await _accountsDataService.SetToken(user, loginProvider, tokenName, tokenValue);
        return await UpdateUser(user);
    }

    private async Task<User> RemoveAuthenticationToken(User user, string loginProvider, string tokenName)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        if (loginProvider == null)
        {
            throw new ArgumentNullException(nameof(loginProvider));
        }

        if (tokenName == null)
        {
            throw new ArgumentNullException(nameof(tokenName));
        }

        await _accountsDataService.RemoveToken(user, loginProvider, tokenName);
        return await UpdateUser(user);
    }

    private async Task<PasswordVerificationResult> VerifyPassword(User user, string password)
    {
        var hash = await _accountsDataService.GetPasswordHash(user);
        if (hash == null)
        {
            return PasswordVerificationResult.Failed;
        }

        return _passwordHasher.VerifyHashedPassword(user, hash, password);
    }
}