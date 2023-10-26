namespace cbs_sports_api.Models;

public record PlayerSearchDto(SportEnum? Sport, char? FirstLetterLastName, int? MinAge, int? MaxAge, string? Position);