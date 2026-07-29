using System;

class Program
{
    static void Main()
    {
        string aFriend = "\aSaad";
        Console.WriteLine("Hello " + aFriend + "!");
        string firstFriend = "Ghayyur";
        Console.WriteLine("Hello " + firstFriend + "!");
        string secondFriend = "Wazir";
        Console.WriteLine("Hello " + secondFriend + "!");
        Console.WriteLine("Hello " + aFriend + ", " + firstFriend + ", and " + secondFriend + "!");

        //=========================================================
        string message = "                         Hello World!                        ";
        Console.WriteLine(message);

        Console.WriteLine(message.TrimStart());
        Console.WriteLine(message.TrimEnd());
        Console.WriteLine(message.Trim());
        //=========================================================

        string  sayHello = "Hello World!";
        Console.WriteLine(sayHello);
        sayHello =  sayHello.Replace("Hello", "Greetings");
        Console.WriteLine(sayHello);
        //=========================================================

        string myString = "Hello World!";
        Console.WriteLine(myString);
        myString = myString.ToUpper();
        Console.WriteLine(myString);
        myString = myString.ToLower();
        Console.WriteLine(myString);
        
        //=========================================================

        Console.WriteLine("What is your name?");
        Console.Write("Type your name: ");
        String username = Console.ReadLine();
        Console.WriteLine("Hello " + username + ", nice to meet you!");

    

        //=========================================================
        Console.WriteLine("How old are you?");
        Console.Write("Type your age: ");
        string userAge = Console.ReadLine();
        
        // FIX: Declare the integer variable first for older C# versions
        int age; 
        if (int.TryParse(userAge, out age))
        {
            Console.WriteLine("You are " + age + " years old.");
            Console.WriteLine("You are going to be " + (age + 1) + " next year.");
        }
        else
        {
            Console.WriteLine("\aInvalid input. Please enter a valid age.");
        }
        //=========================================================




    }
}