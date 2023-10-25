using cbs_sports_api.Data;
using cbs_sports_api.Models;
using Microsoft.AspNetCore.Mvc;

namespace cbs_sports_api.Controllers;

[ApiController]
[Route("[controller]")]
public class PlayersController : ControllerBase
{
    private readonly ILogger<PlayersController> _logger;
	private readonly PlayerDbContext _context;

    public PlayersController(ILogger<PlayersController> logger, PlayerDbContext context)
    {
        _logger = logger;
		_context = context;
    }

    [HttpGet("{id}")]
    public ActionResult<Player> Get(int id)
    {
        var found = _context.Players.FirstOrDefault(p => p.Id == id);

        return found is null ? NotFound() : found;
    }

	[HttpPost("sync")]
    public IActionResult SyncData()
    {
        _context.Players.AddRange(
            new Player(123, "First", "Last"),
            new Player(456, "First 2", "Last 2")
        );
        _context.SaveChanges();
        return NoContent();
    }
}
