using System;
using System.Linq;
using System.Threading;

namespace Lib
{
    public class VerifyUser : Login, IBorrower
    {
        public string? Username { get; set; }

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
            int borrowedCount = LibraryDatabase.Catalog.Values
                .Count(book => book.IsBorrowed && book.BorrowedBy == Username);

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
            foundBook.BorrowedBy = Username ?? string.Empty;
            foundBook.BorrowedDate = DateTime.Now;
            foundBook.DueDate = DateTime.Now.AddDays(14);

            LibraryDatabase.SaveToFile();

            LibraryDatabase.LogTransaction("BORROW", foundBook.BookID, foundBook.Name, Username ?? string.Empty);

            UI.PrintSuccess(foundBook.Name + " borrowed successfully! Price = " + foundBook.Cost + " PKR");
            UI.PrintWarning("IMPORTANT: Your due date is " + foundBook.DueDate.ToString("yyyy-MM-dd") + ".");
        }

        public void ReturnBook()
        {
            LibraryDatabase.DisplayAvailableTitles();

            int targetID = UI.ReadInt32("Enter the Book ID to return");

            if (!LibraryDatabase.Catalog.ContainsKey(targetID))
            {
                
                UI.PrintError("Error!, The book ID : ["+targetID+"] not found in the catalog");
                return;

            }

            Book foundBook = LibraryDatabase.Catalog[targetID];

            if(!foundBook.IsBorrowed)
            {
                UI.PrintError("Error ! the book is not marked as borrowed!");
                return;
            }

            if (DateTime.Now > foundBook.DueDate)
            {

                int DaysLate = (DateTime.Now - foundBook.DueDate).Days;
                if(DaysLate<=0) DaysLate =1;

                int fineMultiplier = DaysLate switch
                {
                    
                    >= 1 and <= 5 => 50,
                    >5 and <=15 => 75,
                    _=>100

                };

                int fineAmount = DaysLate * fineMultiplier;
                foundBook.FineDue += fineAmount;

                UI.PrintWarning("Warning !: The book is "+DaysLate+" days late.");
                Console.WriteLine("The total amount of fine is "+fineAmount+" PKR ");

            }

            foundBook.IsBorrowed = false;
            foundBook.BorrowedBy = " ";
            foundBook.BorrowedDate = DateTime.MinValue; 
            foundBook.DueDate = DateTime.MinValue;      
            LibraryDatabase.SaveToFile();

            LibraryDatabase.LogTransaction("RETURN", foundBook.BookID, foundBook.Name, Username ?? string.Empty);

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
}
