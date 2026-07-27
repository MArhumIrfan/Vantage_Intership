using System;

namespace ConsoleApp1
{
    public class Class1
    {
        public static void Main(string[] args)
        {
            string gap = "----------*****----------";

            Console.WriteLine(gap);
            Console.WriteLine("Number Guessing Game!");
            Console.WriteLine(gap);

            int attempt = 0;
            Console.WriteLine("Who should guess the number from 1-100, user or computer?");
            string choice = Console.ReadLine().ToLower();

            Random random = new Random();

            switch (choice)
            {
                //==========================================================================
                case "user":
                    int guess = random.Next(1, 101);
                    bool correct = false;
                    Console.WriteLine("I'm thinking of a number between 1-100");
                    do
                    {
                        
                        Console.WriteLine("Enter your answer: ");
                        int userGuess = Convert.ToInt32(Console.ReadLine());

                        if (userGuess > guess && userGuess <= 100)
                        {
                            Console.WriteLine("Too High!");
                            attempt++;
                        }
                        else if (userGuess < guess && userGuess >= 1)
                        {
                            Console.WriteLine("Too Low!");
                            attempt++;
                        }
                        else if (userGuess == guess)
                        {
                            Console.WriteLine("You're right! My number was: " + guess);
                            Console.WriteLine("You guessed the number in " + attempt + " attempts!");
                            Console.WriteLine(gap);
                            correct = true;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input");
                        }
                    }
                    while (!correct);
                    break;
                //===============================================================================
                case "computer":
                    Console.WriteLine("Think of a number between 1-100");
                    int min = 1;
                    int max = 100;
                    bool computerCorrect = false;

                    do
                    {
                        attempt++;
                        int computerGuess = (min + max) / 2;
                        Console.WriteLine("My guess is: " + computerGuess);
                        Console.WriteLine("Enter correct choice, (H) for high, (L) for low, (C) for correct:");
                        string ans = Console.ReadLine().ToLower();

                        if (ans == "c")
                        {
                            Console.WriteLine("Yay! I got it right! The number was: " + computerGuess);
                            Console.WriteLine("It took me " + attempt + " attempts to get it right");
                            Console.WriteLine(gap);
                            computerCorrect = true;
                        }
                        else if (ans == "h")
                        {
                            max = computerGuess - 1;
                        }
                        else if (ans == "l")
                        {
                            min = computerGuess + 1;
                        }
                        else
                        {
                            Console.WriteLine("Incorrect input, choose the correct one.");
                            attempt--;
                        }

                        if (min > max)
                        {
                            Console.WriteLine("Answer has changed, please don't cheat");
                            break;
                        }
                    }
                    while (!computerCorrect);
                    break;

                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }//=============================================================================
        }
    }
}
