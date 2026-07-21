using System;

class Program
{
    static void Main(string[] args)
    {
           
        int a = 10;
        int b = 5;
        int sum = a + b;
        Console.WriteLine("the sum of " + a + " and " + b + " is = " + sum);

        //=========================================================

        int max = int.MaxValue;
        int min = int.MinValue;
        Console.WriteLine("Maximum value of int: " + max);
        Console.WriteLine("Minimum value of int: " + min);
        //=========================================================
        int what = max + 3;
        Console.WriteLine("Maximum value of int + 3: " + what);

        double aD = 18;
        double bD = 6;
        double cD = 5;
        double result = (aD + bD) / cD;
        Console.WriteLine("Result of (aD + bD) / cD = " + result);
    }
}