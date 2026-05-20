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
                        WHERE language = @Language::language
                        ORDER BY random()
                        LIMIT 1;";

            var dare = await _dbAdapter.GetSingleOrDefault<RandomDare>(query, new { Language = language.ToString().ToLower() });

            if (dare is null)
            {
                _logger.LogWarning("No random dares found for language {Language}", language);
                if (language != LocalizationType.ua)
                {
                    _logger.LogInformation("Falling back to Ukrainian language dares");
                    return await GetRandomDareByLanguage(LocalizationType.ua);
                }
                throw new InvalidOperationException("No random dares available");
            }

            return dare;
        }
        catch (Exception ex) when (ex is not InvalidOperationException)
        {
            _logger.LogError(ex, "Error getting random dare for language {Language}", language);
            throw;
        }
    }
}