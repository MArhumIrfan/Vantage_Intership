using System;

namespace Lib
{ 
    public class Verify
    {
        private int age;

        public int Age
        {
            get { return age; }
            set
            {
                if (value < 18)
                {   
                  
                    Console.WriteLine("you are Under 18!");
                   
                    Environment.Exit(0); 
                }
                else if (value > 110)
                {   
                   Console.WriteLine("To Old !");
                                            Environment.Exit(0); 
                }
                else if (value >= 18 && value <= 110)
                {
                  
                    Console.WriteLine("Access Allowed");
                                 }

                age = value;
            }
        }
    }

    public class Book
    {   
        private int _cost; 

        public int Cost
        {
            get { return _cost; }
            set
            {
                if (value < 0)
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

        public static int ReadInt32(string prompt)
        {
            Console.Write(prompt);
            string input = Console.ReadLine() ?? "";
         
            if (int.TryParse(input, out int result))
            {
                return result;
            }
            
            Console.WriteLine("Invalid entry! Defaulting to 0.");
            return 0;
        }

        static void Main(string[] args)
        {   
            Verify ageChecker = new Verify();
            Book book = new Book();
            
            gap();
            ageChecker.Age = ReadInt32("Enter your age: ");
            gap();
            
            gap();
            Console.WriteLine("Enter the book Name:");
            book.name = Console.ReadLine() ?? "";
            
            gap();
            Console.WriteLine("Enter the book publisher:");
            book.publisher = Console.ReadLine() ?? "";
            
            gap();
            book.datePublish = ReadInt32("Enter the year published: ");
            
            gap();
            Console.WriteLine("Enter the book genre:");
            book.genre = Console.ReadLine() ?? "";
            
            gap();
            book.Cost = ReadInt32("Enter the Book Price: ");

            gap();
            Console.WriteLine($"The Book entered is {book.name} and the publisher of the book is {book.publisher} ");
            Console.WriteLine($"and was published in {book.datePublish} and is a {book.genre} book");
            Console.WriteLine($"The book costs around: {book.Cost}");
        }
    }
}
