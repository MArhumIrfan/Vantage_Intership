using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lib
{
    public static class LibraryDatabase
    {
        public static Dictionary<int, Book> Catalog = new Dictionary<int, Book>();

        public static int TotalBooksCount
        {
            get { return Catalog.Count; }
        }

        private const string SaveFilePath = "library.txt";

        public static void Seed()
        {
            // 3 Copies of The Great Gatsby
            Catalog.Add(101, new Book(101, "The Great Gatsby", "Penguin", 1990, "Fantasy", 1000));
            Catalog.Add(102, new Book(102, "The Great Gatsby", "Penguin", 1990, "Fantasy", 1000));
            Catalog.Add(103, new Book(103, "The Great Gatsby", "Penguin", 1990, "Fantasy", 1000));

            // 2 Copies of To Kill a Mockingbird
            Book mockingbird1 = new Book(104, "To Kill a Mockingbird", "Lippincott", 1960, "Classic", 1200);
            mockingbird1.FineDue = 500;
            Catalog.Add(104, mockingbird1);
            Catalog.Add(105, new Book(105, "To Kill a Mockingbird", "Lippincott", 1960, "Classic", 1200));

            // Single Copies
            Catalog.Add(106, new Book(106, "Data Communication and Networks", "Hafiz Mati ur Rahman", 2023, "Education", 2500));

            Book lr0Parser = new Book(107, "Mastering LR(0) Parsers", "Iqra Press", 2024, "Computer Science", 3000);
            lr0Parser.IsBorrowed = true;
            lr0Parser.BorrowedBy = "Tayyab";
            lr0Parser.BorrowedDate = DateTime.Now.AddDays(-10);
            lr0Parser.DueDate = DateTime.Now.AddDays(4);
            Catalog.Add(107, lr0Parser);

            Book pythonGui = new Book(108, "Python GUI with Tkinter", "Vantage Tech", 2025, "Programming", 1800);
            pythonGui.FineDue = 150;
            Catalog.Add(108, pythonGui);

            Catalog.Add(109, new Book(109, "The Art of UI Redesign", "Ghayyur Abbas", 2022, "Design", 2200));
            Catalog.Add(110, new Book(110, "High-Speed Routing for PC Games", "Wazir Muzammil Hussain", 2021, "Technology", 1500));
            Catalog.Add(111, new Book(111, "Fashion Design made easy!", "Syed Tayyab Moassud Alam Shah Buhkari Pher Shahab", 2023, "Design", 3500));
            Catalog.Add(112, new Book(112, "Beaten, But never broken", "Muhammad Whahahj", 2024, "Education", 1500));
            Catalog.Add(113, new Book(113, "Aritifical Intelligence", "Insafullah Khan", 2023, "Computer Science", 1500));
        }

        public static List<Book> SearchBooks(string keyword, string genreFilter, int maxPrice)
        {
            return Catalog.Values
            .Where(b =>
            (string.IsNullOrEmpty(keyword) ||
             b.Name.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 ||
             b.Genre.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0) &&
            (string.IsNullOrEmpty(genreFilter) || b.Genre.Equals(genreFilter, StringComparison.OrdinalIgnoreCase)) &&
            (b.Cost <= maxPrice)
             )
            .ToList();
        }

        public static void LoadFromFile()
        {
            try
            {
                if (!File.Exists(SaveFilePath))
                {
                    throw new FileNotFoundException("SAve file does not exist , starting defualt seed catalog.");
                }
                Catalog.Clear();
                string[] lines = File.ReadAllLines(SaveFilePath);

                foreach (string line in lines)
                {
                    string[] parts = line.Split('|');
                    if (parts.Length != 11)
                    {
                        throw new FormatException("A corrupted or malformed data lines was detected in the save file");
                    }

                    Book b = new Book();
                    b.BookID = int.Parse(parts[0]);
                    b.Name = parts[1];
                    b.Publisher = parts[2];
                    b.DatePublish = int.Parse(parts[3]);
                    b.Genre = parts[4];
                    b.Cost = int.Parse(parts[5]);
                    b.FineDue = int.Parse(parts[6]);
                    b.IsBorrowed = bool.Parse(parts[7]);
                    b.BorrowedBy = parts[8];
                    b.BorrowedDate = DateTime.Parse(parts[9]);
                    b.DueDate = DateTime.Parse(parts[10]);

                    Catalog.Add(b.BookID, b);
                }
            }
            catch (FileNotFoundException ex)
            {
                UI.PrintWarning(ex.Message);
                Seed();
            }
            catch (FormatException ex)
            {
                UI.PrintError("Data corruption Error :" + ex.Message + "Resorting to defualt catalog.");
                Seed();
            }
            catch (Exception ex)
            {
                UI.PrintError("Un expected Error occured while loading files !" + ex.Message);
                Seed();
            }
        }

        public static void SaveToFile()
        {
            try
            {
                List<string> lines = new List<string>();

                foreach (Book b in Catalog.Values)
                {
                    string line = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}",
                    b.BookID, b.Name, b.Publisher, b.DatePublish, b.Genre, b.Cost, b.FineDue,
                    b.IsBorrowed, b.BorrowedBy, b.BorrowedDate.ToString(), b.DueDate.ToString());
                    lines.Add(line);
                }

                File.WriteAllLines(SaveFilePath, lines);
            }
            catch (UnauthorizedAccessException ex)
            {
                UI.PrintError("Permission Error ! Cannot save to file (" + ex.Message + ")");
            }
            catch (IOException ex)
            {
                UI.PrintError("I/O Error: Disk Write Failed (" + ex.Message + ")");
            }
            catch (Exception ex)
            {
                UI.PrintError("Warning ! Could not save to Library Disk(" + ex.Message + ")");
            }
        }

        public static void DisplayAvailableTitles()
        {
            Console.WriteLine("--- Current Library Catalog Inventory ---");
            if (Catalog.Count == 0)
            {
                Console.WriteLine("No Books stored in memory");
                return;
            }

            int totalCount = Catalog.Count;
            int availableCount = Catalog.Values.Count(b => !b.IsBorrowed);
            int borrowedCount = Catalog.Values.Count(b => b.IsBorrowed);

            Console.WriteLine("Summary: [Total: " + totalCount + "] | [Available: " + availableCount + "] | [Checked Out: " + borrowedCount + "]");
            Console.WriteLine("--------------------------------------------------");

            Console.WriteLine("\nFilter catalog by genre?");
            Console.WriteLine("1. Show All Books");
            Console.WriteLine("2. Computer Science / Education");
            Console.WriteLine("3. Fantasy / Classic / Design / Technology");
            int filterChoice = UI.ReadInt32("Select filter option (1-3): ");
            Console.WriteLine();

            foreach (Book b in Catalog.Values)
            {
                if (filterChoice == 2 && b.Genre != "Education" && b.Genre != "Computer Science") continue;
                if (filterChoice == 3 && b.Genre != "Fantasy" && b.Genre != "Classic" && b.Genre != "Design" && b.Genre != "Technology") continue;

                string status = b.IsBorrowed ? "BORROWED (Due: " + b.DueDate.ToString("yyyy-MM-dd") + ")" : "Available";
                Console.WriteLine(string.Format("[ID: {0}] {1} ({2}) - {3}", b.BookID, b.Name, b.Genre, status));
            }
            Console.WriteLine("-----------------------------------------");
        }

        public static void AdvancedSearch()
        {
            Console.WriteLine("1. Search by Title Keyword");
            Console.WriteLine("2. Search by Genre");
            Console.WriteLine("3. Search by Price Range");
            Console.WriteLine("4. Search by Book ID");
            Console.WriteLine("5. Search by Publisher");
            Console.WriteLine("6. Return to Previous Menu\n");

            int choice = UI.ReadInt32("Enter your choice: ");

            List<Book> results = new List<Book>();

            if (choice == 1)
            {
                Console.Write("Enter a keyword to search in the title: ");
                string keyword = Console.ReadLine() ?? "";

                results = Catalog.Values
                    .Where(b => b.Name.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }
            else if (choice == 2)
            {
                Console.Write("Enter genre to search for: ");
                string genre = Console.ReadLine() ?? "";

                results = Catalog.Values
                    .Where(b => b.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            else if (choice == 3)
            {
                int minPrice = UI.ReadInt32("Enter minimum price (PKR): ");
                int maxPrice = UI.ReadInt32("Enter maximum price (PKR): ");

                results = Catalog.Values
                    .Where(b => b.Cost >= minPrice && b.Cost <= maxPrice)
                    .ToList();
            }
            else if (choice == 4)
            {
                int searchId = UI.ReadInt32("Enter the exact Book ID: ");

                if (Catalog.ContainsKey(searchId))
                {
                    results.Add(Catalog[searchId]);
                }
                else
                {
                    UI.PrintError("No book found matching that ID.");
                }
            }
            else if (choice == 5)
            {
                Console.Write("Enter publisher name keyword: ");
                string pubKeyword = Console.ReadLine() ?? "";

                results = Catalog.Values
                    .Where(b => b.Publisher.IndexOf(pubKeyword, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }
            else
            {
                return;
            }

            // Optional price sorting
            if (results.Count > 0 && choice != 4)
            {
                Console.WriteLine("\nWould you like to sort these results by price?");
                Console.WriteLine("1. Low to High (Budget Friendly)");
                Console.WriteLine("2. High to Low (Premium)");
                Console.WriteLine("3. Don't sort (Default order)");
                int sortChoice = UI.ReadInt32("Enter sort choice (1-3): ");

                if (sortChoice == 1)
                {
                    results = results.OrderBy(b => b.Cost).ToList();
                }
                else if (sortChoice == 2)
                {
                    results = results.OrderByDescending(b => b.Cost).ToList();
                }
            }

            Console.WriteLine("\n--- Search Results ---");

            if (results.Count == 0 && choice != 4)
            {
                Console.WriteLine("No books found matching your criteria.");
            }
            else
            {
                foreach (Book b in results)
                {
                    string status = b.IsBorrowed ? "BORROWED" : "Available";
                    Console.WriteLine(string.Format("[ID: {0}] {1} | Publisher: {2} ({3}) - {4} PKR - {5}",
                        b.BookID, b.Name, b.Publisher, b.Genre, b.Cost, status));
                }
            }
        }

        // --- Audit Trail Logger ---
        public static void LogTransaction(string action, int bookId, string bookTitle, string memberName)
        {
            try
            {
                string filePath = "history.txt";
                string logEntry = string.Format("[{0}] {1} - '{2}' (ID: {3}) by {4}",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), action, bookTitle, bookId, memberName);

                File.AppendAllText(filePath, logEntry + Environment.NewLine);
            }
            catch (Exception ex)
            {
                UI.PrintError("Warning: Could not write to transaction log (" + ex.Message + ")");
            }
        }
    }
}
