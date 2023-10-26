using System.Text.Json.Serialization;

namespace cbs_sports_api.Models;

public record PlayerGetDto
{
	public int Id { get; init; }
	[JsonPropertyName("first_name")]
	public string FirstName { get; init; }
	[JsonPropertyName("last_name")]
	public string LastName { get; init; }
	public string Position { get; init; }
	public int? Age { get; init; }
	[JsonIgnore]
	public SportEnum Sport { get; init; }
	[JsonPropertyName("average_position_age_diff")]
	public double AveragePositionAgeDiff { get; init; }

	public PlayerGetDto(Player player, double averagePositionAgeDiff)
	{
		Id = player.Id;
		FirstName = player.FirstName;
		LastName = player.LastName;
		Position = player.Position;
		Age = player.Age;
		Sport = player.Sport;
		AveragePositionAgeDiff = averagePositionAgeDiff;
	}

	[JsonPropertyName("name_brief")]
	public string NameBrief {
		get => Sport switch
		{
			SportEnum.Baseball => $"{FirstCharacter(FirstName)}. {FirstCharacter(LastName)}.",
			SportEnum.Basketball => $"{FirstName} {FirstCharacter(LastName)}.",
			SportEnum.Football => $"{FirstCharacter(FirstName)}. {LastName}",
		};
	}

	private static string FirstCharacter(string s) => s.Substring(0,1);
}	