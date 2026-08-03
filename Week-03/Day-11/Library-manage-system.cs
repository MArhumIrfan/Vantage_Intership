/*
--UserNames & Password for all types of users--
--
--Admin: UserName: "Admin", Password: "admin123".--
--
--User: Member ID required (e.g., 39393), Password: "User123".--
--
--Guest: No UserName or Password Required, just have to be 18 and above.--
--
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Lib
{
    public enum SystemRole
    {
        Admin,
        User,
        Guest,
        Unknown
    }

    public class Book
    {
        public int BookID { get; set; }
        public string Name { get; set; }
        public string Publisher { get; set; }
        public int DatePublish { get; set; }
        public string Genre { get; set; }
        public int Cost { get; set; }
        public int FineDue { get; set; }

        public bool IsBorrowed { get; set; }
        public string BorrowedBy { get; set; }

        public DateTime BorrowedDate { get; set; }
        public DateTime DueDate { get; set; }

        public Book()
        {
            IsBorrowed = false;
            BorrowedBy = "";
            BorrowedDate = DateTime.MinValue;
            DueDate = DateTime.MinValue;
        }

        public Book(int id, string name, string publisher, int datePublish, string genre, int cost)
        {
            BookID = id;
            Name = name;
            Publisher = publisher;
            DatePublish = datePublish;
            Genre = genre;
            Cost = cost;
            IsBorrowed = false;
            BorrowedBy = "";
            BorrowedDate = DateTime.MinValue;
            DueDate = DateTime.MinValue;
        }
    }

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
        }

        public static void LoadFromFile()
        {
            try
            {
                if (File.Exists(SaveFilePath))
                {
                    Catalog.Clear();
                    string[] lines = File.ReadAllLines(SaveFilePath);
                    
                    foreach (string line in lines)
                    {
                        string[] parts = line.Split('|');
                        
                        if (parts.Length == 11)
                        {
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
                }
                else
                {
                    Seed();
                }
            }
            catch (Exception)
            {
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
            catch (Exception ex)
            {
                UI.PrintError("Warning: could not save library data (" + ex.Message + ")");
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

            foreach (Book b in Catalog.Values)
            {
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
            Console.WriteLine("5. Return to Previous Menu\n");
            
            int choice = UI.ReadInt32("Enter your choice: ");
            
            List<Book> results = new List<Book>();

            if (choice == 1)
            {
                Console.Write("Enter a keyword to search in the title: ");
                string keyword = Console.ReadLine() ?? "";
                
                foreach (Book book in Catalog.Values)
                {
                    if (book.Name.ToLower().Contains(keyword.ToLower()))
                    {
                        results.Add(book);
                    }
                }
            }
            else if (choice == 2)
            {
                Console.Write("Enter genre to search for: ");
                string genre = Console.ReadLine() ?? "";
                
                foreach (Book book in Catalog.Values)
                {
                    if (book.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase))
                    {
                        results.Add(book);
                    }
                }
            }
            else if (choice == 3)
            {
                int minPrice = UI.ReadInt32("Enter minimum price (PKR): ");
                int maxPrice = UI.ReadInt32("Enter maximum price (PKR): ");
                
                foreach (Book book in Catalog.Values)
                {
                    if (book.Cost >= minPrice && book.Cost <= maxPrice)
                    {
                        results.Add(book);
                    }
                }
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
            else
            {
                return; 
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
                    Console.WriteLine(string.Format("[ID: {0}] {1} ({2}) - {3} PKR - {4}", b.BookID, b.Name, b.Genre, b.Cost, status));
                }
            }
        }

        // --- NEW: Audit Trail Logger ---
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

    public interface IBorrower
    {
        void BorrowBook();
        void ReturnBook();
        void PayFineBook();
    }

    public abstract class Login
    {
        public abstract void ExecuteRoleActions();
    }

    public class VerifyAdmin : Login
    {
        private String adminUserName = "Admin";
        public string AdminUserName
        {
            get { return adminUserName; }
            set
            {
                if (value != "Admin") { UI.PrintError("Incorrect Username entered!"); Environment.Exit(0); }
                else { UI.PrintSuccess("Correct Admin Username input"); adminUserName = value; }
            }
        }

        private String adminPassword = "admin123";
        public string AdminPassword
        {
            get { return adminPassword; }
            set
            {
                if (value != "admin123") { UI.PrintError("Incorrect password Entered"); Environment.Exit(0); }
                else { UI.PrintSuccess("Correct Admin Password Input"); adminPassword = value; }
            }
        }

        public override void ExecuteRoleActions()
        {
            while (true)
            {
                UI.ClearAndHeader("Admin Dashboard");
                Console.WriteLine("1. Add a new book");
                Console.WriteLine("2. View current Inventory (Basic)");
                Console.WriteLine("3. Full Detailed Book Report");
                Console.WriteLine("4. User Details Report");
                Console.WriteLine("5. Register a New Member");
                Console.WriteLine("6. View Transaction History"); // NEW
                Console.WriteLine("7. Logout / Main menu\n");     // Shifted to 7

                int selection = UI.ReadInt32("Please choose an option: ");

                if (selection == 1)
                {
                    UI.ClearAndHeader("Admin Book Entry Panel");

                    Console.Write("Enter the book name: ");
                    string bName = Console.ReadLine() ?? "";

                    Console.Write("Enter your book publisher: ");
                    string bPub = Console.ReadLine() ?? "";

                    int bYear = UI.ReadInt32("Enter the Book Year: ");

                    Console.Write("Enter the book genre: ");
                    string bGenre = Console.ReadLine() ?? "";

                    int bCost = UI.ReadInt32("Enter the book price: ");

                    int newId = 101;
                    if (LibraryDatabase.Catalog.Count > 0)
                    {
                        newId = LibraryDatabase.Catalog.Keys.Max() + 1;
                    }

                    Book book = new Book(newId, bName, bPub, bYear, bGenre, bCost);
                    LibraryDatabase.Catalog.Add(newId, book);
                    LibraryDatabase.SaveToFile();

                    UI.PrintSuccess("Successfully added " + book.Name + " with ID [" + newId + "]!");
                    Console.WriteLine("Total books in live runtime memory: " + LibraryDatabase.TotalBooksCount);
                    UI.Pause();
                }
                else if (selection == 2)
                {
                    UI.ClearAndHeader("Live Inventory Tracking");
                    Console.WriteLine("Total Books Stored: " + LibraryDatabase.TotalBooksCount + "\n");
                    LibraryDatabase.DisplayAvailableTitles();
                    UI.Pause();
                }
                else if (selection == 3)
                {
                    UI.ClearAndHeader("Full Detailed Book Report");
                    
                    if (LibraryDatabase.Catalog.Count == 0)
                    {
                        Console.WriteLine("No Books stored in memory.");
                    }
                    else
                    {
                        foreach (Book b in LibraryDatabase.Catalog.Values)
                        {
                            Console.WriteLine(string.Format("[ID: {0}] {1}", b.BookID, b.Name.ToUpper()));
                            Console.WriteLine(string.Format("   Publisher: {0} | Year: {1} | Genre: {2}", b.Publisher, b.DatePublish, b.Genre));
                            Console.WriteLine(string.Format("   Value: {0} PKR", b.Cost));
                            
                            if (b.IsBorrowed)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine(string.Format("   Status: BORROWED by {0}", b.BorrowedBy));
                                Console.WriteLine(string.Format("   Borrowed On: {0} | Due Date: {1}", b.BorrowedDate.ToString("yyyy-MM-dd"), b.DueDate.ToString("yyyy-MM-dd")));
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("   Status: AVAILABLE");
                                Console.ResetColor();
                            }

                            if (b.FineDue > 0)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(string.Format("   Outstanding Fine: {0} PKR", b.FineDue));
                                Console.ResetColor();
                            }
                            
                            Console.WriteLine("--------------------------------------------------");
                        }
                    }
                    UI.Pause();
                }
                else if (selection == 4) 
                {
                    UI.ClearAndHeader("User Details Report");
                    
                    foreach (var userKvp in UI.RegisteredUsers)
                    {
                        int memberId = userKvp.Key;
                        string memberName = userKvp.Value;
                        
                        int activeCheckouts = 0;
                        int totalFines = 0;

                        foreach (Book b in LibraryDatabase.Catalog.Values)
                        {
                            if (b.IsBorrowed && b.BorrowedBy == memberName)
                            {
                                activeCheckouts++;
                                totalFines += b.FineDue;
                            }
                        }

                        Console.WriteLine(string.Format("[Member ID: {0}] {1}", memberId, memberName.ToUpper()));
                        Console.WriteLine(string.Format("   Active Checkouts: {0} / 3", activeCheckouts));
                        
                        if (totalFines > 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(string.Format("   Total Accumulated Fines: {0} PKR", totalFines));
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.WriteLine("   Total Accumulated Fines: 0 PKR");
                        }
                        Console.WriteLine("--------------------------------------------------");
                    }
                    UI.Pause();
                }
                else if (selection == 5)
                {
                    UI.ClearAndHeader("Register New Member");
                    
                    int newMemberId = UI.ReadInt32("Enter new 5-digit Member ID: ");
                    
                    if (UI.RegisteredUsers.ContainsKey(newMemberId))
                    {
                        UI.PrintError("That ID is already registered in the system.");
                    }
                    else
                    {
                        Console.Write("Enter the member's full name: ");
                        string newName = Console.ReadLine() ?? "";
                        
                        UI.RegisteredUsers.Add(newMemberId, newName);
                        UI.PrintSuccess("User " + newName + " successfully registered with ID [" + newMemberId + "]!");
                    }
                    UI.Pause();
                }
                else if (selection == 6) // --- NEW: View Transaction History Logic ---
                {
                    UI.ClearAndHeader("Permanent Transaction History");
                    
                    if (File.Exists("history.txt"))
                    {
                        string[] historyLog = File.ReadAllLines("history.txt");
                        foreach (string entry in historyLog)
                        {
                            Console.WriteLine(entry);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No transactions have been recorded yet.");
                    }
                    
                    UI.Pause();
                }
                else if (selection == 7)
                {
                    UI.PrintWarning("Logging out of the System.......");
                    System.Threading.Thread.Sleep(1000);
                    break;
                }
                else
                {
                    UI.PrintError("Incorrect input! Try again.");
                    UI.Pause();
                }
            }
        }
    }

    public class VerifyUser : Login, IBorrower
    {
        public string Username { get; set; } 

        private string userPassword = "User123";
        public string UserPassword
        {
            get { return userPassword; }
            set
            {
                if (value != "User123") { UI.PrintError("Incorrect password !"); Environment.Exit(0); }
                else { UI.PrintSuccess("Correct Password Input"); userPassword = value; }
            }
        }

        public override void ExecuteRoleActions()
        {
            while (true)
            {
                UI.ClearAndHeader(Username + "'s Dashboard");
                Console.WriteLine("1. To Borrow a Book");
                Console.WriteLine("2. Return a Book");
                Console.WriteLine("3. Pay Outstanding Fine");
                Console.WriteLine("4. Advanced Search & Filtering");
                Console.WriteLine("5. Log out / Main menu\n");
                
                int selection = UI.ReadInt32("Enter your choice: ");

                if (selection == 1)
                {
                    UI.ClearAndHeader("Borrow a Book");
                    BorrowBook();
                    UI.Pause();
                }
                else if (selection == 2)
                {
                    UI.ClearAndHeader("Return a Book");
                    ReturnBook();
                    UI.Pause();
                }
                else if (selection == 3)
                {
                    UI.ClearAndHeader("Fine Payment");
                    PayFineBook();
                    UI.Pause();
                }
                else if (selection == 4)
                {
                    UI.ClearAndHeader("Advanced Search");
                    LibraryDatabase.AdvancedSearch(); 
                    UI.Pause();
                }
                else if (selection == 5)
                {
                    UI.PrintWarning("Logging out of the System.......");
                    Thread.Sleep(1000);
                    break;
                }
                else
                {
                    UI.PrintError("Invalid Input! Please choose 1-5.");
                    UI.Pause();
                }
            }
        }

        public void BorrowBook()
        {
            int borrowedCount = 0;
            foreach (Book book in LibraryDatabase.Catalog.Values)
            {
                if (book.IsBorrowed && book.BorrowedBy == Username)
                {
                    borrowedCount++;
                }
            }

            if (borrowedCount >= 3)
            {
                UI.PrintError("Limit Reached: You already have " + borrowedCount + " books checked out.");
                Console.WriteLine("Please return a book before borrowing a new one.");
                return; 
            }

            LibraryDatabase.DisplayAvailableTitles();

            int targetID = UI.ReadInt32("\nEnter the ID of the book to be borrowed: ");

            if (!LibraryDatabase.Catalog.ContainsKey(targetID))
            {
                UI.PrintError("Book ID [" + targetID + "] not found in the catalog.");
                return;
            }

            Book foundBook = LibraryDatabase.Catalog[targetID];

            if (foundBook.IsBorrowed)
            {
                UI.PrintError(foundBook.Name + " is currently borrowed by " +
                    (foundBook.BorrowedBy == Username ? "you already" : "someone else") + "!");
                return;
            }

            foundBook.IsBorrowed = true;
            foundBook.BorrowedBy = Username;
            foundBook.BorrowedDate = DateTime.Now; 
            foundBook.DueDate = DateTime.Now.AddDays(14); 

            LibraryDatabase.SaveToFile();
            
            // --- NEW: Log the borrow transaction ---
            LibraryDatabase.LogTransaction("BORROW", foundBook.BookID, foundBook.Name, Username);

            UI.PrintSuccess(foundBook.Name + " borrowed successfully! Price = " + foundBook.Cost + " PKR");
            UI.PrintWarning("IMPORTANT: Your due date is " + foundBook.DueDate.ToString("yyyy-MM-dd") + ".");
        }

        public void ReturnBook()
        {
            LibraryDatabase.DisplayAvailableTitles();

            int targetID = UI.ReadInt32("\nEnter the Book ID to return: ");

            if (!LibraryDatabase.Catalog.ContainsKey(targetID))
            {
                UI.PrintError("Book ID [" + targetID + "] not found in the catalog.");
                return;
            }

            Book foundBook = LibraryDatabase.Catalog[targetID];

            if (!foundBook.IsBorrowed)
            {
                UI.PrintError("This book wasn't marked as borrowed, so there's nothing to return.");
                return;
            }

            if (DateTime.Now > foundBook.DueDate)
            {
                int daysLate = (DateTime.Now - foundBook.DueDate).Days; 
                if (daysLate <= 0) daysLate = 1; 
                
                int fineAmount = daysLate * 50; 
                foundBook.FineDue += fineAmount;

                UI.PrintWarning("WARNING: This book is " + daysLate + " days late.");
                Console.WriteLine("An automatic late fee of " + fineAmount + " PKR has been added to the account.");
            }

            foundBook.IsBorrowed = false;
            foundBook.BorrowedBy = "";
            foundBook.BorrowedDate = DateTime.MinValue; 
            foundBook.DueDate = DateTime.MinValue;      
            LibraryDatabase.SaveToFile();

            // --- NEW: Log the return transaction ---
            LibraryDatabase.LogTransaction("RETURN", foundBook.BookID, foundBook.Name, Username);

            UI.PrintSuccess("Thank you for returning the book: " + foundBook.Name);
        }

        public void PayFineBook()
        {
            LibraryDatabase.DisplayAvailableTitles();

            int targetID = UI.ReadInt32("\nEnter the Book ID to settle fines: ");
            
            if (LibraryDatabase.Catalog.ContainsKey(targetID))
            {
                Book foundBook = LibraryDatabase.Catalog[targetID];

                if (foundBook.FineDue <= 0)
                {
                    UI.PrintSuccess("Good News! There is no outstanding fine for this book.");
                }
                else
                {
                    Console.WriteLine("\nOutstanding late fees for " + foundBook.Name + " is: " + foundBook.FineDue + " PKR ");
                    int payment = UI.ReadInt32("Enter the Amount to pay for the fine: ");

                    if (payment <= 0)
                    {
                        UI.PrintError("Invalid amount of payment value given. Transaction cancelled.");
                    }
                    else if (payment > foundBook.FineDue)
                    {
                        int change = payment - foundBook.FineDue;
                        UI.PrintSuccess("Transaction Completed! Your change is: " + change + " PKR ");
                        foundBook.FineDue = 0;
                        LibraryDatabase.SaveToFile();
                    }
                    else
                    {
                        foundBook.FineDue -= payment;
                        LibraryDatabase.SaveToFile();
                        UI.PrintSuccess("Payment accepted! Your remaining balance is: " + foundBook.FineDue + " PKR ");
                    }
                }
            }
            else
            {
                UI.PrintError("Book ID [" + targetID + "] not found.");
            }
        }
    }

    public class GuestUser : Login
    {
        private int age;
        public int Age
        {
            get { return age; }
            set
            {
                if (value < 18) { UI.PrintError("You are Under 18!"); Environment.Exit(0); }
                else if (value > 110) { UI.PrintError("Too Old!"); Environment.Exit(0); }
                else { UI.PrintSuccess("Access Allowed"); age = value; }
            }
        }

        public override void ExecuteRoleActions()
        {
            UI.ClearAndHeader("Guest Catalog");

            if (LibraryDatabase.Catalog.Count == 0)
            {
                Console.WriteLine("No Books available\n");
                UI.Pause();
                return;
            }

            foreach (Book book in LibraryDatabase.Catalog.Values)
            {
                string status = book.IsBorrowed ? "Currently borrowed" : "Available";
                Console.WriteLine(string.Format("[ID: {0}] {1}, {2} by {3}; Price = {4} PKR ; {5}", 
                    book.BookID, book.Name, book.Genre, book.Publisher, book.Cost, status));
            }
            
            Console.WriteLine();
            UI.Pause();
        }
    }

    class UI
    {
        public static Dictionary<int, string> RegisteredUsers = new Dictionary<int, string>();

        public static void ClearAndHeader(string title)
        {
            Console.Clear(); 
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==================================================");
            
            int spaces = (50 - title.Length) / 2;
            string padding = new string(' ', spaces > 0 ? spaces : 0);
            
            Console.WriteLine(padding + title.ToUpper());
            Console.WriteLine("==================================================\n");
            Console.ResetColor();
        }

        // --- NEW: Random Quote Generator ---
        public static void DisplayRandomQuote()
        {
            List<string> quotes = new List<string>
            {
                "\"The more that you read, the more things you will know. The more that you learn, the more places you'll go.\" – Dr. Seuss",
                "\"A room without books is like a body without a soul.\" – Marcus Tullius Cicero",
                "\"Live as if you were to die tomorrow. Learn as if you were to live forever.\" – Mahatma Gandhi",
                "\"An investment in knowledge pays the best interest.\" – Benjamin Franklin",
                "\"Books are a uniquely portable magic.\" – Stephen King",
                "\"Intellect without ambition is a bird without wings.\" – Salvador Dalí",
                "\"Education is the passport to the future, for tomorrow belongs to those who prepare for it today.\" – Malcolm X",
                "\"Reading is to the mind what exercise is to the body.\" – Joseph Addison"
            };

            Random rand = new Random();
            int index = rand.Next(quotes.Count);

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Quote of the Moment:");
            Console.WriteLine(quotes[index]);
            Console.ResetColor();
            Console.WriteLine();
        }

        public static int ReadInt32(string prompt)
        {
            Console.Write(prompt);
            string input = Console.ReadLine() ?? "";
            int result;

            if (int.TryParse(input, out result)) return result;

            PrintError("Invalid entry! Defaulting to 0.");
            return 0;
        }

        public static void PrintSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n[SUCCESS] " + message);
            Console.ResetColor();
        }

        public static void PrintError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n[ERROR] " + message);
            Console.ResetColor();
        }

        public static void PrintWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n[WARNING] " + message);
            Console.ResetColor();
        }

        public static void Pause()
        {
            Console.WriteLine("\nPress ENTER to continue...");
            Console.ReadLine();
        }

        static void Main(string[] args)
        {
            RegisteredUsers.Add(39393, "Muhammad Arhum Irfan");
            RegisteredUsers.Add(39425, "Ghayyur Abbas");
            RegisteredUsers.Add(40142, "Wazir Muzzamil Hussain");
            RegisteredUsers.Add(39358, "Muhammad Whahaj");
            RegisteredUsers.Add(39859, "Insfalullah Khan");

            LibraryDatabase.LoadFromFile();

            while (true)
            {
                ClearAndHeader("Library Management System");

                DisplayRandomQuote();

                Console.WriteLine("Identify your role (Admin/User/Guest) or type 'exit' to quit: ");
                string inputRole = Console.ReadLine() ?? "";

                if (inputRole.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    LibraryDatabase.SaveToFile();
                    PrintWarning("Closing execution thread session. Goodbye!");
                    Thread.Sleep(1000);
                    break;
                }

                SystemRole chosenRole = SystemRole.Unknown;
                if (Enum.TryParse(inputRole, true, out chosenRole))
                {
                }

                Login userSession = null;

                if (chosenRole == SystemRole.Admin)
                {
                    VerifyAdmin admin = new VerifyAdmin();
                    Console.Write("\nEnter the Admin Username: ");
                    admin.AdminUserName = Console.ReadLine() ?? "";

                    Console.Write("Enter the Admin Password: ");
                    admin.AdminPassword = Console.ReadLine() ?? "";
                    
                    Thread.Sleep(500);
                    userSession = admin;
                }
                else if (chosenRole == SystemRole.User)
                {
                    int memberId = ReadInt32("\nEnter your Member ID: ");

                    if (RegisteredUsers.ContainsKey(memberId))
                    {
                        VerifyUser user = new VerifyUser();
                        user.Username = RegisteredUsers[memberId]; 

                        PrintSuccess("Welcome, " + user.Username + "!");
                        Console.Write("Enter the password: ");
                        user.UserPassword = Console.ReadLine() ?? "";

                        Thread.Sleep(500);
                        userSession = user;
                    }
                    else
                    {
                        PrintError("Member ID not recognized in the system.");
                        Pause();
                        continue;
                    }
                }
                else if (chosenRole == SystemRole.Guest)
                {
                    GuestUser guest = new GuestUser();
                    guest.Age = ReadInt32("\nEnter your age to proceed: ");
                    
                    Thread.Sleep(500);
                    userSession = guest;
                }
                else
                {
                    PrintError("Invalid system chosen! Try again.");
                    Pause();
                    continue;
                }

                if (userSession != null)
                {
                    userSession.ExecuteRoleActions();
                }
            }
        }
    }
}