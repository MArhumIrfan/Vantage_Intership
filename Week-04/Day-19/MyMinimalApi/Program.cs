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

// ==================== BOOK CRUD ENDPOINTS ====================

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

// 3. GET: Advanced search (mirrors LibraryDatabase.SearchBooks used by the console app)
app.MapGet("/api/books/search", (string? keyword, string? genre, int? maxPrice) =>
{
    var results = LibraryDatabase.SearchBooks(keyword ?? "", genre ?? "", maxPrice ?? int.MaxValue);
    return Results.Ok(results);
});

// 4. POST: Add a new book
app.MapPost("/api/books", (Book newBook) =>
{
    int newId = LibraryDatabase.Catalog.Keys.Any() ? LibraryDatabase.Catalog.Keys.Max() + 1 : 101;
    newBook.BookID = newId;

    LibraryDatabase.Catalog.Add(newId, newBook);
    LibraryDatabase.SaveToFile();

    return Results.Created($"/api/books/{newId}", newBook);
});

// 5. PUT: Update an existing book
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

// 6. DELETE: Remove a book
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

// ==================== BORROW / RETURN / FINES ====================
// These mirror VerifyUser.BorrowBook / ReturnBook / PayFineBook so the
// business rules (3-book limit, late fee tiers, etc.) stay identical
// between the console app and the API.

// 7. POST: Borrow a book
app.MapPost("/api/books/{id}/borrow", (int id, BorrowRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.Username))
    {
        return Results.BadRequest(new { Error = "Username is required." });
    }

    if (!LibraryDatabase.Catalog.ContainsKey(id))
    {
        return Results.NotFound(new { Error = $"Book ID {id} not found." });
    }

    int activeCheckouts = LibraryDatabase.Catalog.Values
        .Count(b => b.IsBorrowed && b.BorrowedBy == request.Username);

    if (activeCheckouts >= 3)
    {
        return Results.BadRequest(new { Error = $"Limit reached: {request.Username} already has {activeCheckouts} books checked out." });
    }

    var book = LibraryDatabase.Catalog[id];

    if (book.IsBorrowed)
    {
        string who = book.BorrowedBy == request.Username ? "you already" : "someone else";
        return Results.Conflict(new { Error = $"'{book.Name}' is currently borrowed by {who}." });
    }

    book.IsBorrowed = true;
    book.BorrowedBy = request.Username;
    book.BorrowedDate = DateTime.Now;
    book.DueDate = DateTime.Now.AddDays(14);

    LibraryDatabase.SaveToFile();
    LibraryDatabase.LogTransaction("BORROW", book.BookID, book.Name, request.Username);

    return Results.Ok(book);
});

// 8. POST: Return a book (applies the same late-fee tiers as the console app)
app.MapPost("/api/books/{id}/return", (int id, BorrowRequest request) =>
{
    if (!LibraryDatabase.Catalog.ContainsKey(id))
    {
        return Results.NotFound(new { Error = $"Book ID {id} not found." });
    }

    var book = LibraryDatabase.Catalog[id];

    if (!book.IsBorrowed)
    {
        return Results.BadRequest(new { Error = "This book is not currently marked as borrowed." });
    }

    string message = $"Thank you for returning: {book.Name}";
    int fineAmount = 0;

    if (DateTime.Now > book.DueDate)
    {
        int daysLate = (DateTime.Now - book.DueDate).Days;
        if (daysLate <= 0) daysLate = 1;

        int fineMultiplier = daysLate switch
        {
            >= 1 and <= 5 => 50,
            > 5 and <= 15 => 75,
            _ => 100
        };

        fineAmount = daysLate * fineMultiplier;
        book.FineDue += fineAmount;
        message = $"Book was {daysLate} day(s) late. Fine of {fineAmount} PKR added.";
    }

    string returnedBy = string.IsNullOrWhiteSpace(request.Username) ? book.BorrowedBy : request.Username;

    book.IsBorrowed = false;
    book.BorrowedBy = "";
    book.BorrowedDate = DateTime.MinValue;
    book.DueDate = DateTime.MinValue;

    LibraryDatabase.SaveToFile();
    LibraryDatabase.LogTransaction("RETURN", book.BookID, book.Name, returnedBy);

    return Results.Ok(new { Message = message, FineAdded = fineAmount, Book = book });
});

