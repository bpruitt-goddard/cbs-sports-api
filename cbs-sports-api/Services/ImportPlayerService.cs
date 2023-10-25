using cbs_sports_api.Data;
using cbs_sports_api.Models;

namespace cbs_sports_api.Services;

public interface IImportPlayerService
{
	Task ImportPlayers();
}

public class ImportPlayerService : IImportPlayerService
{
	private readonly PlayerDbContext _context;
	private readonly ICbsApiService _cbsApi;

    public ImportPlayerService(PlayerDbContext context, ICbsApiService cbsApi)
    {
        _context = context;
        _cbsApi = cbsApi;
    }

    public async Task ImportPlayers()
    {
		var players = await _cbsApi.GetPlayersBySport(SportEnum.Baseball);
        _context.Players.AddRange(players.ToList());
        _context.SaveChanges();
    }
}