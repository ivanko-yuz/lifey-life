using LifeyLife.Core.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeyLife.Api.Controllers;

[ApiController]
[Route("api/leaderboard")]
[Authorize]
public class LeaderboardController : ControllerBase
{
    private readonly ILeaderboardDataService _leaderboardDataService;

    public LeaderboardController(ILeaderboardDataService leaderboardDataService)
    {
        _leaderboardDataService = leaderboardDataService;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int limit = 50)
    {
        if (limit is <= 0 or > 100) limit = 50;
        var entries = await _leaderboardDataService.GetTopPlayers(limit);
        return Ok(entries);
    }
}
