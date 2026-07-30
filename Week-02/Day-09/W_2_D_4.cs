/*

UserNames & Password

--Admin:
UserName:"Admin"
Password:"admin123"
----

--User
UserName:"User"
Password:"User123"
----

--Guest
No UserName or PassWord Required
----

*/
using System;
using System.Collections.Generic;

namespace Lib 
 {   
    public static class LibarayDatabase
    {
        public static List<Book> Books = new List<Book>();
    }

    public interface IBorrower
    {
        void BorrowBook();
        void ReturnBook(); 
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
                else { Console.WriteLine("Correct Username entered."); adminUserName = value; }
            }
        }
        
        private String adminPassword = "admin123";
        public string AdminPassword
        {
            get { return adminPassword; }
            set
            {
                if (value != "admin123") { Console.WriteLine("Incorrect password Entred"); Environment.Exit(0); }
                else { Console.WriteLine("Correct password entered."); adminPassword = value; }
            }
        }

        public override void ExecuteRoleActions()
        {
            Imp.gap(); 
            Console.WriteLine("--ADMIN--: BOOK ENTRY PANAL:");

            Book book = new Book();

            Console.WriteLine("Enter the book name : ");
            book.name = Console.ReadLine() ?? "";

            Console.WriteLine("Enter your book publihser : ");
            book.publisher = Console.ReadLine() ?? "";

            book.datePublish = Imp.ReadInt32("Enter the Book Year: ");

            Console.WriteLine("Enter the book genre: ");
            book.genre = Console.ReadLine() ?? "";

            book.Cost = Imp.ReadInt32("Enter the book price:");

            LibarayDatabase.Books.Add(book);

            Imp.gap();
            Console.WriteLine("Successfully added " + book.name + " ! ");    
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
            BorrowBook();
            ReturnBook();
        }

       
        public void BorrowBook()
        {
            Console.WriteLine("Borrow Book: Code not implemented");
        }
        public void ReturnBook()
        {
            Console.WriteLine("Return Book : Code not implemented");
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
                Console.WriteLine("No Book avaliable");
                return;
            }

            foreach(var book in LibarayDatabase.Books)
            {
                Console.WriteLine(" " + book.name + "," + book.genre + " by " + book.publisher + "; Price = " + book.Cost + " PKR ");
            }
        }
    }

    public class Book
    {   
        private int _cost; 
        public int Cost
        {
            get { return _cost; }
            set { _cost = value < 0 ? 0 : value; } 
        }

        public string name;
        public string publisher;
        public int datePublish;
        public string genre;

        public Book()
        {
            name = "unknown";
            publisher = "unknown";
            datePublish = 2026;
            genre = "unknown";
            _cost = 0; 
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
            gap();
            Console.WriteLine(" Identify your role (Admin/User/Guest) ");
            string inputRole = Console.ReadLine() ?? "";

            Login userSession = null;

            if (inputRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                VerifyAdmin admin = new VerifyAdmin();
                Console.WriteLine("Enter the Admin Username : ");
                admin.AdminUserName = Console.ReadLine() ?? "";

                Console.WriteLine("Enter the Admin Password : ");
                admin.AdminPassword = Console.ReadLine() ?? "";

                userSession = admin; 
            }
            else if (inputRole.Equals("User", StringComparison.OrdinalIgnoreCase))
            {
                VerifyUser user = new VerifyUser();
                Console.WriteLine("Enter the username : ");
                user.Username = Console.ReadLine() ?? "";
                
                Console.WriteLine("Enter the password : ");
                user.UserPassword = Console.ReadLine() ?? ""; 

                userSession = user; 
            }
            else if (inputRole.Equals("Guest", StringComparison.OrdinalIgnoreCase))
            {
                GuestUser guest = new GuestUser(); 
                guest.Age = ReadInt32("Enter your age to proceed: ");
                
                userSession = guest; 
            }
            else
            {
                Console.WriteLine("Invalid system chosen!");
                return;
            }

            if (userSession != null)
            {
                userSession.ExecuteRoleActions();
            }
        }
    }
}