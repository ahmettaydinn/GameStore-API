using GameStore.Api;
using GameStore.Api.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string GetGameEndpointName = "GetGame";

List<GameDto> games = [
  new(
    1,
    "Street Fighter",
    "Fighting",
    19.99M,
    new DateOnly(1992,7, 15)),
      new(
    2,
    "Street Kisser",
    "Kiss",
    20.99M,
    new DateOnly(1993,7, 15)),
      new(
    3,
    "FIFA 23",
    "Football",
    21.99M,
    new DateOnly(1994,7, 15))
];

app.MapGet("games", ()=> games);

app.MapGet("/", () => "Hello World!");

app.MapGet("games/{id}", (int id) =>
{
  GameDto? game = games.Find(game => game.Id == id);

  return game is null ? Results.NotFound() : Results.Ok(game);
 })
 .WithName(GetGameEndpointName);

app.MapPost("games", (CreateGameDto newGame)=> {
  GameDto game = new(
  games.Count + 1,
  newGame.Name,
  newGame.Genre,
  newGame.Price,
  newGame.ReleaseDate);

  games.Add(game);

  return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
});

app.MapPut("games/{id}", (int id, UpdateGameDto updatedGame)=>
 {
  var index = games.FindIndex(game => game.Id == id);

  if(index == -1){
    return Results.NotFound();
  }

  games[index] = new GameDto(
    id,
    updatedGame.Name,
    updatedGame.Genre,
    updatedGame.Price,
    updatedGame.ReleaseDate
  );

  return Results.NoContent();
});

app.MapDelete("games/{id}", (int id)=>{
  games.RemoveAll(game => game.Id == id);

  return Results.NoContent();
});

app.Run();
