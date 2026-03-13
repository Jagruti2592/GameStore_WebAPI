using System;
using GameStore.Api.Dtos;

namespace GameStore.Api.EndPoints;

public static class GamesEndPoints
{
    const string GetGameEndpointName = "GetGame";
    private static readonly List<GameDto> games = [
    new(
        1,
        "The Legend of Zelda: Breath of the Wild",
        "Action-adventure",
        59.99m,
        new DateOnly(2017, 3, 3)
    ),
    new(
        2,
        "Super Mario Odyssey",
        "Platform",
        59.99m,
        new DateOnly(2017, 10, 27)
    ),
    new(
        3,
        "Red Dead Redemption 2",
        "Action-adventure",
        59.99m,
        new DateOnly(2018, 10, 26)
    ),
    new(
        4,
        "The Witcher 3: Wild Hunt",
        "Action RPG",
        39.99m,
        new DateOnly(2015, 5, 19)
    ),
    new(
        5,
        "Minecraft",
        "Sandbox",
        26.95m,
        new DateOnly(2011, 11, 18)
    )
];

  //Extension menthod
  public static void MapGamesEndpoints(this WebApplication app)
    {

        var group = app.MapGroup("/games");

        //GET: api/games
group.MapGet("", () => games);



//GET: api/games/{id}
group.MapGet("/{id}", (int id) =>
{
    var game = games.Find(g => g.Id == id);
    return games is null ? Results.NotFound() : Results.Ok(game);
})
.WithName(GetGameEndpointName);

//POST: api/games
group.MapPost("", (CreateGameDto newGame) =>
{
    GameDto game = new(
        games.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate
    );
    games.Add(game);
    return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
});

//PUT: api/games/{id}
group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
{
    var index = games.FindIndex(g => g.Id == id);
    if (index == -1)
    {
        return Results.NotFound();
    }
    games[index] = new GameDto
    (
        id,
        updatedGame.Name,
        updatedGame.Genre,
        updatedGame.Price,
        updatedGame.ReleaseDate
    );

    return Results.NoContent();
});


//DELETE: api/games/{id}
group.MapDelete("/{id}", (int id) =>
{
    var index = games.FindIndex(g => g.Id == id);
    games.RemoveAt(index);

    return Results.NoContent();
});
    }

}
