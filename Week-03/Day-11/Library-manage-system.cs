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
using System.Linq; // Added to easily find the highest Book ID

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
        // --- New: Unique Identifier ---
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

        // Parameterless constructor
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
        // --- Transformed to a Dictionary (Key: BookID, Value: Book Object) ---
        public static Dictionary<int, Book> Catalog = new Dictionary<int, Book>();

        public static int TotalBooksCount
        {
            get { return Catalog.Count; }
        }

        private const string SaveFilePath = "library.txt";

        public static void Seed()
        {
            // --- 3 Copies of The Great Gatsby ---
            Catalog.Add(101, new Book(101, "The Great Gatsby", "Penguin", 1990, "Fantasy", 1000));
            Catalog.Add(102, new Book(102, "The Great Gatsby", "Penguin", 1990, "Fantasy", 1000));
            Catalog.Add(103, new Book(103, "The Great Gatsby", "Penguin", 1990, "Fantasy", 1000));
            
            // --- 2 Copies of To Kill a Mockingbird ---
            Book mockingbird1 = new Book(104, "To Kill a Mockingbird", "Lippincott", 1960, "Classic", 1200);
            mockingbird1.FineDue = 500; // Someone returned this copy late
            Catalog.Add(104, mockingbird1);
            
            Catalog.Add(105, new Book(105, "To Kill a Mockingbird", "Lippincott", 1960, "Classic", 1200));

            // --- Single Copies ---
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
                        
                        // Updated to 11 parts to include the new BookID at index 0
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
                    Console.WriteLine("Library data loaded from " + SaveFilePath);
                }
                else
                {
                    Seed();
                    Console.WriteLine("No saved data found, starting with default catalog.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not load saved data (" + ex.Message + "). Starting with default catalog.");
                Seed();
            }
        }

        public static void SaveToFile()
        {
            try
            {
                List<string> lines = new List<string>();
                
                // Iterating through the Dictionary's Values
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
                Console.WriteLine("Warning: could not save library data (" + ex.Message + ")");
            }
        }

        public static void DisplayAvailableTitles()
        {
            Console.WriteLine("\n---Current Library Catalog Inventory---");
            if (Catalog.Count == 0)
            {
                Console.WriteLine("No Books stored in memory");
                return;
            }

            foreach (Book b in Catalog.Values)
            {
                string status = b.IsBorrowed ? "BORROWED (Due: " + b.DueDate.ToString("yyyy-MM-dd") + ")" : "Available";
                // Displaying the Book ID so the user knows what to type
                Console.WriteLine(string.Format("[ID: {0}] {1} ({2}) - {3}", b.BookID, b.Name, b.Genre, status));
            }
            Console.WriteLine("-------------------------------------");
        }

        public static void AdvancedSearch()
        {
            Imp.Gap();
            Console.WriteLine("--- Advanced Search & Filtering ---");
            Console.WriteLine("1. Search by Title Keyword");
            Console.WriteLine("2. Search by Genre");
            Console.WriteLine("3. Search by Price Range");
            Console.WriteLine("4.Search by Book ID");
            Console.WriteLine("5. Return to Previous Menu");
            
            int choice = Imp.ReadInt32("Enter your choice: ");
            
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
                Console.Write("Enter genre to search for (e.g., Fantasy, Classic): ");
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
                int minPrice = Imp.ReadInt32("Enter minimum price (PKR): ");
                int maxPrice = Imp.ReadInt32("Enter maximum price (PKR): ");
                
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
                int searchId = Imp.ReadInt32("Enter the exact Book ID : ");

                if (Catalog.ContainsKey(searchId))
                {
                    results.Add(Catalog[searchId]);
                }
                else
                {
                    Console.WriteLine("No Book found With that ID. ");
                }
            }
            else
            {
                return; 
            }

            Imp.Gap();
            Console.WriteLine("--- Search Results ---");
            
            if (results.Count == 0)
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
                if (value != "Admin") { Console.WriteLine("Incorrect Username entered!"); Environment.Exit(0); }
                else { Console.WriteLine(" Correct Admin Username input "); adminUserName = value; }
            }
        }

        private String adminPassword = "admin123";
        public string AdminPassword
        {
            get { return adminPassword; }
            set
            {
                if (value != "admin123") { Console.WriteLine("Incorrect password Entered"); Environment.Exit(0); }
                else { Console.WriteLine(" Correct Admin Password Input "); adminPassword = value; }
            }
        }

        public override void ExecuteRoleActions()
        {
            while (true)
            {
                Imp.Gap();
                Console.WriteLine("--ADMIN--: BOOK ENTRY PANEL:");
                Console.WriteLine("1.Add a new book");
                Console.WriteLine("2.View current Inventory");
                Console.WriteLine("3.Logout / Mainmenu");

                int selection = Imp.ReadInt32("Please chose a choice number : ");

                if (selection == 1)
                {
                    Imp.Gap();
                    Console.WriteLine(" Admin Book Entry Panel ");

                    Console.WriteLine("Enter the book name : ");
                    string bName = Console.ReadLine() ?? "";

                    Console.WriteLine("Enter your book publisher : ");
                    string bPub = Console.ReadLine() ?? "";

                    int bYear = Imp.ReadInt32("Enter the Book Year: ");

                    Console.WriteLine("Enter the book genre: ");
                    string bGenre = Console.ReadLine() ?? "";

                    int bCost = Imp.ReadInt32("Enter the book price: ");

                    // Automatically generate the next available ID
                    int newId = 101;
                    if (LibraryDatabase.Catalog.Count > 0)
                    {
                        newId = LibraryDatabase.Catalog.Keys.Max() + 1;
                    }

                    Book book = new Book(newId, bName, bPub, bYear, bGenre, bCost);
                    LibraryDatabase.Catalog.Add(newId, book);
                    LibraryDatabase.SaveToFile();

                    Imp.Gap();
                    Console.WriteLine("Successfully added " + book.Name + " with ID [" + newId + "]!");
                    Console.WriteLine("Total books in live runtime memory: " + LibraryDatabase.TotalBooksCount);
                }
                else if (selection == 2)
                {
                    Imp.Gap();
                    Console.WriteLine("Live tracking Inventory count: " + LibraryDatabase.TotalBooksCount + " Books stored. ");
                    LibraryDatabase.DisplayAvailableTitles();
                }
                else if (selection == 3)
                {
                    Imp.Gap();
                    Console.WriteLine("Logging out of the System.......");
                    break;
                }
                else
                {
                    Console.WriteLine("Incorrect input! try again");
                }
            }
        }
    }

    public class VerifyUser : Login, IBorrower
    {
        // Now holds the actual user's name instead of just "User"
        public string Username { get; set; } 

        private string userPassword = "User123";
        public string UserPassword
        {
            get { return userPassword; }
            set
            {
                if (value != "User123") { Console.WriteLine("Incorrect password !"); Environment.Exit(0); }
                else { Console.WriteLine(" Correct Password Input "); userPassword = value; }
            }
        }

        public override void ExecuteRoleActions()
        {
            while (true)
            {
                Imp.Gap();
                Console.WriteLine(Username + "'s Dashboard");
                Console.WriteLine("1. To Borrow a Book");
                Console.WriteLine("2. Return a Book");
                Console.WriteLine("3. Pay Outstanding Fine");
                Console.WriteLine("4. Advanced Search & Filtering");
                Console.WriteLine("5. Log out/Main menu");
                
                int selection = Imp.ReadInt32("Enter your choice: ");

                if (selection == 1)
                {
                    BorrowBook();
                }
                else if (selection == 2)
                {
                    ReturnBook();
                }
                else if (selection == 3)
                {
                    PayFineBook();
                }
                else if (selection == 4)
                {
                    LibraryDatabase.AdvancedSearch(); 
                }
                else if (selection == 5)
                {
                    Imp.Gap();
                    Console.WriteLine("Logging out of the System.......");
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid Input! Please choose 1-5.");
                }
            }
        }

        public void BorrowBook()
        {   


            int borrowedCount = 0;

            foreach(Book book in LibraryDatabase.Catalog.Values)
            {
                if (book.IsBorrowed && book.BorrowedBy == Username)
                {
                    borrowedCount++;
                }
            }

            if(borrowedCount >= 3)
            {
                Imp.Gap();
                Console.WriteLine("Limit Reached: You already have "+ borrowedCount+ " book checked out. ");
                Console.WriteLine("Please return a book before borrowing a new one.");
                return;
            }


            LibraryDatabase.DisplayAvailableTitles();

            // Simplified to an O(1) Dictionary Lookup
            int targetID = Imp.ReadInt32("\nEnter the ID of the book to be borrowed: ");

            if (!LibraryDatabase.Catalog.ContainsKey(targetID))
            {
                Console.WriteLine("Error: Book ID [" + targetID + "] not found in the catalog.");
                return;
            }

            Book foundBook = LibraryDatabase.Catalog[targetID];

            if (foundBook.IsBorrowed)
            {
                Console.WriteLine(foundBook.Name + " is currently borrowed by " +
                    (foundBook.BorrowedBy == Username ? "you already" : "someone else") + "!");
                return;
            }

            foundBook.IsBorrowed = true;
            foundBook.BorrowedBy = Username;
            foundBook.BorrowedDate = DateTime.Now; 
            foundBook.DueDate = DateTime.Now.AddDays(14); 

            LibraryDatabase.SaveToFile();

            Imp.Gap();
            Console.WriteLine(foundBook.Name + " borrowed successfully! Price = " + foundBook.Cost + " PKR");
            Console.WriteLine("IMPORTANT: Your due date is " + foundBook.DueDate.ToString("yyyy-MM-dd") + ".");
        }

        public void ReturnBook()
        {
            LibraryDatabase.DisplayAvailableTitles();

            int targetID = Imp.ReadInt32("\nEnter the Book ID to return: ");

            if (!LibraryDatabase.Catalog.ContainsKey(targetID))
            {
                Console.WriteLine("Error: Book ID [" + targetID + "] not found in the catalog.");
                return;
            }

            Book foundBook = LibraryDatabase.Catalog[targetID];

            if (!foundBook.IsBorrowed)
            {
                Console.WriteLine("This book wasn't marked as borrowed, so there's nothing to return.");
                return;
            }

            if (DateTime.Now > foundBook.DueDate)
            {
                int daysLate = (DateTime.Now - foundBook.DueDate).Days; 
                if (daysLate <= 0) daysLate = 1; 
                
                int fineAmount = daysLate * 50; 
                foundBook.FineDue += fineAmount;

                Imp.Gap();
                Console.WriteLine("WARNING: This book is " + daysLate + " days late.");
                Console.WriteLine("An automatic late fee of " + fineAmount + " PKR has been added to the account.");
            }

            foundBook.IsBorrowed = false;
            foundBook.BorrowedBy = "";
            foundBook.BorrowedDate = DateTime.MinValue; 
            foundBook.DueDate = DateTime.MinValue;      
            LibraryDatabase.SaveToFile();

            Imp.Gap();
            Console.WriteLine("Thank you for returning the book: " + foundBook.Name);
        }

        public void PayFineBook()
        {
            Console.WriteLine("\n---Fine--Payment---");
            LibraryDatabase.DisplayAvailableTitles();

            int targetID = Imp.ReadInt32("Enter the Book ID to settle fines: ");
            
            if (LibraryDatabase.Catalog.ContainsKey(targetID))
            {
                Book foundBook = LibraryDatabase.Catalog[targetID];

                if (foundBook.FineDue <= 0)
                {
                    Console.WriteLine("Good News! There is no outstanding fine for this book.");
                }
                else
                {
                    Console.WriteLine("Outstanding late fees for " + foundBook.Name + " is: " + foundBook.FineDue + " PKR ");
                    int payment = Imp.ReadInt32("Enter the Amount to pay for the fine: ");

                    if (payment <= 0)
                    {
                        Console.WriteLine("Invalid amount of payment value given, Transaction cancelled");
                    }
                    else if (payment > foundBook.FineDue)
                    {
                        int change = payment - foundBook.FineDue;
                        Console.WriteLine("Transaction Completed! your change is : " + change + " PKR ");
                        foundBook.FineDue = 0;
                        LibraryDatabase.SaveToFile();
                    }
                    else
                    {
                        foundBook.FineDue -= payment;
                        LibraryDatabase.SaveToFile();
                        Console.WriteLine("Payment accepted! Your remaining payment is : " + foundBook.FineDue + " PKR ");
                    }
                }
            }
            else
            {
                Console.WriteLine("Error: Book ID [" + targetID + "] not found.");
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
                if (value < 18) { Console.WriteLine("You are Under 18!"); Environment.Exit(0); }
                else if (value > 110) { Console.WriteLine("Too Old!"); Environment.Exit(0); }
                else { Console.WriteLine("Access Allowed"); age = value; }
            }
        }

        public override void ExecuteRoleActions()
        {
            Imp.Gap();
            Console.WriteLine("\n --Guest Catalog--");

            if (LibraryDatabase.Catalog.Count == 0)
            {
                Console.WriteLine("\n No Books available");
                return;
            }

            foreach (Book book in LibraryDatabase.Catalog.Values)
            {
                string status = book.IsBorrowed ? "Currently borrowed" : "Available";
                Console.WriteLine(string.Format("[ID: {0}] {1}, {2} by {3}; Price = {4} PKR ; {5}", 
                    book.BookID, book.Name, book.Genre, book.Publisher, book.Cost, status));
            }
        }
    }

    class Imp
    {
        // --- New: Registered Users Registry ---
        public static Dictionary<int, string> RegisteredUsers = new Dictionary<int, string>();

        public static void ClearAndHeader(string title)
        {
            Console.Clear(); // Wipes the terminal clean
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==================================================");
            
            // Centers the title roughly based on the 50-character border
            int spaces = (50 - title.Length) / 2;
            string padding = new string(' ', spaces > 0 ? spaces : 0);
            
            Console.WriteLine(padding + title.ToUpper());
            Console.WriteLine("==================================================\n");
            Console.ResetColor();
        }

        public static int ReadInt32(string prompt)
        {
            Console.Write(prompt);
            string input = Console.ReadLine() ?? "";
            int result;

            if (int.TryParse(input, out result)) return result;

            Console.WriteLine("Invalid entry! Defaulting to 0.");
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

        // Pauses the screen so the user can read messages before the screen clears again
        public static void Pause()
        {
            Console.WriteLine("\nPress ENTER to continue...");
            Console.ReadLine();
        }
    }

        static void Main(string[] args)
        {
            // Seed the users dictionary
            RegisteredUsers.Add(39393, "Muhammad Arhum Irfan");
            RegisteredUsers.Add(39425, "Ghayyur Abbas");
            RegisteredUsers.Add(40142,"Wazir Muzzammil Hussain");
            RegisteredUsers.Add(39358,"Muhammad Whahaj");
            RegisteredUsers.Add(39859,"Insafullah Khan");

            LibraryDatabase.LoadFromFile();

            while (true)
            {
                Gap();
                Console.WriteLine("\n Identify your role (Admin/User/Guest) or type 'exit' to quit: ");
                string inputRole = Console.ReadLine() ?? "";

                if (inputRole.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    LibraryDatabase.SaveToFile();
                    Console.WriteLine("Closing execution thread session. Goodbye!");
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
                    Console.WriteLine("Enter the Admin Username : ");
                    admin.AdminUserName = Console.ReadLine() ?? "";

                    Console.WriteLine("Enter the Admin Password : ");
                    admin.AdminPassword = Console.ReadLine() ?? "";

                    userSession = admin;
                }
                else if (chosenRole == SystemRole.User)
                {
                    // Update: User login now requests a Member ID
                    int memberId = ReadInt32("Enter your Member ID: ");

                    if (RegisteredUsers.ContainsKey(memberId))
                    {
                        VerifyUser user = new VerifyUser();
                        user.Username = RegisteredUsers[memberId]; // Assigns the real name

                        Console.WriteLine("Welcome, " + user.Username + "!");
                        Console.WriteLine("Enter the password : ");
                        user.UserPassword = Console.ReadLine() ?? "";

                        userSession = user;
                    }
                    else
                    {
                        Console.WriteLine("Error: Member ID not recognized in the system.");
                        continue;
                    }
                }
                else if (chosenRole == SystemRole.Guest)
                {
                    GuestUser guest = new GuestUser();
                    guest.Age = ReadInt32("Enter your age to proceed: ");

                    userSession = guest;
                }
                else
                {
                    Console.WriteLine("Invalid system chosen! Try again.");
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