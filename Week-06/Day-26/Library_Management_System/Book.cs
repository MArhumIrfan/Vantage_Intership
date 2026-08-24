using System;

namespace Lib
{
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
            Name = "";
            Publisher = "";
            Genre = "";
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
}
