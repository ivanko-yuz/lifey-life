using LifeyLife.Core.Contracts;
using LifeyLife.Core.Contracts.Authentication;
using LifeyLife.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace LifeyLife.Api.Controllers
{
    [ApiController]
    [Route("api/random-dare")]
    public class RandomDareController : ControllerBase
    {
        private readonly ILogger<RandomDareController> _logger;
        private readonly IRandomDareDataService _randomDareDataService;
        private readonly IHistoryDataService _historyDataService;
        private readonly IAccountsDataService _accountsDataService;

        public RandomDareController(
            ILogger<RandomDareController> logger,
            IRandomDareDataService randomDareDataService, 
            IHistoryDataService historyDataService,
            IAccountsDataService accountsDataService)
        {
            _logger = logger;
            _randomDareDataService = randomDareDataService;
            _historyDataService = historyDataService;
            _accountsDataService = accountsDataService;
        }

        [HttpGet]
        public async Task<RandomDare> Get([FromQuery] LocalizationType? language = null)
        {
            try
            {
                LocalizationType targetLanguage;

                if (language.HasValue)
                {
                    // Use the explicitly requested language
                    targetLanguage = language.Value;
                }
                else
                {
                    // Get user's preferred language from their profile if authenticated
                    if (User.Identity?.IsAuthenticated == true)
                    {
                        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        if (Guid.TryParse(userId, out var userGuid))
                        {
                            var user = await _accountsDataService.FindById(userGuid);
                            targetLanguage = user?.PreferredLanguage ?? LocalizationType.ua;
                        }
                        else
                        {
                            targetLanguage = LocalizationType.ua;
                        }
                    }
                    else
                    {
                        // Default to Ukrainian for unauthenticated users
                        targetLanguage = LocalizationType.ua;
                    }
                }

                return await _randomDareDataService.GetRandomDareByLanguage(targetLanguage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting random dare");
                // Fallback to default method if there's an error
                return await _randomDareDataService.GetRandomDare();
            }
        }
        
        [HttpPost]
        [Route("Complete")]
        public async Task Complete([FromBody]RandomDare randomDare)
        {
            await _historyDataService.SaveCompletedRandomDareInHistory(Guid.NewGuid(), randomDare.Uuid);
        }
        
        [HttpPost]
        [Route("Skip")]
        public async Task Skip([FromBody]RandomDare randomDare)
        {
            await _historyDataService.SaveSkippedRandomDareInHistory(Guid.NewGuid(), randomDare.Uuid);
        }
    }
}