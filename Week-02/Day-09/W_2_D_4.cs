/*UserNames & Password
--
--Admin:UserName:"Admin"Password:"admin123"--
--
--UserUserName:"User"Password:"User123"--
--
--GuestNo UserName or PassWord Required--
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



    public static class LibarayDatabase
    {
        public static List<Book> Books = new List<Book>();
        
        
        public static int TotalBooksCount =1;


        public static void seed()
        {   
            Book book1 = new Book("The Great Gatsby","Penguin",1990,"Fanstasy",1000);
            Books.Add(book1);
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
                if (value != "admin123") { Console.WriteLine("Incorrect password Entred"); Environment.Exit(0); }
                else { adminPassword = value; }
            }
        }

        public override void ExecuteRoleActions()
        {
            Imp.gap(); 
            Console.WriteLine("--ADMIN--: BOOK ENTRY PANAL:");

            Book book = new Book();

            Console.WriteLine("Enter the book name : ");
            book.name = Console.ReadLine() ?? "";

            Console.WriteLine("Enter your book publisher : ");
            book.publisher = Console.ReadLine() ?? "";

            book.datePublish = Imp.ReadInt32("Enter the Book Year: ");

            Console.WriteLine("Enter the book genre: ");
            book.genre = Console.ReadLine() ?? "";

            book.Cost = Imp.ReadInt32("Enter the book price:");

            LibarayDatabase.Books.Add(book);

            Book book = new Book( bName, bPub, bYear, bGenre, bCost);
            LibarayDatabase.Books.Add(book);

            LibarayDatabase.TotalBooksCount++; 

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
            PayFineBook();
        }

        public void BorrowBook()
        {
            Console.WriteLine("\nEnter the name of the book to be borrowed: ");
            string target = Console.ReadLine() ?? "";
            
            bool found = false;
            foreach (var book in LibarayDatabase.Books)
            {
                if (book.name.Equals(target, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine(target + " Book available for borrowing.");
                    found = true;
                    break; 
                }
            }

            if (!found)
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
                if (book.name.Equals(target, StringComparison.OrdinalIgnoreCase))
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
            Console.WriteLine("\nPay the fine due: Code active");
            Console.WriteLine("Enter the book name: ");
            string target = Console.ReadLine() ?? ""; 
            Console.WriteLine("Processing fine clearance for: " + target);
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
                Console.WriteLine(" " + book.name + "," + book.genre + " by " + book.publisher + "; Price = " + book.Cost + " PKR ");
            }
        }
    }

    public record Book(string name, string publisher,int datePublish,string Genre,int cost);

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
                else if (chosenRole== SystemRole.User)
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
