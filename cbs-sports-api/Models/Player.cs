using Microsoft.EntityFrameworkCore;

namespace cbs_sports_api.Models;

[PrimaryKey(nameof(Id), nameof(Sport))]
public record Player(int Id, string FirstName, string LastName, string Position, int? Age, SportEnum Sport)
{
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