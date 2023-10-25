using cbs_sports_api.Data;
using cbs_sports_api.Models;
using cbs_sports_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace cbs_sports_api.Controllers;

[ApiController]
[Route("[controller]")]
public class PlayersController : ControllerBase
{
    private readonly ILogger<PlayersController> _logger;
	private readonly PlayerDbContext _context;
    private readonly IImportPlayerService _importPlayerService;

    public PlayersController(ILogger<PlayersController> logger, PlayerDbContext context, IImportPlayerService importPlayerService)
    {
        _logger = logger;
        _context = context;
        _importPlayerService = importPlayerService;
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
        _importPlayerService.ImportPlayers();
        return NoContent();
    }
}
