namespace cbs_sports_api.Models;

public record Player(int Id, string FirstName, string LastName, string Position, int? Age, SportEnum sport);