using LifeyLife.Core.Models;

namespace LifeyLife.Core.Contracts.Authentication;

public interface IAccountsDataService
{
    Task<User> FindByName(string userName);
    Task<User> FindById(Guid uuid);
    Task<User> Update(User user);
    //Task SetPasswordHashAsync(User user, string? hash);
    Task<bool> CreateUser(User user);
    Task<string> GetToken(User user, string loginProvider, string tokenName);
    Task SetToken(User user, string loginProvider, string tokenName, string tokenValue);
    Task RemoveToken(User user, string loginProvider, string tokenName);
    Task<string> GetPasswordHash(User user);
}