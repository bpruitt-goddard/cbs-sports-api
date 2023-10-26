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
    }

    [HttpPost("sync")]
    public async Task<IActionResult> SyncData()
    {
        await _importPlayerService.ImportPlayers();
        return NoContent();
    }

    [HttpPost("search")]
    public ActionResult<PlayerGetDto> SearchPlayer(PlayerSearchDto search)
    {
        IQueryable<Player> query = BuildSearch(search);

        var result = query.FirstOrDefault();
        if (result is null)
        {
            return NotFound();
        }

        double positionAgeDifference = GetPositionAgeDifference(result);

        return new PlayerGetDto(result, positionAgeDifference);

        // This can be extracted to a search service
        IQueryable<Player> BuildSearch(PlayerSearchDto search)
        {
            IQueryable<Player> query = _context.Players;

            if (search.Sport is not null)
            {
                query = query.Where(p => p.Sport == search.Sport);
            }

            if (search.FirstLetterLastName is not null)
            {
                query = query.Where(p => p.LastName.StartsWith(search.FirstLetterLastName.Value));
            }

            if (search.MinAge is not null)
            {
                query = query.Where(p => p.Age >= search.MinAge);
            }

            if (search.MaxAge is not null)
            {
                query = query.Where(p => p.Age <= search.MaxAge);
            }

            if (search.Position is not null)
            {
                query = query.Where(p => p.Position == search.Position);
            }

            return query;
        }
    }

    private double GetPositionAgeDifference(Player player)
    {
        var averagePositionAge = _context.Players
            .Where(p => p.Sport == player.Sport && p.Position == player.Position)
            .Average(p => p.Age) ?? 0.00;
        return Math.Abs((player.Age ?? 0.00) - averagePositionAge);
    }
}
