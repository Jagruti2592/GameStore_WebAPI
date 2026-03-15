

namespace GameStore.Api.Dtos;

public record GamesDetailsDto
(
    int Id,
    string Name,
    int GenreId,
    decimal Price,
    DateOnly ReleaseDate
);
