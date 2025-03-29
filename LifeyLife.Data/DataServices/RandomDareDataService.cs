using LifeyLife.Core.Contracts;
using LifeyLife.Core.Models;
using Microsoft.Extensions.Logging;

namespace LifeyLife.Data.DataServices;

public class RandomDareDataService : IRandomDareDataService
{
    private readonly IDbAdapter _dbAdapter;
    private readonly ILogger<RandomDareDataService> _logger;

    public RandomDareDataService(IDbAdapter dbAdapter, ILogger<RandomDareDataService> logger)
    {
        _dbAdapter = dbAdapter;
        _logger = logger;
    }

    public async Task<RandomDare> GetRandomDare()
    {
        try
        {
            var query = $@"SELECT 
                            uuid as {nameof(RandomDare.Uuid)}, 
                            context as {nameof(RandomDare.Context)}, 
                            experience_gained as {nameof(RandomDare.ExperienceGained)}, 
                            given_time as {nameof(RandomDare.GivenTime)}
                        FROM public.random_dare;";

            var randomDares = (await _dbAdapter.GetMany<RandomDare>(query, new { })).ToList();
            
            if (!randomDares.Any())
            {
                _logger.LogWarning("No random dares found in the database");
                throw new InvalidOperationException("No random dares available");
            }

            return randomDares.ElementAt(new Random().Next(0, randomDares.Count));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting random dare");
            throw;
        }
    }
}