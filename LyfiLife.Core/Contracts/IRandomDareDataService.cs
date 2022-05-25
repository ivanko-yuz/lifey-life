using LyfiLife.Core.Models;

namespace LyfiLife.Core.Contracts;

public interface IRandomDareDataService
{
    public Task<RandomDare> GetRandomDare();
}