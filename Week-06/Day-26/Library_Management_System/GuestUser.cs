using System;

namespace Lib
{
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
}
