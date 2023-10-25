using cbs_sports_api.Models;

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
        var response = await _client.GetAsync("players/list?version=3.0&SPORT=baseball&response_format=JSON");
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