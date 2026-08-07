using System;
using System.IO;
using System.Linq;

namespace Lib
{
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
                Console.WriteLine("6. View Transaction History");
                Console.WriteLine("7. Advance Library Analytics");
                Console.WriteLine("8. Interactive Book search");
                Console.WriteLine("9. Logout / Main menu\n");

                int selection = UI.ReadInt32("Please choose an option: ");

                if (selection == 1)
                {
                    AddBook();
                }
                else if (selection == 2)
                {
                    ViewInventory();
                }
                else if (selection == 3)
                {
                    FullDetailedReport();
                }
                else if (selection == 4)
                {
                    UserDetailsReport();
                }
                else if (selection == 5)
                {
                    RegisterNewMember();
                }
                else if (selection == 6)
                {
                    ViewTransactionHistory();
                }
                else if (selection == 7)
                {
                    AdvancedAnalytics();
                }
                else if (selection == 8)
                {
                    InteractiveSearch();
                }
                else if (selection == 9)
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

        private void AddBook()
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

        private void ViewInventory()
        {
            UI.ClearAndHeader("Live Inventory Tracking");
            Console.WriteLine("Total Books Stored: " + LibraryDatabase.TotalBooksCount + "\n");
            LibraryDatabase.DisplayAvailableTitles();
            UI.Pause();
        }

        private void FullDetailedReport()
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

        private void UserDetailsReport()
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

        private void RegisterNewMember()
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

        private void ViewTransactionHistory()
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

        private void AdvancedAnalytics()
        {
            UI.ClearAndHeader("Advanced Library Analytics");

            if (LibraryDatabase.Catalog.Count == 0)
            {
                Console.WriteLine("No books available for analysis.");
                return;
            }

            // 1. Total Valuation & Average Cost
            int totalValue = LibraryDatabase.Catalog.Values.Sum(b => b.Cost);
            double avgCost = LibraryDatabase.Catalog.Values.Average(b => b.Cost);
            Book expensiveBook = LibraryDatabase.Catalog.Values.OrderByDescending(b => b.Cost).FirstOrDefault();

            Console.WriteLine("--- Financial Overview ---");
            Console.WriteLine("Total Inventory: " + totalValue + " PKR");
            Console.WriteLine("Average Book: " + avgCost.ToString("F2") + " PKR");
            if (expensiveBook != null)
            {
                Console.WriteLine("Most Expensive Book: " + expensiveBook.Name + " (" + expensiveBook.Cost + " PKR)");
            }
            Console.WriteLine("--------------------------------------------------");

            // 2. Genre Distribution Breakdown
            Console.WriteLine("--- Genre Distribution ---");
            var genreGroups = LibraryDatabase.Catalog.Values
                .GroupBy(b => b.Genre)
                .Select(g => new { Genre = g.Key, Count = g.Count() });

            foreach (var group in genreGroups)
            {
                Console.WriteLine(" Genre: " + group.Genre + " | Books Count: " + group.Count);
            }
            Console.WriteLine("--------------------------------------------------");

            // 3. Overdue Books Radar
            Console.WriteLine("--- OverDue Books Radar ---");
            var overdueBooks = LibraryDatabase.Catalog.Values
                .Where(b => b.IsBorrowed && DateTime.Now > b.DueDate)
                .ToList();

            if (overdueBooks.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(" Status: All checked-out books are currently on time!");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" Alert: " + overdueBooks.Count + " book(s) are currently overdue!");
                foreach (var b in overdueBooks)
                {
                    Console.WriteLine("   * '" + b.Name + "' (Borrowed by: " + b.BorrowedBy + ") - Due: " + b.DueDate.ToString("yyyy-MM-dd"));
                }
                Console.ResetColor();
            }
            Console.WriteLine("--------------------------------------------------");

            // 4. Active Borrowers Leaderboard
            Console.WriteLine("--- Active Borrowers Leaderboard ---");
            var topBorrowers = LibraryDatabase.Catalog.Values
                .Where(b => b.IsBorrowed)
                .GroupBy(b => b.BorrowedBy)
                .Select(g => new { Username = g.Key, ActiveCount = g.Count() })
                .OrderByDescending(g => g.ActiveCount)
                .ToList();

            if (topBorrowers.Count == 0)
            {
                Console.WriteLine(" No members currently have books checked out.");
            }
            else
            {
                foreach (var borrower in topBorrowers)
                {
                    Console.WriteLine(" User: " + borrower.Username + " | Active Books: " + borrower.ActiveCount);
                }
            }
            Console.WriteLine("--------------------------------------------------");

            UI.Pause();
        }

        private void InteractiveSearch()
        {
            UI.ClearAndHeader("Interactive Book Serach Engine");

            Console.WriteLine("Enter Keyword(Title or author, or press Enter to skip): ");
            string keyword = Console.ReadLine() ?? "";

            Console.WriteLine("Enter Genre filter (or press Enter to skip): ");
            string genre = Console.ReadLine() ?? "";

            Console.Write("Enter Maximum Budget / Cost in PKR (e.g., 5000): ");
            string priceInput = Console.ReadLine() ?? "";

            int maxPrice;
            if (!int.TryParse(priceInput, out maxPrice))
            {
                maxPrice = int.MaxValue; // Defaults to maximum if they leave it blank or type invalid text
            }

            var results = LibraryDatabase.SearchBooks(keyword, genre, maxPrice);

            Console.WriteLine("\n--- Search Results (" + results.Count + "found)---");
            if (results.Count == 0)
            {
                UI.PrintError("No Books match your specific critera");
            }
            else
            {
                foreach (var b in results)
                {
                    string status = b.IsBorrowed ? "(Borrowed)" : "(Available)";
                    Console.WriteLine("*-> " + b.Name + " by " + b.Genre + " | Cost: " + b.Cost + " PKR " + status);
                }
            }

            Console.WriteLine("----------------------------------------------------------");
            UI.Pause();
        }
    }
}
