using LifeyLife.Core.Contracts;
using LifeyLife.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LifeyLife.Api.Controllers
{
    [ApiController]
    [Route("api/random-dare-history")]
    public class RandomDareHistoryController : ControllerBase
    {
        private readonly IHistoryDataService _historyDataService;

        public RandomDareHistoryController(IHistoryDataService historyDataService)
        {
            _historyDataService = historyDataService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userId, out var userGuid))
            {
                return Unauthorized();
            }

            var history = await _historyDataService.ListHistory(userGuid);
            return Ok(history);
        }
    }
}