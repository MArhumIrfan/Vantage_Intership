using System.Collections.Generic;

namespace Lib
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        
        // Navigation property for 1-to-many relationship (One category has many books)
        public List<Book> Books { get; set; } = new();
    }
}