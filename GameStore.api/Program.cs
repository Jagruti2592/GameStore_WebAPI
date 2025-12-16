using GameStore.api.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string GetGameEndPointName = "GetGame";
List<GameDto> games = [
    new(1, "Street Fighter II", "Fighting", 29.99m, new DateOnly(1991, 2, 21)),
    new(2, "The Legend of Zelda: Ocarina of Time", "Adventure", 39.99m, new DateOnly(1998, 11, 21)),
    new(3, "Final Fantasy VII", "RPG", 49.99m, new DateOnly(1997, 1, 31)),
    new(4, "FIFA"," Sports", 59.99m, new DateOnly(1993, 12, 15))
];

// GET /games
app.MapGet("games", () => games);

// GET /games/{id}
app.MapGet("games/{id}", (int id) =>
{
    GameDto? game = games.Find(game => game.Id == id);
    return game is null ? Results.NotFound() : Results.Ok(game);
}).WithName(GetGameEndPointName);

//POST /games
app.MapPost("games", (CreateGameDto newGame) =>
{
    GameDto game = new(
        games.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate
    );
    games.Add(game);
    return Results.CreatedAtRoute(GetGameEndPointName, new { id = game.Id }, game);
});

//PUT /games/{id}
app.MapPut("games/{id}", (int id, UpdateGameDto updateGame) =>
{
    var index = games.FindIndex(game => game.Id == id);
    if (index == -1)
    {
        return Results.NotFound();
    }

    games[index] = new GameDto
    (
        id,
        updateGame.Name,
        updateGame.Genre,
        updateGame.Price,
        updateGame.ReleaseDate
    );
    return Results.NoContent();
});

// DELETE /games/{id}
app.MapDelete("games/{id}", (int id) =>
{
    var index = games.FindIndex(game => game.Id == id);
    if (index == -1)
    {
        return Results.NotFound();
    }
    games.RemoveAt(index);
    return Results.NoContent();
});


//app.MapGet("/", () => "Hello World!");

app.Run();
