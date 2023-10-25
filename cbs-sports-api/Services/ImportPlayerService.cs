using cbs_sports_api.Data;
using cbs_sports_api.Models;

namespace cbs_sports_api.Services;

public interface IImportPlayerService
{
	void ImportPlayers();
}

public class ImportPlayerService : IImportPlayerService
{
	private readonly PlayerDbContext _context;

    public ImportPlayerService(PlayerDbContext context)
    {
        _context = context;
    }

    public void ImportPlayers()
    {
        _context.Players.AddRange(
            new Player(123, "First", "Last"),
            new Player(456, "First 2", "Last 2")
        );
        _context.SaveChanges();
    }
}