// 9. POST: Pay an outstanding fine on a book
app.MapPost("/api/books/{id}/pay-fine", (int id, PayFineRequest request) =>
{
    if (!LibraryDatabase.Catalog.ContainsKey(id))
    {
        return Results.NotFound(new { Error = $"Book ID {id} not found." });
    }

    var book = LibraryDatabase.Catalog[id];

    if (book.FineDue <= 0)
    {
        return Results.Ok(new { Message = "No outstanding fine for this book.", Book = book });
    }

    if (request.Amount <= 0)
    {
        return Results.BadRequest(new { Error = "Payment amount must be greater than zero." });
    }

    if (request.Amount > book.FineDue)
    {
        int change = request.Amount - book.FineDue;
        book.FineDue = 0;
        LibraryDatabase.SaveToFile();
        return Results.Ok(new { Message = "Transaction completed.", Change = change, Book = book });
    }

    book.FineDue -= request.Amount;
    LibraryDatabase.SaveToFile();

    return Results.Ok(new { Message = "Payment accepted.", RemainingBalance = book.FineDue, Book = book });
});

// ==================== MEMBERS ====================
// Reuses UI.RegisteredUsers so members registered via the console app's
// Admin Dashboard show up here too, and vice versa (within the same run).

// 10. GET: List all registered members
app.MapGet("/api/members", () => Results.Ok(UI.RegisteredUsers));

// 11. POST: Register a new member
app.MapPost("/api/members", (RegisterMemberRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.Name))
    {
        return Results.BadRequest(new { Error = "Member name is required." });
    }

    if (UI.RegisteredUsers.ContainsKey(request.MemberId))
    {
        return Results.Conflict(new { Error = "That Member ID is already registered." });
    }

    UI.RegisteredUsers.Add(request.MemberId, request.Name);
    return Results.Created($"/api/members/{request.MemberId}", new { request.MemberId, request.Name });
});

// 12. GET: Report for a single member (active checkouts + outstanding fines)
app.MapGet("/api/members/{id}", (int id) =>
{
    if (!UI.RegisteredUsers.TryGetValue(id, out var name))
    {
        return Results.NotFound(new { Error = $"Member ID {id} not found." });
    }

    var borrowedBooks = LibraryDatabase.Catalog.Values
        .Where(b => b.IsBorrowed && b.BorrowedBy == name)
        .ToList();

    int totalFines = borrowedBooks.Sum(b => b.FineDue);

    return Results.Ok(new
    {
        MemberId = id,
        Name = name,
        ActiveCheckouts = borrowedBooks.Count,
        TotalFines = totalFines,
        Books = borrowedBooks
    });
});

// ==================== HISTORY & ANALYTICS ====================

// 13. GET: Transaction history log (same file the console app writes to)
app.MapGet("/api/history", () =>
{
    const string path = "history.txt";
    if (!File.Exists(path))
    {
        return Results.Ok(Array.Empty<string>());
    }
    return Results.Ok(File.ReadAllLines(path));
});

// 14. GET: Library-wide analytics (mirrors VerifyAdmin.AdvancedAnalytics)
app.MapGet("/api/analytics", () =>
{
    if (LibraryDatabase.Catalog.Count == 0)
    {
        return Results.Ok(new { Message = "No books available for analysis." });
    }

    var books = LibraryDatabase.Catalog.Values;

    int totalValue = books.Sum(b => b.Cost);
    double avgCost = books.Average(b => b.Cost);
    var mostExpensive = books.OrderByDescending(b => b.Cost).FirstOrDefault();

    var genreDistribution = books
        .GroupBy(b => b.Genre)
        .Select(g => new { Genre = g.Key, Count = g.Count() });

    var overdueBooks = books
        .Where(b => b.IsBorrowed && DateTime.Now > b.DueDate)
        .Select(b => new { b.BookID, b.Name, b.BorrowedBy, b.DueDate })
        .ToList();

    var topBorrowers = books
        .Where(b => b.IsBorrowed)
        .GroupBy(b => b.BorrowedBy)
        .Select(g => new { Username = g.Key, ActiveCount = g.Count() })
        .OrderByDescending(g => g.ActiveCount)
        .ToList();

    return Results.Ok(new
    {
        TotalValue = totalValue,
        AverageCost = Math.Round(avgCost, 2),
        MostExpensiveBook = mostExpensive,
        GenreDistribution = genreDistribution,
        OverdueBooks = overdueBooks,
        TopBorrowers = topBorrowers
    });
});

app.Run();

// ==================== REQUEST DTOs ====================
// Small records for endpoints that need input other than a full Book.

public record BorrowRequest(string Username);
public record PayFineRequest(int Amount);
public record RegisterMemberRequest(int MemberId, string Name);
