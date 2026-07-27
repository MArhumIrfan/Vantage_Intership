using System;

namespace ConsoleApp1
{
    public class Class1
    {
        static void gap()
        {
            Console.WriteLine("___---***---___");
        }

        static void User()
        {
            int attempt = 0;
            Random random = new Random();
            int guess = random.Next(1, 101);
            bool correct = false;
            Console.WriteLine("I'm thinking of a number between 1-100");

            while (!correct)
            {
                Console.WriteLine("Enter your guess : ");
                int userGuess = Convert.ToInt32(Console.ReadLine());
                attempt++;

                if (userGuess > guess && userGuess <= 100)
                {
                    Console.WriteLine("Too High!");
                }
                else if (userGuess < guess && userGuess >= 1)
                {
                    Console.WriteLine("Too Low!");
                }
                else if (userGuess == guess)
                {
                    Console.WriteLine("You guessed it!");
                    Console.WriteLine("My Number was " + guess + " !");
                    Console.WriteLine("You guessed it in " + attempt + " attempts !");
                    Console.WriteLine("Thanks for playing!");
                    correct = true;
                }
                else
                {
                    Console.WriteLine("Invalid input! The numbers are between 1-100!");
                    attempt--;
                }
            }
        }

        static void Computer()
        {
            Console.WriteLine("Think of a number between 1-100");
            Console.WriteLine("And I'll try to guess it!");

            int min = 1;
            int max = 100;
            int attempt = 0;
            bool compCorrect = false;

            while (!compCorrect)
            {
                attempt++;
                int compGuess = (min + max) / 2;
                Console.WriteLine("My guess is " + compGuess);
                Console.WriteLine("Am I correct?");
                Console.WriteLine("Enter the correct choice: (H)igh, (L)ow & (C)orrect?");
                string ans = Console.ReadLine().ToLower();

                if (ans == "c")
                {
                    Console.WriteLine("Yay! I got it right!");
                    Console.WriteLine("Your number was " + compGuess + " ! ");
                    Console.WriteLine("It took me " + attempt + " tries to guess your number ! ");
                    compCorrect = true;
                }
                else if (ans == "h")
                {
                    max = compGuess - 1;
                }
                else if (ans == "l")
                {
                    min = compGuess + 1;
                }
                else
                {
                    Console.WriteLine("Incorrect input! Please choose one.");
                    attempt--;
                }

                if (min > max)
                {
                    Console.WriteLine("Answer has changed! Don't cheat!");
                    break;
                }
            }
        }

        public static void Main(string[] args)
        {
            gap();
            Console.WriteLine("Number Guessing Game!");
            Console.WriteLine("One person has to think of a number between 1-100 and the other person has to guess it!");
            Console.WriteLine("Who do you want to guess the number? The user or the Computer?");
            string choice = Console.ReadLine().ToLower();

            switch (choice)
            {
                case "user":
                    gap();
                    User();
                    gap();
                    break;

                case "computer":
                    gap();
                    Computer();
                    gap();
                    break;

                default:
                    Console.WriteLine("Invalid Input, ending the program");
                    gap();
                    break;
            }
        }
    }
}
