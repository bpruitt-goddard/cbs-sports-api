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
		// Delete old data
		// Note with a proper relational provider we can leverage new EF Core 7
		// await _context.Players.ExecuteDeleteAsync();

		// For in-memory we have to do it manually instead
		_context.Players.RemoveRange(_context.Players);
		_context.SaveChanges();
        
		// Insert new data
		var playerResults = await Task.WhenAll(
			_cbsApi.GetPlayersBySport(SportEnum.Baseball),
			_cbsApi.GetPlayersBySport(SportEnum.Basketball),
			_cbsApi.GetPlayersBySport(SportEnum.Football)
		);

		IEnumerable<Player> entities = playerResults.SelectMany(p => p);
		_context.Players.AddRange(entities);
		_context.SaveChanges();
    }
}