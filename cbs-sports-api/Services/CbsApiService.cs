using cbs_sports_api.Models;
using Microsoft.AspNetCore.WebUtilities;

namespace cbs_sports_api.Services;

public interface ICbsApiService
{
	Task<IEnumerable<Player>> GetPlayersBySport(SportEnum sport);
}

public record PlayerResponse(string UriAlias, string StatusMessage, int StatusCode, PlayerResponseBody Body);
public record PlayerResponseBody(List<PlayerResponsePlayer> Players);
public record PlayerResponsePlayer(int Id, string FirstName, string LastName, string Position, int? Age)
{
    public Player ToPlayerModel(SportEnum sport) => new(Id, FirstName, LastName, Position, Age, sport);
}

public class CbsApiService : ICbsApiService
{
	private readonly HttpClient _client;
	public CbsApiService(HttpClient client)
	{
		_client = client;
	}

    public async Task<IEnumerable<Player>> GetPlayersBySport(SportEnum sport)
    {
		Dictionary<string, string> queryString = new()
		{
			["version"] = "3.0",
			// Cbs Api fails if this is PascalCase so it needs to be lower case
			["sport"] = sport.ToString().ToLower(),
			["response_format"] = "json",
		};
        var response = await _client.GetAsync(QueryHelpers.AddQueryString("players/list", queryString));
		if (!response.IsSuccessStatusCode)
		{
			// todo handle error case
			return null;
		}
		
		var model = await response.Content.ReadFromJsonAsync<PlayerResponse>();
		return model!.Body.Players
			.Select(p => p.ToPlayerModel(sport));
    }
}