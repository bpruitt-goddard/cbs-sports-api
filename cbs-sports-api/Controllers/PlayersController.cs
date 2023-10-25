using cbs_sports_api.Data;
using cbs_sports_api.Models;
using Microsoft.AspNetCore.Mvc;

namespace cbs_sports_api.Controllers;

[ApiController]
[Route("[controller]")]
public class PlayersController : ControllerBase
{
    private readonly ILogger<PlayersController> _logger;
    public PlayersController(ILogger<PlayersController> logger)
    {
        _logger = logger;
    }

    [HttpGet("{id}")]
    public Player Get(int id)
    {
        return new Player(id, "First Name", "Last Name");
    }

	[HttpPost("sync")]
    public IActionResult SyncData()
    {
        return NoContent();
    }
}
