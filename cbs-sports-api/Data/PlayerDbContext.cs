using cbs_sports_api.Models;
using Microsoft.EntityFrameworkCore;

namespace cbs_sports_api.Data;

public class PlayerDbContext : DbContext
{
	public PlayerDbContext(DbContextOptions<PlayerDbContext> options) : base(options)
	{
	}

    public DbSet<Player> Players { get; set; }
}