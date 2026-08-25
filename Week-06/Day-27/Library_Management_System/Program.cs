using Lib;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.NativeInterop;
using Microsoft.VisualBasic;
var builder = WebApplication.CreateBuilder(args);

// Add Swagger services for testing
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=LibraryDb;Trusted_Connection=True;MultipleActiveResultSets=true"));
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Opens Swagger UI at /swagger
}

/*
// --- INITIALIZE PRE-REGISTERED MEMBERS & LOAD CATALOG ---
if (!UI.RegisteredUsers.ContainsKey(39393))
{
    UI.RegisteredUsers.Add(39393, "Muhammad Arhum Irfan");
    UI.RegisteredUsers.Add(39425, "Ghayyur Abbas");
    UI.RegisteredUsers.Add(40142, "Wazir Muzzamil Hussain");
    UI.RegisteredUsers.Add(39358, "Muhammad Whahaj");
    UI.RegisteredUsers.Add(39859, "Insfalullah Khan");
}
*/
LibraryDatabase.LoadFromFile();

// ==================== AUTHENTICATION ENDPOINT ====================


// ==================== AUTHENTICATION ENDPOINT ====================

app.MapPost("/api/auth/login", async (ApiLoginRequest request, LibraryDbContext db) =>
{
    string roleLower = request.Role?.Trim().ToLower() ?? "";

    if (roleLower == "admin")
    {
        if (request.Username == "Admin" && request.Password == "admin123")
        {
            return Results.Ok(new { Role = "Admin", Message = "Admin login successful!" });
        }
        return Results.BadRequest(new { Error = "Invalid Admin credentials. Username: Admin, Password: admin123" });
    }
    else if (roleLower == "user")
    {
        if (request.MemberId.HasValue)
        {
            // Query the database instead of the old UI dictionary
            var user = await db.Users.FindAsync(request.MemberId.Value);
            
            if (user != null)
            {
                if (request.Password == "User123")
                {
                    return Results.Ok(new { Role = "User", MemberId = request.MemberId, Name = user.Name, Message = $"Welcome, {user.Name}!" });
                }
                return Results.BadRequest(new { Error = "Invalid password. Default user password is User123" });
            }
        }
        return Results.NotFound(new { Error = "Member ID not recognized in the system." });
    }
    else if (roleLower == "guest")
    {
        if (request.Age.HasValue && request.Age.Value >= 18)
        {
            return Results.Ok(new { Role = "Guest", Message = "Guest access granted (Age 18+)." });
        }
        return Results.BadRequest(new { Error = "Guests must be 18 and above to proceed." });
    }

    return Results.BadRequest(new { Error = "Invalid role specified. Use Admin, User, or Guest." });
});
// ==================== BOOK CRUD ENDPOINTS ====================

app.MapGet("/api/books", () =>
{
    var books = LibraryDatabase.Catalog.Values.ToList();
    return Results.Ok(books);
});

app.MapGet("/api/books/{id}", (int id) =>
{
    if (!LibraryDatabase.Catalog.ContainsKey(id))
    {
        return Results.NotFound(new { Error = $"Book ID {id} not found." });
    }
    return Results.Ok(LibraryDatabase.Catalog[id]);
});

app.MapGet("/api/books/search", (string? keyword, string? genre, int? maxPrice) =>
{
    var results = LibraryDatabase.SearchBooks(keyword ?? "", genre ?? "", maxPrice ?? int.MaxValue);
    return Results.Ok(results);
});

app.MapPost("/api/books", (Book newBook) =>
{
    int newId = LibraryDatabase.Catalog.Keys.Any() ? LibraryDatabase.Catalog.Keys.Max() + 1 : 101;
    newBook.BookID = newId;

    LibraryDatabase.Catalog.Add(newId, newBook);
    LibraryDatabase.SaveToFile();

    return Results.Created($"/api/books/{newId}", newBook);
});

app.MapPut("/api/books/{id}", (int id, Book updatedBook) =>
{
    if (!LibraryDatabase.Catalog.ContainsKey(id))
    {
        return Results.NotFound(new { Error = $"Book ID {id} not found." });
    }

    var book = LibraryDatabase.Catalog[id];
    book.Name = updatedBook.Name ?? "";
    book.Publisher = updatedBook.Publisher ?? "";
    book.DatePublish = updatedBook.DatePublish;
    book.Genre = updatedBook.Genre ?? "";
    book.Cost = updatedBook.Cost;

    LibraryDatabase.SaveToFile();

    return Results.Ok(book);
});

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


app.MapPost("/api/books/{id}/return", (int id, ReturnRequest request) =>
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

app.MapGet("/api/members", async (LibraryDbContext db) =>
{
    // Fetches all users straight from the SQL Server Users table
    var members = await db.Users.ToListAsync(); 
    
    if (members.Any())
    {
        return Results.Ok(members);
    }
    
    return Results.Ok(new { Message = "No members found in the database." });
});

app.MapGet("/api/members/{id}", async (int id, LibraryDbContext db) =>
{
    // Searches the SQL database for a user matching the provided ID
    var member = await db.Users.FindAsync(id);
    
    if (member != null)
    {
        return Results.Ok(member);
    }
    
    return Results.NotFound(new { Error = $"Member with ID {id} not found." });
});

// ==================== HISTORY & ANALYTICS ====================

app.MapGet("/api/history", () =>
{
    const string path = "history.txt";
    if (!File.Exists(path))
    {
        return Results.Ok(Array.Empty<string>());
    }
    return Results.Ok(File.ReadAllLines(path));
});

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

app.MapPost("/api/books/borrow", async (BorrowRequest request, LibraryDbContext db) =>
{
    // 1. Enforce the 3-book limit rule
    int borrowedCount = await db.Books.CountAsync(b => b.IsBorrowed && b.BorrowedBy == request.MemberName);
    if (borrowedCount >= 3)
    {
        return Results.BadRequest(new { Error = $"Limit Reached: {request.MemberName} already has 3 books checked out." });
    }

    // 2. Locate the requested book in SQL Server
    var book = await db.Books.FindAsync(request.BookId);
    if (book == null) 
    {
        return Results.NotFound(new { Error = $"Book ID {request.BookId} not found." });
    }

    // 3. Ensure it isn't already checked out
    if (book.IsBorrowed) 
    {
        return Results.BadRequest(new { Error = $"Book '{book.Name}' is currently unavailable." });
    }

    // 4. Apply the borrowing business logic
    book.IsBorrowed = true;
    book.BorrowedBy = request.MemberName;
    book.BorrowedDate = DateTime.Now;
    book.DueDate = DateTime.Now.AddDays(14); // 14-day checkout rule

    await db.SaveChangesAsync();

    return Results.Ok(new 
    { 
        Message = $"Successfully borrowed '{book.Name}'.", 
        DueDate = book.DueDate.ToString("yyyy-MM-dd") 
    });
});
app.Run();

// ==================== REQUEST DTOs ====================

public record ApiLoginRequest(string Role, string? Username, string? Password, int? MemberId, int? Age);
public record ReturnRequest(string? Username);
public record PayFineRequest(int Amount);
public record RegisterMemberRequest(int MemberId, string Name);
public record BorrowRequest(int BookId, string MemberName);