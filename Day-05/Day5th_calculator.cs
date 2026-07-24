//dotnet run Day5th_calculator.cs
using System;

namespace ConsoleApp1
{
    public class Class1
    {
        static void Gap()
        {
            Console.WriteLine("__________");
        }

        static double Cal()
        {   
            double result = 0;

            Console.WriteLine("Enter the length of the operation (how many numbers): ");
            int length = Convert.ToInt32(Console.ReadLine());

            double[] input = new double[length];
            string[] sign = new string[length];

            for (int i = 0; i < length; i++)
            {
                Console.WriteLine($"Enter number {i + 1}:");
                input[i] = Convert.ToDouble(Console.ReadLine());

                if (i < length - 1)
                {
                    Console.WriteLine("Enter the operation (+, -, *, /):");
                    // Fix CS8601: Added ?? "" to handle potential null values safely
                    sign[i] = Console.ReadLine() ?? "";
                }
            }

            // Fix CS0029: Set result to the first number in the array, not the array itself
            result = input[0];

            for (int i = 0; i < length - 1; i++)
            {
                if (sign[i] == "+")
                {
                    result += input[i + 1];
                }
                else if (sign[i] == "-")
                {
                    result -= input[i + 1];
                }
                else if (sign[i] == "*")
                {
                    result *= input[i + 1];
                }
                else if (sign[i] == "/")
                {
                    if (input[i + 1] != 0)
                    {
                        result /= input[i + 1];
                    }
                    else
                    {
                        Console.WriteLine("Error: Division by zero.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid operator entered.");
                }
            }

            return result;
        }

        public static void Main(string[] args)
        {   
            Gap();
            Console.WriteLine("Calculator using Arrays!");
            Gap();
            
            double totalAnswer = Cal();
            
            Console.WriteLine($"Total answer is: {totalAnswer}");
        }    
    }
}

//dotnet run Day5th_calculator.cs
using System;

namespace ConsoleApp1
{
    public class Class1
    {
        static void Gap()
        {
            Console.WriteLine("__________");
        }

        static double Cal()
        {   
            double result = 0;

            Console.WriteLine("Enter the length of the operation (how many numbers): ");
            int length = Convert.ToInt32(Console.ReadLine());

            double[] input = new double[length];
            string[] sign = new string[length];

            for (int i = 0; i < length; i++)
            {
                Console.WriteLine($"Enter number {i + 1}:");
                input[i] = Convert.ToDouble(Console.ReadLine());

                if (i < length - 1)
                {
                    Console.WriteLine("Enter the operation (+, -, *, /):");
                    // Fix CS8601: Added ?? "" to handle potential null values safely
                    sign[i] = Console.ReadLine() ?? "";
                }
            }

            // Fix CS0029: Set result to the first number in the array, not the array itself
            result = input[0];

            for (int i = 0; i < length - 1; i++)
            {
                if (sign[i] == "+")
                {
                    result += input[i + 1];
                }
                else if (sign[i] == "-")
                {
                    result -= input[i + 1];
                }
                else if (sign[i] == "*")
                {
                    result *= input[i + 1];
                }
                else if (sign[i] == "/")
                {
                    if (input[i + 1] != 0)
                    {
                        result /= input[i + 1];
                    }
                    else
                    {
                        Console.WriteLine("Error: Division by zero.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid operator entered.");
                }
            }

            return result;
        }

        public static void Main(string[] args)
        {   
            Gap();
            Console.WriteLine("Calculator using Arrays!");
            Gap();
            
            double totalAnswer = Cal();
            
            Console.WriteLine($"Total answer is: {totalAnswer}");
        }    
    }
}
