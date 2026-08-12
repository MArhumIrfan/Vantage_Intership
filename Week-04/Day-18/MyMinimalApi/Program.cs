using System.Buffers;
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// In-Memory Database Simulation

var books = new List<BookResponse>
{
    new BookResponse(1, "C# in Depth", "Jon Skeet", 2019, DateTime.Now),
    new BookResponse(2, "Clean Code", "Robert C. Martin", 2008, DateTime.Now)
};

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};


// GET Endpoints

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
}).WithName("GetWeatherForecast");

app.MapGet("/api/welcome", () =>
{
    return Results.Ok(new { Message = "Welcome to the integrated Library Web API!", Timestamp = DateTime.Now });
});

app.MapGet("/api/calc", (string? op, double? num1, double? num2) =>
{
    if (string.IsNullOrEmpty(op) || !num1.HasValue || !num2.HasValue)
    {
        return Results.BadRequest(new { Error = "Missing parameters. Please provide 'op', 'num1', and 'num2'." });
    }

    double result = op.ToLower() switch
    {
        "add" or "+" => num1.Value + num2.Value,
        "subtract" or "-" => num1.Value - num2.Value,
        "multiply" or "*" => num1.Value * num2.Value,
        "divide" or "/" => num2.Value != 0 ? num1.Value / num2.Value : double.NaN,
        _ => double.NaN
    };

    if (double.IsNaN(result))
    {
        return Results.BadRequest(new { Error = "Invalid operation or division by zero." });
    }

    return Results.Ok(new { Operation = op, Num1 = num1.Value, Num2 = num2.Value, Result = result });
});


// BOOKS: GET All & GET by ID

app.MapGet("/api/books", () => Results.Ok(books));

app.MapGet("/api/books/{id:int}", (int id) =>
{
    var book = books.FirstOrDefault(b => b.Id == id);
    return book is not null ? Results.Ok(book) : Results.NotFound(new { Error = $"Book with ID {id} not found." });
});


// BOOKS: POST (Create)

app.MapPost("/api/books", (CreateBookRequest request) =>
{

    var newBook = new BookResponse(
        Id: books.Count > 0 ? books.Max(b => b.Id) + 1 : 1,
        Title: request.Title,
        Author: request.Author,
        PublishedYear: request.PublishedYear,
        CreatedAt: DateTime.Now
    );

    books.Add(newBook);
    
  
    return Results.Created($"/api/books/{newBook.Id}", newBook);
});


// BOOKS: PUT (Update) with Validation & Status Codes

app.MapPut("/api/books/{id:int}", (int id, UpdateBookRequest request) =>
{
    var existingBookIndex = books.FindIndex(b => b.Id == id);

    
    if (existingBookIndex == -1)
    {
        return Results.NotFound(new { Error = $"Book with ID {id} not found for updating." });
    }

  
    var updatedBook = new BookResponse(
        Id: id,
        Title: request.Title,
        Author: request.Author,
        PublishedYear: request.PublishedYear,
        CreatedAt: books[existingBookIndex].CreatedAt // Keep original creation date
    );

    books[existingBookIndex] = updatedBook;

  
    return Results.Ok(updatedBook);
});




app.MapDelete("/api/books/{id:int}", (int id) =>
{
    var book = books.FirstOrDefault(b => b.Id == id);

   
    if (book is null)
    {
        return Results.NotFound(new { Error = $"Book with ID {id} not found for deletion." });
    }

    books.Remove(book);

    return Results.NoContent();
});

app.Run();


// Models and DTOs

public record CreateBookRequest(
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 100 characters.")]
    string Title,

    [Required(ErrorMessage = "Author is required.")]
    string Author,

    [Range(1000, 2100, ErrorMessage = "Published year must be between 1000 and 2100.")]
    int PublishedYear
);

public record UpdateBookRequest(
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(100, MinimumLength = 1)]
    string Title,

    [Required(ErrorMessage = "Author is required.")]
    string Author,

    [Range(1000, 2100)]
    int PublishedYear
);

public record BookResponse(
    int Id,
    string Title,
    string Author,
    int PublishedYear,
    DateTime CreatedAt
);

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}