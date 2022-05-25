using LyfiLife.Core.Contracts;
using LyfiLife.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LyfiLife.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IHistoryDataService _historyDataService;

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            IHistoryDataService historyDataService)
        {
            _logger = logger;
            _historyDataService = historyDataService;
        }

        [HttpGet]
        public async Task<IEnumerable<HistoryRecord>> Get()
        {
            return await _historyDataService.GetAllHistory();
        }
    }
}