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
        // Default to Ukrainian if no language preference is specified
        return await GetRandomDareByLanguage(LocalizationType.ua);
    }

    public async Task<RandomDare> GetRandomDareByLanguage(LocalizationType language)
    {
        try
        {
            var query = $@"SELECT 
                            uuid as {nameof(RandomDare.Uuid)},
                            language as {nameof(RandomDare.Language)}, 
                            context as {nameof(RandomDare.Context)}, 
                            experience_gained as {nameof(RandomDare.ExperienceGained)}, 
                            given_time as {nameof(RandomDare.GivenTime)}
                        FROM public.random_dare
                        WHERE language = @Language::language;";

            var randomDares = (await _dbAdapter.GetMany<RandomDare>(query, new { Language = language.ToString().ToLower() })).ToList();
            
            if (!randomDares.Any())
            {
                _logger.LogWarning("No random dares found for language {Language}", language);
                // Fallback to Ukrainian if no dares found for the specified language
                if (language != LocalizationType.ua)
                {
                    _logger.LogInformation("Falling back to Ukrainian language dares");
                    return await GetRandomDareByLanguage(LocalizationType.ua);
                }
                throw new InvalidOperationException("No random dares available");
            }

            return randomDares.ElementAt(new Random().Next(0, randomDares.Count));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting random dare for language {Language}", language);
            throw;
        }
    }
}