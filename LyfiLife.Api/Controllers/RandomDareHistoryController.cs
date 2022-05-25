using LyfiLife.Core.Contracts;
using LyfiLife.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LyfiLife.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RandomDareHistoryController : ControllerBase
    {
        private readonly ILogger<RandomDareHistoryController> _logger;
        private readonly IHistoryDataService _historyDataService;

        public RandomDareHistoryController(
            ILogger<RandomDareHistoryController> logger,
            IHistoryDataService historyDataService)
        {
            _logger = logger;
            _historyDataService = historyDataService;
        }

        [HttpGet]
        public async Task<IEnumerable<RandomDareHistory>> Get()
        {
            return await _historyDataService.ListHistory(Guid.NewGuid());
        }
    }
}