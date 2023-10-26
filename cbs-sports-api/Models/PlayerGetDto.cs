namespace cbs_sports_api.Models;

public record PlayerGetDto(int Id, string FirstName, string LastName, string Position, int? Age, SportEnum Sport, double AveragePositionAgeDiff)
	: Player(Id, FirstName, LastName, Position, Age, Sport)
{
	public PlayerGetDto(Player player, double averagePositionAgeDiff)
	  : this(player.Id, player.FirstName, player.LastName, player.Position, player.Age, player.Sport, averagePositionAgeDiff)
	{
	}
}	