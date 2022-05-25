using LyfiLife.Core.Contracts;
using LyfiLife.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LyfiLife.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RandomDareController : ControllerBase
    {
        private readonly ILogger<RandomDareController> _logger;
        private readonly IRandomDareDataService _randomDareDataService;

        public RandomDareController(
            ILogger<RandomDareController> logger,
            IRandomDareDataService randomDareDataService)
        {
            _logger = logger;
            _randomDareDataService = randomDareDataService;
        }

        [HttpGet]
        public async Task<RandomDare> Get()
        {
            return await _randomDareDataService.GetRandomDare();
        }
    }
}