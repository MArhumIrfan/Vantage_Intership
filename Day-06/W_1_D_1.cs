using System;


namespace Lib{ 
public  class Book
{
    public string name;

    public string publisher;

    public int datePublish;

    public string genre;


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
        book.name=Console.ReadLine();
        gap();
        Console.WriteLine("Enter the book publisher:");
        book.publisher=Console.ReadLine();
        gap();
        Console.WriteLine("Enter the year published:");
        book.datePublish = Convert.ToInt32(Console.ReadLine());
        gap();
        Console.WriteLine("Enter the book genre");
        book.genre = Console.ReadLine();
        gap();

        Console.WriteLine($"The Book entered is {book.name} and is the publisher of the book is {book.publisher} and was published at {book.datePublish} and is a {book.genre} book");

    }

}

}