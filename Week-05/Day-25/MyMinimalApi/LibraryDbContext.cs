using Microsoft.EntityFrameworkCore;

namespace Lib
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

        // This tells EF Core to create a "Books" table in your SQL Server database
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<User> Users => Set<User>();
    }
}