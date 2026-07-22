 using System;

 namespace ConsoleApp1
{
    public class Class1
    {
        public static void Main(string[] args)
        {
            String intro = "Hello, welcome to the program!, this program will use diffrent control flow statements to demonstrate how they work.";
            String gap = "\n _________________***________________\n";
            String end = "\n Thank you for using the program, have a nice day!";
            Console.WriteLine(gap);
            Console.WriteLine(intro);
            Console.WriteLine(gap);
            //========================================================================================================
            // If Statement
            Console.WriteLine("If Statement");
            Console.WriteLine("Please enter first lenght of a four sided shape: ");
            int lenght1 =Convert.ToInt32((Console.ReadLine()));
            Console.WriteLine("Please enter second lenght of a four sided shape:");
            int lenght2 = Convert.ToInt32((Console.ReadLine()));
            if(lenght1 == lenght2)
            {
                Console.WriteLine("the shape is a square");
            }
            Console.WriteLine(gap);
            //========================================================================================================
            //if else Statement
            Console.WriteLine("If else Statement");
            Console.WriteLine("Please enter first lenght of a four sided shape:");
            int lenght3 = Convert.ToInt32((Console.ReadLine()));
            Console.WriteLine("Please enter second lenght of a four sided shape:");
            int lenght4 = Convert.ToInt32((Console.ReadLine()));
            if(lenght3 == lenght4)
            {
                Console.WriteLine("the shape is a square");
            }
            else
            {
                Console.WriteLine("the shape is a rectangle");
            }
            Console.WriteLine(gap);
            //========================================================================================================
            //if elseif else Statement
            Console.WriteLine("If elseif else Statement");
            Console.WriteLine("Please enter first lenght of a four sided shape:");
            int lenght5 = Convert.ToInt32((Console.ReadLine()));
            Console.WriteLine("PLease enter second lenght of a four sided shape:");
            int lenght6 = Convert.ToInt32((Console.ReadLine()));
            Console.WriteLine("Please enter third lenght of a four sided shape:");
            int lenght7 = Convert.ToInt32((Console.ReadLine()));
            Console.WriteLine("Please enter fourth lenght of a four sided shape:");
            int lenght8 = Convert.ToInt32((COnsole.Readline()));
            if(lenght5 == lenght6 && lenght5 == lenght7 && lenght5 == lenght8)
            {
                Console.WriteLine("the shape is a square.");
            }
            else if(lenght5 == lenght7 && lenght6 == lenght8)
            {
                Console.WriteLine("The shape is rectangle.");
            }
            else
            Console.WriteLine("The shap is a irregular four sided polygon.");

            Console.WriteLine(gap);
            //======================================================================================
            //switch statement

            Console.WriteLine(gap);

            Console.WriteLine(" This code will use Switch statemnet");

            Console.WriteLine("there are four diections areound you, pick a direction using 1-4");
            var direction = Console.ReadLine();

            switch (direction)
            {
                case 1:
                Console.WriteLine("You are heading south");

                case 2:
                Console.WriteLine("You are heading west.");

                case 3:
                Console.WriteLine("You are heading north.");

                case 4:
                Console.WriteLine("You are heading north.");

            }

            Console.WriteLine(gap);
            //===================================================================
            // for loop
            Console.WriteLine("This code will use for loop");
            Console.WriteLine(gap);

            int i = 0;
            Console.WriteLine("The Value of i is : "+ i);
            for(i =0; i<10;i++ )
            {
                Console.WriteLine(i);

            }
            Console.WriteLine(gap);
            //====================================================================== 
            //  while loop
            Console.WriteLine("This program will use While loop");
            Console.WriteLine(gap);




        }
    }
}

    