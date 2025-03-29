using System.Security.Claims;
using LifeyLife.Core.Contracts.Authentication;
using LifeyLife.Core.Models;
using LifeyLife.Core.Utils;

namespace LifeyLife.Core.Services;

public class AccountsService : IAccountsService
{
    private readonly IAccountsDataService _accountsDataService;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AccountsService(
        IAccountsDataService accountsDataService,
        IPasswordHasher<User> passwordHasher)
    {
        _accountsDataService = accountsDataService;
        _passwordHasher = passwordHasher;
    }

    public string GetUserName(ClaimsPrincipal principal)
    {
        if (principal == null)
        {
            throw new ArgumentNullException(nameof(principal));
        }

        return principal.FindFirstValue(ClaimTypes.NameIdentifier) ?? 
               throw new InvalidOperationException("User name claim not found");
    }

    public string GetUserId(ClaimsPrincipal principal)
    {
        if (principal == null)
        {
            throw new ArgumentNullException(nameof(principal));
        }

        return principal.FindFirstValue(ClaimTypes.NameIdentifier) ?? 
               throw new InvalidOperationException("User ID claim not found");
    }

    public async Task<User> GetUser(ClaimsPrincipal principal)
    {
        if (principal == null)
        {
            throw new ArgumentNullException(nameof(principal));
        }

        var id = GetUserId(principal);
        return await _accountsDataService.FindById(Guid.Parse(id));
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

        var hash = _passwordHasher.HashPassword(user, password);
        user.SetHashedPassword(hash);
        return await _accountsDataService.CreateUser(user);
    }

    public async Task<User> FindByName(string userName)
    {
        if (userName == null)
        {
            throw new ArgumentNullException(nameof(userName));
        }

        userName = KeyNormalizer.NormalizeName(userName);
        return await _accountsDataService.FindByName(userName);
    }

    public async Task<bool> CheckPassword(User user, string password)
    {
        if (user == null)
        {
            return false;
        }

        var hash = await _accountsDataService.GetPasswordHash(user);
        if (hash == null)
        {
            return false;
        }

        return _passwordHasher.VerifyHashedPassword(user, hash, password) != PasswordVerificationResult.Failed;
    }
}