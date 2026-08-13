using Lib;

var builder = WebApplication.CreateBuilder(args);

// Add Swagger services for testing
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Opens Swagger UI at /swagger
}

// --- Load the catalog once at startup so the API isn't empty ---
LibraryDatabase.LoadFromFile();

// --- CRUD ENDPOINTS LINKED TO YOUR LIBRARY DATABASE ---

// 1. GET: Retrieve all books
app.MapGet("/api/books", () =>
{
    var books = LibraryDatabase.Catalog.Values.ToList();
    return Results.Ok(books);
});

// 2. GET by ID: Retrieve a specific book
app.MapGet("/api/books/{id}", (int id) =>
{
    if (!LibraryDatabase.Catalog.ContainsKey(id))
    {
        return Results.NotFound(new { Error = $"Book ID {id} not found." });
    }
    return Results.Ok(LibraryDatabase.Catalog[id]);
});

// 3. POST: Add a new book
app.MapPost("/api/books", (Book newBook) =>
{
    int newId = LibraryDatabase.Catalog.Keys.Any() ? LibraryDatabase.Catalog.Keys.Max() + 1 : 101;
    newBook.BookID = newId;

    LibraryDatabase.Catalog.Add(newId, newBook);
    LibraryDatabase.SaveToFile();

    return Results.Created($"/api/books/{newId}", newBook);
});

// 4. PUT: Update an existing book
app.MapPut("/api/books/{id}", (int id, Book updatedBook) =>
{
    if (!LibraryDatabase.Catalog.ContainsKey(id))
    {
        return Results.NotFound(new { Error = $"Book ID {id} not found." });
    }

    var book = LibraryDatabase.Catalog[id];
    book.Name = updatedBook.Name;
    book.Publisher = updatedBook.Publisher;
    book.DatePublish = updatedBook.DatePublish;
    book.Genre = updatedBook.Genre;
    book.Cost = updatedBook.Cost;

    LibraryDatabase.SaveToFile();

    return Results.Ok(book);
});

// 5. DELETE: Remove a book
app.MapDelete("/api/books/{id}", (int id) =>
{
    if (!LibraryDatabase.Catalog.ContainsKey(id))
    {
        return Results.NotFound(new { Error = $"Book ID {id} not found." });
    }

    LibraryDatabase.Catalog.Remove(id);
    LibraryDatabase.SaveToFile();

    return Results.NoContent();
});

app.Run();
