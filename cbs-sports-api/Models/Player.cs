using Microsoft.EntityFrameworkCore;

namespace cbs_sports_api.Models;

[PrimaryKey(nameof(Id), nameof(Sport))]
public record Player(int Id, string FirstName, string LastName, string Position, int? Age, SportEnum Sport);