/*
--UserNames & Password for all types of users--
--
--Admin: UserName: "Admin", Password: "admin123".--
--
--User: UserName: "User", Password: "User123".--
--
--Guest: No UserName or Password Required, just have to be 18 and above.--
--
*/

using System;
using System.Collections.Generic;
using System.IO;

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
        public string Name { get; set; }
        public string Publisher { get; set; }
        public int DatePublish { get; set; }
        public string Genre { get; set; }
        public int Cost { get; set; }
        public int FineDue { get; set; }

        public bool IsBorrowed { get; set; }
        public string BorrowedBy { get; set; }

        // --- New: Due Date Tracking ---
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

        public Book(string name, string publisher, int datePublish, string genre, int cost)
        {
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
        public static List<Book> Books = new List<Book>();

        public static int TotalBooksCount
        {
            get { return Books.Count; }
        }

        private const string SaveFilePath = "library.txt";

        public static void Seed()
        {
            Book book1 = new Book("The Great Gatsby", "Penguin", 1990, "Fantasy", 1000);
            Books.Add(book1);

            Book book2 = new Book("To Kill a Mockingbird", "Lippincott", 1960, "Classic", 1200);
            book2.FineDue = 500;
            Books.Add(book2);
        }

        public static void LoadFromFile()
        {
            try
            {
                if (File.Exists(SaveFilePath))
                {
                    Books.Clear();
                    string[] lines = File.ReadAllLines(SaveFilePath);
                    
                    foreach (string line in lines)
                    {
                        string[] parts = line.Split('|');
                        
                        // Updated to expect 10 properties instead of 8
                        if (parts.Length == 10)
                        {
                            Book b = new Book();
                            b.Name = parts[0];
                            b.Publisher = parts[1];
                            b.DatePublish = int.Parse(parts[2]);
                            b.Genre = parts[3];
                            b.Cost = int.Parse(parts[4]);
                            b.FineDue = int.Parse(parts[5]);
                            b.IsBorrowed = bool.Parse(parts[6]);
                            b.BorrowedBy = parts[7];
                            b.BorrowedDate = DateTime.Parse(parts[8]); // Parse saved date
                            b.DueDate = DateTime.Parse(parts[9]);      // Parse saved date
                            
                            Books.Add(b);
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
                
                foreach (Book b in Books)
                {
                    // Added {8} and {9} to save the BorrowedDate and DueDate as strings
                    string line = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}", 
                        b.Name, b.Publisher, b.DatePublish, b.Genre, b.Cost, b.FineDue, 
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
            if (Books.Count == 0)
            {
                Console.WriteLine("No Books stored in memory");
                return;
            }

            for (int i = 0; i < Books.Count; i++)
            {
                string status = Books[i].IsBorrowed ? "BORROWED (Due: " + Books[i].DueDate.ToString("yyyy-MM-dd") + ")" : "Available";
                Console.WriteLine((i + 1) + " . " + Books[i].Name + " (" + Books[i].Genre + ") - " + status);
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
            Console.WriteLine("4. Return to Previous Menu");
            
            int choice = Imp.ReadInt32("Enter your choice: ");
            
            List<Book> results = new List<Book>();

            if (choice == 1)
            {
                Console.Write("Enter a keyword to search in the title: ");
                string keyword = Console.ReadLine() ?? "";
                
                foreach (var book in Books)
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
                
                foreach (var book in Books)
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
                
                foreach (var book in Books)
                {
                    if (book.Cost >= minPrice && book.Cost <= maxPrice)
                    {
                        results.Add(book);
                    }
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
                for (int i = 0; i < results.Count; i++)
                {
                    string status = results[i].IsBorrowed ? "BORROWED" : "Available";
                    Console.WriteLine(string.Format("{0}. {1} ({2}) - {3} PKR - {4}", (i + 1), results[i].Name, results[i].Genre, results[i].Cost, status));
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

                    Book book = new Book(bName, bPub, bYear, bGenre, bCost);
                    LibraryDatabase.Books.Add(book);
                    LibraryDatabase.SaveToFile();

                    Imp.Gap();
                    Console.WriteLine("Successfully added " + book.Name + " ! ");
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
        private string username = "User";
        public string Username
        {
            get { return username; }
            set
            {
                if (value != "User") { Console.WriteLine("Incorrect username !"); Environment.Exit(0); }
                else { Console.WriteLine(" Correct Username Input "); username = value; }
            }
        }

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
                Console.WriteLine("User-Dashboard");
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
            LibraryDatabase.DisplayAvailableTitles();

            Console.WriteLine("\nEnter the name of the book to be borrowed: ");
            string target = Console.ReadLine() ?? "";

            Book foundBook = null;

            foreach (var book in LibraryDatabase.Books)
            {
                if (book.Name.Equals(target, StringComparison.OrdinalIgnoreCase))
                {
                    foundBook = book;
                    break;
                }
            }

            if (foundBook == null)
            {
                Console.WriteLine("Incorrect! No '" + target + "' book names matching!");
                return;
            }

            if (foundBook.IsBorrowed)
            {
                Console.WriteLine(foundBook.Name + " is currently borrowed by " +
                    (foundBook.BorrowedBy == Username ? "you already" : "someone else") + "!");
                return;
            }

            // --- New: Assigning the Dates ---
            foundBook.IsBorrowed = true;
            foundBook.BorrowedBy = Username;
            foundBook.BorrowedDate = DateTime.Now; // Gets current exact date and time
            foundBook.DueDate = DateTime.Now.AddDays(14); // Due exactly 14 days from now

            LibraryDatabase.SaveToFile();

            Imp.Gap();
            Console.WriteLine(foundBook.Name + " borrowed successfully! Price = " + foundBook.Cost + " PKR");
            Console.WriteLine("IMPORTANT: Your due date is " + foundBook.DueDate.ToString("yyyy-MM-dd") + ".");
        }

        public void ReturnBook()
        {
            LibraryDatabase.DisplayAvailableTitles();

            Console.WriteLine("\nEnter the book to return: ");
            string target = Console.ReadLine() ?? "";

            Book foundBook = null;
            foreach (var book in LibraryDatabase.Books)
            {
                if (book.Name.Equals(target, StringComparison.OrdinalIgnoreCase))
                {
                    foundBook = book;
                    break;
                }
            }

            if (foundBook == null)
            {
                Console.WriteLine("That book does not belong to our catalog inventory registry.");
                return;
            }

            if (!foundBook.IsBorrowed)
            {
                Console.WriteLine("This book wasn't marked as borrowed, so there's nothing to return.");
                return;
            }

            // --- New: Calculating the Fine ---
            if (DateTime.Now > foundBook.DueDate)
            {
                // Subtracting DueDate from Current Date gives us a TimeSpan. We pull the total .Days from it.
                int daysLate = (DateTime.Now - foundBook.DueDate).Days; 
                
                // If it's less than a full 24 hours late, it might register as 0 days. We ensure at least 1 day is charged.
                if (daysLate == 0) daysLate = 1; 
                
                int fineAmount = daysLate * 50; // 50 PKR per day
                foundBook.FineDue += fineAmount;

                Imp.Gap();
                Console.WriteLine("WARNING: This book is " + daysLate + " days late.");
                Console.WriteLine("An automatic late fee of " + fineAmount + " PKR has been added to the account for this book.");
            }

            foundBook.IsBorrowed = false;
            foundBook.BorrowedBy = "";
            foundBook.BorrowedDate = DateTime.MinValue; // Reset dates
            foundBook.DueDate = DateTime.MinValue;      // Reset dates
            LibraryDatabase.SaveToFile();

            Imp.Gap();
            Console.WriteLine("Thank you for returning the book: " + foundBook.Name);
        }

        public void PayFineBook()
        {
            Console.WriteLine("\n---Fine--Payment---");

            LibraryDatabase.DisplayAvailableTitles();

            Console.WriteLine("Enter the exact name of book to settle fines");
            string target = Console.ReadLine() ?? "";
            Book foundBook = null;

            foreach (var book in LibraryDatabase.Books)
            {
                if (book.Name.Equals(target, StringComparison.OrdinalIgnoreCase))
                {
                    foundBook = book;
                    break;
                }
            }

            if (foundBook != null)
            {
                if (foundBook.FineDue <= 0)
                {
                    Console.WriteLine("Good News! There is no outstanding fine for the book!");+++
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
                Console.WriteLine("Error: '" + target + "' could not be found in our system records");
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

            if (LibraryDatabase.Books.Count == 0)
            {
                Console.WriteLine("\n No Books available");
                return;
            }

            foreach (var book in LibraryDatabase.Books)
            {
                string status = book.IsBorrowed ? "Currently borrowed" : "Available";
                Console.WriteLine(" " + book.Name + ", " + book.Genre + " by " + book.Publisher + "; Price = " + book.Cost + " PKR ; " + status);
            }
        }
    }

    class Imp
    {
        public static void Gap()
        {
            Console.WriteLine("__--==++****++==--__");
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

        static void Main(string[] args)
        {
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
                    VerifyUser user = new VerifyUser();
                    Console.WriteLine("Enter the username : ");
                    user.Username = Console.ReadLine() ?? "";

                    Console.WriteLine("Enter the password : ");
                    user.UserPassword = Console.ReadLine() ?? "";

                    userSession = user;
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