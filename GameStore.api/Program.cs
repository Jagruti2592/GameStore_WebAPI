
using GameStore.api.Endpoints;
using GameStore.api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("GameStore");
builder.Services.AddSqlite<GameStoreContext>(connString);
builder.Services.AddScoped<GameStoreContext>();

var app = builder.Build();

app.MapGamesEndpoints();

//app.MapGet("/", () => "Hello World!");

await app.MigrateDbAsync();

app.Run();

// Design-time factory for EF Core migrations
public class GameStoreContextFactory : IDesignTimeDbContextFactory<GameStoreContext>
{
    public GameStoreContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var options = new DbContextOptionsBuilder<GameStoreContext>()
            .UseSqlite(configuration.GetConnectionString("GameStore"))
            .Options;

        return new GameStoreContext(options);
    }
}
