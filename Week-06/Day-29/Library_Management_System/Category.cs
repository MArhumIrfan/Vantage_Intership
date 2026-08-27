using System.Collections.Generic;

namespace Lib
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        
        // Self-referencing relationship for subcategories (hierarchical stacking)
        public int? ParentCategoryId { get; set; }
        public Category? ParentCategory { get; set; }
        public List<Category> SubCategories { get; set; } = new();

        // Navigation property for 1-to-many relationship (One category has many books)
        public List<Book> Books { get; set; } = new();
    }
}