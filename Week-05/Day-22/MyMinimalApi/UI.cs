/*
using System;
using System.Collections.Generic;

namespace Lib
{
    public class UI
    {
        public static Dictionary<int, string> RegisteredUsers = new Dictionary<int, string>();

        public static void ClearAndHeader(string title)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;

            if (title == "Library Management System")
            {
                Console.WriteLine(@"=================================================");
                Console.WriteLine(@"  _      _ _                          ");
                Console.WriteLine(@" | |    (_) |                         ");
                Console.WriteLine(@" | |     _| |__  _ __ __ _ _ __ _   _ ");
                Console.WriteLine(@" | |    | | '_ \| '__/ _` | '__| | | |");
                Console.WriteLine(@" | |____| | |_) | | | (_| | |  | |_| |");
                Console.WriteLine(@" |______|_|_.__/|_|  \__,_|_|   \__, |");
                Console.WriteLine(@"                                 __/ |");
                Console.WriteLine(@"                                |___/ ");
                Console.WriteLine("==================================================");
            }
            else
            {
                Console.WriteLine("==================================================");
                int spaces = (50 - title.Length) / 2;
                string padding = new string(' ', spaces > 0 ? spaces : 0);
                Console.WriteLine(padding + title.ToUpper());
                Console.WriteLine("==================================================\n");
            }

            Console.ResetColor();
        }

        // --- Random Quote Generator ---
        public static void DisplayRandomQuote()
        {
            List<string> quotes = new List<string>
            {
                "\"The more that you read, the more things you will know. The more that you learn, the more places you'll go.\" – Dr. Seuss",
                "\"A room without books is like a body without a soul.\" – Marcus Tullius Cicero",
                "\"Live as if you were to die tomorrow. Learn as if you were to live forever.\" – Mahatma Gandhi",
                "\"An investment in knowledge pays the best interest.\" – Benjamin Franklin",
                "\"Books are a uniquely portable magic.\" – Stephen King",
                "\"Intellect without ambition is a bird without wings.\" – Salvador Dalí",
                "\"Education is the passport to the future, for tomorrow belongs to those who prepare for it today.\" – Malcolm X",
                "\"Reading is to the mind what exercise is to the body.\" – Joseph Addison",
                "\"Whoever takes a path in search of knowledge, Allah will make easy for him a path to Paradise.\"-Prophet Muhammad (S.A.W)",
                "\"Ask Allah for knowledge that benefits and take refuge from knowledge that does not benefit.\"-Prophet Muhammad (S.A.W)",
                "\"Knowledge is better than wealth. Knowledge guards you, while you have to guard wealth.\"-Hazrat Ali (R.A)",
                "\"The mind of a wise man is his fortress, and the tongue of a fool is his key.\"-Hazarat Ali (R.A)",
                "\"Knowledge is the life of the mind.\"-Hazart Abu Bakr"
            };

            Random rand = new Random();
            int index = rand.Next(quotes.Count);

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Quote of the Moment:");
            Console.WriteLine(quotes[index]);
            Console.ResetColor();
            Console.WriteLine();
        }

        public static int ReadInt32(string prompt)
        {
            Console.Write(prompt);
            string input = Console.ReadLine() ?? "";
            int result;

            if (int.TryParse(input, out result)) return result;

            PrintError("Invalid entry! Defaulting to 0.");
            return 0;
        }

        public static void PrintSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n[SUCCESS] " + message);
            Console.ResetColor();
        }

        public static void PrintError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n[ERROR] " + message);
            Console.ResetColor();
        }

        public static void PrintWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n[WARNING] " + message);
            Console.ResetColor();
        }

        public static void Pause()
        {
            Console.WriteLine("\nPress ENTER to continue...");
            Console.ReadLine();
        }

        public static int GetSafeIntegerInput(string prompt)
        {
            while (true)
            {
                Console.WriteLine(prompt);
                string input = Console.ReadLine() ?? "";
                try
                {
                    return int.Parse(input);
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error : Invalid Input. Please enter a valid numerical menu option ");
                    Console.ResetColor();
                }
            }
        }
    }
}
*/