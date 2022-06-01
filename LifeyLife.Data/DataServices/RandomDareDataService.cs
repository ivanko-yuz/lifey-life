using System;
using System.Linq;
using System.Threading.Tasks;
using LifeyLife.Core.Contracts;
using LifeyLife.Core.Models;

namespace LifeyLife.Data.DataServices;

public class RandomDareDataService : IRandomDareDataService
{
    private readonly IDbAdapter _dbAdapter;

    public RandomDareDataService(IDbAdapter dbAdapter)
    {
        _dbAdapter = dbAdapter;
    }

    public async Task<RandomDare> GetRandomDare()
    {
        //TODO and type of language
        var query = $@"SELECT 
                            uuid as {nameof(RandomDare.Uuid)}, 
                            context as {nameof(RandomDare.Context)}, 
                            experience_gained as {nameof(RandomDare.ExperienceGained)}, 
                            given_time as {nameof(RandomDare.GivenTime)}
                        FROM public.random_dare;";

        var randomDare = (await _dbAdapter.GetMany<RandomDare>(query, new { })).ToList();
        return randomDare.ElementAt(new Random().Next(0, randomDare.Count - 1));
    }
}