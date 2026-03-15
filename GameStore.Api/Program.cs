using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.EndPoints;
using GameStore.Api.Models;
using Microsoft.AspNetCore.Http.HttpResults;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();

builder.AddGameStoreDb();
  
var app = builder.Build();

app.MapGamesEndpoints();

app.MigrateDb();

app.Run();
