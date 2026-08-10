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

// Example custom GET endpoint
app.MapGet("/api/welcome", () =>
{
    return Results.Ok(new { Message = "Welcome to the integrated Library Web API!", Timestamp = DateTime.Now });
});


app.MapGet("/api/calc/{operation}/{num1:double}/{num2:double}", (string operation, double num1, double num2) =>
{
    double result = operation.ToLower() switch
    {
        "add" => num1 + num2,
        "subtract" => num1 - num2,
        "multiply" => num1 * num2,
        "divide" => num2 != 0 ? num1 / num2 : double.NaN,
        _ => 0
    };

    if (double.IsNaN(result))
    {
        return Results.BadRequest(new { Error = "Cannot divide by zero." });
    }

    return Results.Ok(new { Calculation = $"{num1} {operation} {num2}", Result = result });
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
