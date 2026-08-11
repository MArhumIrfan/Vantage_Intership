using System.Buffers;
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

// Existing GET Endpoints
app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
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

// ==========================================
// NEW: POST Endpoint Using Request/Response Models
// ==========================================
app.MapPost("/api/books", (CreateBookRequest request) =>
{
    // Minimal APIs automatically validate data annotations on the request model.
    // If validation fails, ASP.NET Core returns a 400 Bad Request automatically.

    // Simulate saving the book and creating a response model
    var newBook = new BookResponse(
        Id: Random.Shared.Next(100, 999),
        Title: request.Title,
        Author: request.Author,
        PublishedYear: request.PublishedYear,
        CreatedAt: DateTime.Now
    );

    // Return a 201 Created status with the structured response model
    return Results.Created($"/api/books/{newBook.Id}", newBook);
})
.WithName("CreateBook");

app.Run();

// ==========================================
// Models (DTOs) and Records
// ==========================================

/// <summary>
/// Request model for creating a book, complete with data validation attributes.
/// </summary>
public record CreateBookRequest(
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 100 characters.")]
    string Title,

    [Required(ErrorMessage = "Author is required.")]
    string Author,

    [Range(1000, 2100, ErrorMessage = "Published year must be a valid year.")]
    int PublishedYear
);

/// <summary>
/// Response model representing the saved book returned to the client.
/// </summary>
public record BookResponse(
    int Id,
    string Title,
    string Author,
    int PublishedYear,
    DateTime CreatedAt
);

/// <summary>
/// Weather forecast model.
/// </summary>
public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}