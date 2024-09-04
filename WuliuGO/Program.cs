using WuliuGO.GameLogic.utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    // var forecast =  Enumerable.Range(1, 5).Select(index =>
    //     new WeatherForecast
    //     (
    //         DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
    //         Random.Shared.Next(-20, 55),
    //         summaries[Random.Shared.Next(summaries.Length)]
    //     ))
    //     .ToArray();
    // return forecast;
    WuliuGO.GameLogic.GoGame goGame = new();
    Operation opt = new(
        Color.Black, new Position (0, 0)
    );
    Operation opt2 = new(
        Color.White, new Position (0, 1)
    );
    Operation opt3 = new(
        Color.White, new Position (1, 1)
    );
    Operation opt4 = new(
        Color.Black, new Position (1, 0)
    );
    Operation opt5 = new(
        Color.White, new Position (2, 0)
    );
    Operation opt6 = new(
        Color.Blank, new Revoke()
    );
    Console.WriteLine(goGame.GetBranch());
    goGame.PlayAction(opt);
    Console.WriteLine(goGame.GetBranch());
    goGame.PlayAction(opt2);
    Console.WriteLine(goGame.GetBranch());
    goGame.PlayAction(opt3);
    Console.WriteLine(goGame.GetBranch());
    goGame.PlayAction(opt4);
    Console.WriteLine(goGame.GetBranch());
    goGame.PlayAction(opt5);
    Console.WriteLine(goGame.GetBranch());
    goGame.PlayAction(opt6);
    Console.WriteLine(goGame.GetBranch());
    goGame.PlayAction(opt6);
    Console.WriteLine(goGame.GetBranch());
    goGame.PlayAction(opt6);
    Console.WriteLine(goGame.GetBranch());
    goGame.PlayAction(opt6);
    Console.WriteLine(goGame.GetBranch());
    goGame.PlayAction(opt6);
    Console.WriteLine(goGame.GetBranch());
    return goGame.GetBoard();
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
