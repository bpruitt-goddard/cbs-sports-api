using cbs_sports_api.Data;
using cbs_sports_api.Models;
using cbs_sports_api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

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

    [HttpGet("{sport}/{id}")]
    public ActionResult<PlayerGetDto> Get(SportEnum sport, int id)
    {
        var found = _context.Players.FirstOrDefault(p => p.Sport == sport && p.Id == id);

        if (found is null)
        {
            return NotFound();
        }

        double positionAgeDifference = GetPositionAgeDifference(found);

        return new PlayerGetDto(found, positionAgeDifference);

        double GetPositionAgeDifference(Player player)
        {
            var averagePositionAge = _context.Players
                .Where(p => p.Sport == player.Sport && p.Position == player.Position)
                .Average(p => p.Age) ?? 0.00;
            return Math.Abs((player.Age ?? 0.00) - averagePositionAge);
        }
    }

    [HttpPost("sync")]
    public async Task<IActionResult> SyncData()
    {
        await _importPlayerService.ImportPlayers();
        return NoContent();
    }
}
