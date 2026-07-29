using System;
 
 namespace daySecond
{
     class Program
    {
        public static void Main(string []args)
        {
            string intro = " Hello, This id a C# program that will use multiple scripts. ";
            Console.WriteLine("==========================================================================================");
            
            Console.WriteLine(intro);
            string outro = " Thank you for using this program. ";
            //===========================================================================================================================

            int a = 10;
            Console.WriteLine("Enter your name: ");
            string name  = Console.ReadLine();
            Console.WriteLine("Hello, " + name + "!, Enter your age: ");
            int age = int.Parse(Console.ReadLine());
            if(age < 0 || age > 120)
            {
                Console.WriteLine("Please enter a valid age between 0 and 120");
            }
            Console.WriteLine("Enter the current year:");
            int year = int.Parse(Console.ReadLine());
            if(year < 1900 || year > 2026)
            {
                Console.WriteLine("Please enter a valid year between 1900 and 2026.");
            }
            int birthYear = year - age;
            Console.WriteLine("Hello,"+ name + "!, you are " + age + " years old, and you were born in "+ birthYear + ".");
            Console.WriteLine("in 10 years, you will be " +(age +a)+ " years old at the year " + (year+a) + ".");
             Console.WriteLine("==========================================================================================");

            //===========================================================================================================================

            double pi=3.142;
            Console.WriteLine("Enter the radius of the cirlcle:");
            double radius = double.Parse(Console.ReadLine());
            double area = pi * radius * radius;
            Console.WriteLine("The area of the cirlce with radius "+ radius + " is =" + area); 
             Console.WriteLine("==========================================================================================");
            //===========================================================================================================================

            bool isTrue = true;
            bool isFalse = false;
            Console.WriteLine("The value of isTrue is: " + isTrue);
            Console.WriteLine("The value of isFalse is: " + isFalse);
            Console.WriteLine("The current age  of user " + age + " is greater than 18? " + (age > 18));
             Console.WriteLine("==========================================================================================");

            //===========================================================================================================================

           var myName = "John Doe";
           var myAge = 30;
           Console.WriteLine("My name is " + myName + " and I am " + myAge + " years old."); 
            Console.WriteLine("==========================================================================================");

            //===========================================================================================================================
             
             char Grade = 'A';
             char grade2 = 'B';
             char grade3 = 'C';
             Console.WriteLine("The grades are: " + Grade + ", " + grade2 + ", " + grade3);
              Console.WriteLine("==========================================================================================");

             //===========================================================================================================================
             Console.WriteLine(outro);
              Console.WriteLine("==========================================================================================");
        }

    }
}