namespace cbs_sports_api.Models;

public record PlayerGetDto(int Id, string FirstName, string LastName, string Position, int? Age, SportEnum Sport, double AveragePositionAgeDiff)
{
	public PlayerGetDto(Player player, double averagePositionAgeDiff)
	  : this(player.Id, player.FirstName, player.LastName, player.Position, player.Age, player.Sport, averagePositionAgeDiff)
	{
	}

	public string NameBrief {
		get => Sport switch
		{
			SportEnum.Baseball => $"{InitialFromString(FirstName)}. {InitialFromString(LastName)}.",
			SportEnum.Basketball => $"{FirstName} {InitialFromString(LastName)}.",
			SportEnum.Football => $"{InitialFromString(FirstName)}. {LastName}",
		};
	}

	public static string InitialFromString(string s) => s.Substring(0,1);
}	