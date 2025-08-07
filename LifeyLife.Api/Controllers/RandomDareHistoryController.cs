using LifeyLife.Core.Contracts;
using LifeyLife.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace LifeyLife.Api.Controllers
{
    [ApiController]
    [Route("api/random-dare-history")]
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
        [Authorize]
        public async Task<IActionResult> Get()
        {
            // Debug logging to see what claims are available
            _logger.LogInformation("Available claims:");
            foreach (var claim in User.Claims)
            {
                _logger.LogInformation($"Claim Type: {claim.Type}, Value: {claim.Value}");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _logger.LogInformation($"ClaimTypes.NameIdentifier value: {userId}");

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest($"User ID claim not found. Available claims: {string.Join(", ", User.Claims.Select(c => $"{c.Type}={c.Value}"))}");
            }

            if (!Guid.TryParse(userId, out var userGuid))
            {
                return BadRequest($"Invalid user ID format: {userId}");
            }

            var history = await _historyDataService.ListHistory(userGuid);
            return Ok(history);
        }
    }
}