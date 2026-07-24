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

        static void Unit()
        {
        
            string[] units = { "Length", "Weight", "Currency" };
            
            double[] baseUnitDistance = { 1.0, 1000.0, 1609.34 }; 
            double[] baseUnitWeight = { 1.0, 1000.0, 28.3495 };   
            double[] baseUnitCurrency = { 1.0, 278.50, 360.20 };  

            for (int i = 0; i < units.Length; i++)
            {
                Console.WriteLine($"{i + 1}) {units[i]}");
                Gap();
            }

            Console.WriteLine("Enter your choice: ");
            string ch = Console.ReadLine().ToLower();

            string[] subUnits;
            double[] conversionRates;

            switch (ch)
            {
                case "length":
                    Console.WriteLine("Length converter selected.");
                    subUnits = new string[] { "meter", "kilometer", "miles" };
                    conversionRates = baseUnitDistance;
                    break;

                case "weight":
                    Console.WriteLine("Weight converter selected.");
                    subUnits = new string[] { "gram", "kilogram", "ounce" };
                    conversionRates = baseUnitWeight;
                    break;

                case "currency":
                    Console.WriteLine("Currency converter selected.");
                    subUnits = new string[] { "pkr", "usd", "pounds" };
                    conversionRates = baseUnitCurrency;
                    break;

                default:
                    Console.WriteLine("Invalid unit category selected.");
                    return; 
            }

            Gap();
            Console.WriteLine($"Available sub-units: {string.Join(", ", subUnits)}");
            Gap();

            Console.Write("Convert FROM: ");
            string fromUnit = Console.ReadLine().ToLower();

            Console.Write("Convert TO: ");
            string toUnit = Console.ReadLine().ToLower();

            Console.Write("Enter the value: ");
            double value = Convert.ToDouble(Console.ReadLine());

            int fromIndex = Array.IndexOf(subUnits, fromUnit);
            int toIndex = Array.IndexOf(subUnits, toUnit);

            if (fromIndex == -1 || toIndex == -1)
            {
                Console.WriteLine("Error: Invalid units entered.");
                return;
            }

            
            double valueInBase = value * conversionRates[fromIndex];
            double finalResult = valueInBase / conversionRates[toIndex];

            Gap();
            Console.WriteLine($"Result: {value} {fromUnit.ToUpper()} = {finalResult:F2} {toUnit.ToUpper()}");
            Gap();
        }

        static double Cal()
        {   
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
                    sign[i] = Console.ReadLine() ?? "";
                    Gap();
                }
            }

            double result = input[0];

            for (int i = 0; i < length - 1; i++)
            {
                if (sign[i] == "+") result += input[i + 1];
                else if (sign[i] == "-") result -= input[i + 1];
                else if (sign[i] == "*") result *= input[i + 1];
                else if (sign[i] == "/")
                {
                    if (input[i + 1] != 0) result /= input[i + 1];
                    else Console.WriteLine("Error: Division by zero.");
                    Gap();
                }
                else Console.WriteLine("Invalid operator entered.");
                Gap();
            }

            return result;
        }

        public static void Main(string[] args)
        {   
            Gap();
            Console.WriteLine("Day-05 hands on task");
            Console.WriteLine("Unit Converter & calculator");
            Console.WriteLine("Using Methods, String, Arrays");
            Gap();

            Console.WriteLine("Enter your choice");
            Console.WriteLine("Unit for Unit Converter");
            Console.WriteLine("Calc for Calculator");
            Gap();

            string choice = Console.ReadLine().ToLower();

            if (choice == "calc")
            {
                Gap();
                Console.WriteLine("A Calculator ");
                Gap();

                double totalAnswer = Cal();
                Console.WriteLine($"Total answer is: {totalAnswer}");
                Gap();
            }
            else if (choice == "unit")
            {
                Gap();
                Console.WriteLine("A Unit Converter ");
                Gap();
                Unit();
            }
            else
            {
                Console.WriteLine("Invalid choice entered.");
                Gap();
            }
        }    
    }
}

