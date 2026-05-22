using LifeyLife.Core.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LifeyLife.Api.Controllers;

[ApiController]
[Route("api/character")]
[Authorize]
public class CharacterController : ControllerBase
{
    private readonly ICharacterDataService _characterDataService;

    public CharacterController(ICharacterDataService characterDataService)
    {
        _characterDataService = characterDataService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userId, out var userGuid))
            return BadRequest("Invalid user ID");

        var stats = await _characterDataService.GetOrCreateStats(userGuid);
        return Ok(stats);
    }
}
