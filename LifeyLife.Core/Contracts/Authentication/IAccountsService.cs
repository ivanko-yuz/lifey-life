using System.Security.Claims;
using LifeyLife.Core.Models;

namespace LifeyLife.Core.Contracts.Authentication;

public interface IAccountsService
{
    Task<bool> CreateUser(User user, string password);
    Task<User> UpdateUser(User user);

    Task<User> FindByName(string userName);

    Task<bool> CheckPassword(User user, string password);

    Task<string> GetAuthenticationToken(User user, string tokenName);
    Task<User> SetAuthenticationToken(User user, string tokenValue);
    Task<User> RemoveAuthenticationToken(User user, string tokenName);

    string GetUserName(ClaimsPrincipal principal);
    string GetUserId(ClaimsPrincipal principal);
    Task<User> GetUser(ClaimsPrincipal principal);
}