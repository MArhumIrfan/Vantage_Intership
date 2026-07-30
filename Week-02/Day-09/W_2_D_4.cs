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

namespace Lib 
{   
    public enum SystemRole
    {
        Admin,
        User,
        Guest,
        Unknown
    }

    // FIX: Converted the record to a standard class to pass C# 5 compiler verification
    public class Book
    {
        public string Name { get; set; }
        public string Publisher { get; set; }
        public int DatePublish { get; set; }
        public string Genre { get; set; }
        public int Cost { get; set; }

        public int FineDue{get; set; }

        public Book(string name, string publisher, int datePublish, string genre, int cost)
        {
            Name = name;
            Publisher = publisher;
            DatePublish = datePublish;
            Genre = genre;
            Cost = cost;
        }
    }

    public static class LibarayDatabase
    {
        public static List<Book> Books = new List<Book>();
        
        public static int TotalBooksCount = 1;

        public static void seed()
        {   
            Book book1 = new Book("The Great Gatsby", "Penguin", 1990, "Fantasy", 1000);
            Books.Add(book1);

            Book book2 = new Book("To kill a Mockingbirf","Lippincott",1960,"Classic",1200);
            book2.FineDue = 500;
            Books.Add(book2);
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
                else { adminUserName = value; }
            }
        }
        
        private String adminPassword = "admin123";
        public string AdminPassword
        {
            get { return adminPassword; }
            set
            {
                if (value != "admin123") { Console.WriteLine("Incorrect password Entered"); Environment.Exit(0); }
                else { adminPassword = value; }
            }
        }

        public override void ExecuteRoleActions()
        {
            Imp.gap(); 
            Console.WriteLine("--ADMIN--: BOOK ENTRY PANEL:");

            Console.WriteLine("Enter the book name : ");
            string bName = Console.ReadLine() ?? "";

            Console.WriteLine("Enter your book publisher : ");
            string bPub = Console.ReadLine() ?? "";

            int bYear = Imp.ReadInt32("Enter the Book Year: ");

            Console.WriteLine("Enter the book genre: ");
            string bGenre = Console.ReadLine() ?? "";

            int bCost = Imp.ReadInt32("Enter the book price:");

            Book book = new Book(bName, bPub, bYear, bGenre, bCost);
            LibarayDatabase.Books.Add(book);

            LibarayDatabase.TotalBooksCount++; 

            Imp.gap();
            Console.WriteLine("Successfully added " + book.Name + " ! ");    
            Console.WriteLine("Total books in live runtime memory: " + LibarayDatabase.TotalBooksCount);
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
                else { Console.WriteLine("Correct Username input"); username = value; }
            }
        }

        private string userPassword = "user123";
        public string UserPassword
        {
            get { return userPassword; }
            set
            {
                if (value != "user123") { Console.WriteLine("Incorrect password !"); Environment.Exit(0); }
                else { userPassword = value; }
            }
        }

        public override void ExecuteRoleActions()
        {
            Imp.gap(); 
            Console.WriteLine("User-Dashboard");
            Console.WriteLine("1.To Borrow a Book");
            Console.WriteLine("2.Return a Book");
            Console.WriteLine("3.Pay Outstanding Fine");
            Console.WriteLine("4.Log out/Main menu");
            int selection = Imp.ReadInt32("Enter your choice");

            if (selection = 1)
            {
                
            
            BorrowBook();

            }
            else if (selection = 2)
            {
                
            
            ReturnBook();

            }
            else if (selection = 3)
            {
                
            
            PayFineBook();

            }

            else
            {
                Console.WriteLine("Invalid Input!, Logging out!");
            }
        }

        public void BorrowBook()
        {
            Console.WriteLine("\nEnter the name of the book to be borrowed: ");
            string target = Console.ReadLine() ?? "";
            
            Book foundBook = null; 
            
            foreach (var book in LibarayDatabase.Books)
            {
                if (book.Name.Equals(target, StringComparison.OrdinalIgnoreCase))
                {
                    foundBook = book;
                    break; 
                }
            }

            if (foundBook != null)
            {
                Console.WriteLine(foundBook.Name + " Book available for borrowing. Price = " + foundBook.Cost + " PKR");
            }
            else
            {
                Console.WriteLine("Incorrect! No '" + target + "' book names matching!");
            }
        }

        public void ReturnBook()
        {
            Console.WriteLine("\nEnter the book to return: ");
            string target = Console.ReadLine() ?? "";
            
            bool found = false;
            foreach (var book in LibarayDatabase.Books)
            {
                if (book.Name.Equals(target, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Book verified in system database!");
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Console.WriteLine("That book does not belong to our catalog inventory registry.");
            }
            else
            {
                Console.WriteLine("Thank you for returning the book: " + target);
            }
        }

        public void PayFineBook()
        {
            Console.WriteLine("\n---Fine--Payement---");
            Console.WriteLine("Enter the exact name of book to settle fines");
            string target = Console.ReadLine();
            Book foundBook = null;

            foreach (var book in LibarayDatabase.Books)
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
                    Console.WriteLine("Good News!, There is no outstanding fine for the book!");
                }

                else
                {
                    Console.WriteLine("Outstanding late fess for the "+ foundBook.Name+" is fine:  "+foundBook.FineDue+" PKR ");
                    int payment = Imp.ReadInt32("Enter the Amount to pay for the fine");


                    if (payment <= 0)
                    {
                        Console.WriteLine("Invalid amount of payment value given, Transaction cancelled");
                    }

                    else if (payment > foundBook.FineDue)
                    {
                        int change = payment - foundBook.FineDue;
                        Console.WriteLine("Transaction Completed! your change is : "+change+" PKR ");
                        foundBook.FineDue = 0;
                    }
                    else
                    {
                        foundBook.FineDue -= payment;
                        Console.WriteLine("Payment accpected!, Your remaining payment is : "+foundBook.FineDue +" PKR ");
                    }
                }
            }
            else
            {
                Console.WriteLine("Error: '"+target+"could not be found in our system records");
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
                if (value < 18) { Console.WriteLine("you are Under 18!"); Environment.Exit(0); }
                else if (value > 110) { Console.WriteLine("To Old !"); Environment.Exit(0); }
                else { Console.WriteLine("Access Allowed"); age = value; }
            }
        }

        public override void ExecuteRoleActions()
        {
            Imp.gap();
            Console.WriteLine("--Guest Catalog--");

            if (LibarayDatabase.Books.Count == 0) 
            {
                Console.WriteLine("No Book available");
                return;
            }

            foreach(var book in LibarayDatabase.Books)
            {
                Console.WriteLine(" " + book.Name + "," + book.Genre + " by " + book.Publisher + "; Price = " + book.Cost + " PKR ");
            }
        }
    }

    class Imp
    {
        public static void gap()  
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
            LibarayDatabase.seed();
            
            while (true)
            {
                gap();
                Console.WriteLine("Identify your role (Admin/User/Guest) or type 'exit' to quit: ");
                string inputRole = Console.ReadLine() ?? "";

                if (inputRole.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
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
