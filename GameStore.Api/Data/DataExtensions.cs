using System;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public static class DataExtensions
{
    
    public static void MigrateDb(this WebApplication app){
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();

    dbContext.Database.Migrate();
    }

    public static void AddGameStoreDb(this WebApplicationBuilder builder)
    {
        var connString = builder.Configuration.GetConnectionString("GameStore");

        builder.Services.AddScoped<GameStoreContext>(); 

        builder.Services.AddSqlite<GameStoreContext>(
            connString,
            optionsAction: options => options.UseSeeding((context,_) =>
            {
                if(!context.Set<Genre>().Any())
                 {
                     context.Set<Genre>().AddRange(
                         new Genre { Name = "Action-adventure" },
                         new Genre { Name = "Platform" },
                         new Genre { Name = "Action RPG" },
                         new Genre { Name = "Sandbox" }
                    );
                   context.SaveChanges();
                 }
            })
        );
    }
}
