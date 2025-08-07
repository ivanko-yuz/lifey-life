using System.Security.Claims;
using LifeyLife.Core.Models;

namespace LifeyLife.Core.Contracts.Authentication;

public interface IAccountsService
{
    Task<bool> CreateUser(User user, string password);
    Task<User> FindByName(string userName);
    Task<bool> CheckPassword(User user, string password);
    string GetUserName(ClaimsPrincipal principal);
    string GetUserId(ClaimsPrincipal principal);
    Task<User> GetUser(ClaimsPrincipal principal);
}