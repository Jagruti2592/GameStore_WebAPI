using System;
using GameStore.api.Dtos;
using GameStore.api.Data;
using GameStore.api.Entities;
using GameStore.api.Mapping;

namespace GameStore.api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndPointName = "GetGame";
    private static readonly List<GameDto> games = [
        new(1, "Street Fighter II", "Fighting", 29.99m, new DateOnly(1991, 2, 21)),
    new(2, "The Legend of Zelda: Ocarina of Time", "Adventure", 39.99m, new DateOnly(1998, 11, 21)),
    new(3, "Final Fantasy VII", "RPG", 49.99m, new DateOnly(1997, 1, 31)),
    new(4, "FIFA"," Sports", 59.99m, new DateOnly(1993, 12, 15))
    ];

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {

        var group = app.MapGroup("games")
            ;

        // GET /games
        group.MapGet("/", () => games);

        // GET /games/{id}
        group.MapGet("/{id}", (int id,GameStoreContext dbContext) =>
        {
            Game? game = dbContext.Games.Find(id);
            return game is null ? Results.NotFound() : Results.Ok(game);
        })
        .WithName(GetGameEndPointName);

        //POST /games
        group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            Game game = newGame.ToEntity();
            game.Genre = dbContext.Genres.Find(newGame.GenreId);



            dbContext.Games.Add(game);
            dbContext.SaveChanges();



            return Results.CreatedAtRoute(
                GetGameEndPointName,
                new { id = game.Id },
                game.ToDto());
        });


        //PUT /games/{id}
        group.MapPut("/{id}", (int id, UpdateGameDto updateGame) =>
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
        group.MapDelete("/{id}", (int id) =>
        {
            var index = games.FindIndex(game => game.Id == id);
            if (index == -1)
            {
                return Results.NotFound();
            }
            games.RemoveAt(index);
            return Results.NoContent();
        });


        return group;
    }

}
