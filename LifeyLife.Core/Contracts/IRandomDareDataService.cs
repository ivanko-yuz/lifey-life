using LifeyLife.Core.Models;

namespace LifeyLife.Core.Contracts;

public interface IRandomDareDataService
{
    public Task<RandomDare> GetRandomDare();
    public Task<RandomDare> GetRandomDareByLanguage(LocalizationType language);
}