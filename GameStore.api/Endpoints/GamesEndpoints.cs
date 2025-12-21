using System;
using GameStore.api.Dtos;
using GameStore.api.Data;
using GameStore.api.Entities;
using GameStore.api.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndPointName = "GetGame";
    // private static readonly List<GameSummaryDto> games = [
    // new(1, "Street Fighter II", "Fighting", 29.99m, new DateOnly(1991, 2, 21)),
    // new(2, "The Legend of Zelda: Ocarina of Time", "Adventure", 39.99m, new DateOnly(1998, 11, 21)),
    // new(3, "Final Fantasy VII", "RPG", 49.99m, new DateOnly(1997, 1, 31)),
    // new(4, "FIFA"," Sports", 59.99m, new DateOnly(1993, 12, 15))
    // ];

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {

        var group = app.MapGroup("games")
            ;

        // GET /games
        group.MapGet("/", async (GameStoreContext dbContext) =>

         await dbContext.Games
         .Include(game => game.Genre)
         .Select(game => game.ToGameSummaryDto())
         .AsNoTracking()
         .ToListAsync()
        );

        // GET /games/{id}
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            Game? game = await dbContext.Games.FindAsync(id);
            return game is null ?
            Results.NotFound() : Results.Ok(game.ToGameDetailsDto());
        })
        .WithName(GetGameEndPointName);

        //POST /games
        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            Game game = newGame.ToEntity();
            //game.Genre = dbContext.Genres.Find(newGame.GenreId);

            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();

            return Results.CreatedAtRoute(
                GetGameEndPointName,
                new { id = game.Id },
                game.ToGameDetailsDto());
        });


        //PUT /games/{id}
        group.MapPut("/{id}", async (int id, UpdateGameDto updateGame, GameStoreContext dbContext) =>
        {
            var existingGame = await dbContext.Games.FindAsync(id);
            if (existingGame is null)
            {
                return Results.NotFound();
            }
            dbContext.Entry(existingGame).CurrentValues.SetValues(updateGame.ToEntity(id));
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        // DELETE /games/{id}
        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            await dbContext.Games
            .Where(game => game.Id == id)
            .ExecuteDeleteAsync();

            return Results.NoContent();
        });


        return group;
    }

}
