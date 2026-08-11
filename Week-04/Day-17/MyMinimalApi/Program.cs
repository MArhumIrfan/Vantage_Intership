using System.Buffers;
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");


app.MapGet("/api/welcome", () =>
{
    return Results.Ok(new { Message = "Welcome to the integrated Library Web API!", Timestamp = DateTime.Now });
});




// Endpoint using query parameters: /api/calc?op=add&num1=10&num2=5
app.MapGet("/api/calc", (string op, double num1, double num2) =>
{
    double result = op.ToLower() switch
    {
        "add" or "+" => num1 + num2,
        "subtract" or "-" => num1 - num2,
        "multiply" or "*" => num1 * num2,
        "divide" or "/" => num2 != 0 ? num1 / num2 : double.NaN,
        _ => double.NaN
    };

    if (double.IsNaN(result))
    {
        return Results.BadRequest(new { Error = "Invalid operation or division by zero." });
    }

    return Results.Ok(new { Operation = op, Num1 = num1, Num2 = num2, Result = result });
});

app.Run();


record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
