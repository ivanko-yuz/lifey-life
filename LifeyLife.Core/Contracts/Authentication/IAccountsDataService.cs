using LifeyLife.Core.Models;

namespace LifeyLife.Core.Contracts.Authentication;

public interface IAccountsDataService
{
    Task<User> FindByName(string userName);
    Task<User> FindById(Guid uuid);
    Task<bool> CreateUser(User user);
    Task<string> GetPasswordHash(User user);
}