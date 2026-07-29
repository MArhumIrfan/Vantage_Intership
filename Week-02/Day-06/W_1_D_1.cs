// dotnet run W_1_D_1.cs
using System;

namespace Lib
{ 
    public class Book
    {   
        
        private int _cost; 

        public int Cost
        {
            get { return _cost; }
            set
            {
                if (value <0)
                {
                    Console.WriteLine("Warning!, Price is incorrect !");
                    _cost = 0;
                }
                else
                {
                    _cost = value;
                }
            } 
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
        
        static void Main(string[] args)
        {
            Book book = new Book();
            gap();
            Console.WriteLine("Enter the book Name:");
            book.name = Console.ReadLine() ?? "";
            
            gap();
            Console.WriteLine("Enter the book publisher:");
            book.publisher = Console.ReadLine() ?? "";
            
            gap();
            Console.WriteLine("Enter the year published:");
           
            book.datePublish = Convert.ToInt32(Console.ReadLine() ?? "0");
            
            gap();
            Console.WriteLine("Enter the book genre:");
            book.genre = Console.ReadLine() ?? "";
            
            gap();
            Console.WriteLine("Enter the Book Price: ");
            
            book.Cost = Convert.ToInt32(Console.ReadLine() ?? "0");

            gap();
            Console.WriteLine($"The Book entered is {book.name} and the publisher of the book is {book.publisher} ");
            Console.WriteLine($"and was published in {book.datePublish} and is a {book.genre} book");
            Console.WriteLine($"The book costs around: {book.Cost}");
        }
    }
}
