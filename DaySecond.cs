using System;
using System.Xml;

namespace daySecond
{
     class Program
    {
        public static void Main(string []args)
        {
             string buffer = ("==========================================================================================");

            string intro = " Hello, This id a C# program that will use multiple scripts. ";
            Console.WriteLine(buffer);
            

            Console.WriteLine(intro);
            string outro = " Thank you for using this program. ";
            //===========================================================================================================================

            int a = 10;
            Console.WriteLine("Enter your first name: ");
            string firstName  = Console.ReadLine();
            Console.WriteLine("Enter your last name: ");
            string lastName = Console.ReadLine();
            string name = firstName + " " + lastName;
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
             Console.WriteLine(buffer);

            //===========================================================================================================================

            double pi=3.142;
            Console.WriteLine("Enter the radius of the cirlcle:");
            double radius = double.Parse(Console.ReadLine());
            double area = pi * radius * radius;
            Console.WriteLine("The area of the cirlce with radius "+ radius + " is =" + area); 
             Console.WriteLine(buffer);
            //===========================================================================================================================

            double cashTax = 0.15;
            double cardTax = 0.05;
            Console.WriteLine("Enter the amount of  payment:");
            double Payment = double.Parse(Console.ReadLine());
            Console.WriteLine("Enter the Payment Method (cash or card):");
            String PaymentMethod = Console.ReadLine();
            if(PaymentMethod.ToLower() == "cash")
            {
                Payment = Payment - (Payment * cashTax);
            }
            else if(PaymentMethod.ToLower() == "card")
            {
                Payment = Payment + (Payment * cardTax);
            }
            else
            {
                Console.WriteLine("Invalid payment method. Please enter either 'cash' or 'card'.");
            }
            Console.WriteLine("The final amount after tax is: " + Payment);
             Console.WriteLine(buffer);   
            //===========================================================================================================================
            bool isTrue = true;
            bool isFalse = false;
            Console.WriteLine("The value of isTrue is: " + isTrue);
            Console.WriteLine("The value of isFalse is: " + isFalse);
            Console.WriteLine("The current age  of user " + age + " is greater than 18? " + (age > 18));
             Console.WriteLine(buffer);
            //===========================================================================================================================

            Console.WriteLine("Is the snow white? ");
            string ans1 = Console.ReadLine();
            if (ans1.ToLower() == "yes" || ans1.ToLower() == "y")
            {
                Console.WriteLine("The Answer is " + isTrue);
            }
            else
            {
                Console.WriteLine("The answer is " + isFalse);
            }

            Console.WriteLine(buffer);
            //===========================================================================================================================

           var myName = "John Doe";
           var myAge = 30;
           Console.WriteLine("My name is " + myName + " and I am " + myAge + " years old."); 
            Console.WriteLine(buffer);
            //===========================================================================================================================

            Console.WriteLine("Enter any text you want to output: ");

            var ques1 = Console.ReadLine();

            Console.WriteLine("The text you have typed is : "+ ques1);

            Console.WriteLine("Add another text to type you want:");

            var ques2 = Console.ReadLine();

            Console.WriteLine("The second line of text you typed is : " + ques2);

            Console.WriteLine(buffer);
            //===========================================================================================================================
             
             char Grade = 'A';
             char grade2 = 'B';
             char grade3 = 'C';
             Console.WriteLine("The grades are: " + Grade + ", " + grade2 + ", " + grade3);
             Console.WriteLine(buffer);
             //===========================================================================================================================


                char alpha = 'a';
                char beta = 'b';
                char charlie = 'c';
                char delta = 'd';

                Console.WriteLine("the secret layer is " + alpha+" , "+beta+" , "+charlie+" , " + delta);

                Console.WriteLine("Enter the Secret Layer you want to output: ");

                var secretLayer = Console.ReadLine();

                if(secretLayer.ToLower() == "a c c b d a c d a")
                {
                    Console.WriteLine("The secret layer is correct!");
                }
                else
                {
                    Console.WriteLine("The secret layer is incorrect!");
                }

            

            Console.WriteLine(buffer);    
             //===========================================================================================================================
             Console.WriteLine(outro);
              Console.WriteLine(buffer);
        }

    }
}