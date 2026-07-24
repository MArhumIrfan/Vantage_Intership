using System;

namespace ConsoleApp1
{
    public class Class1
    {   
        static void gap()
        {
            Console.WriteLine("_____---*---_____");
        }
        static void Main(string[] args)
        {   
            gap();
            string[] cars = { "suzuki", "toyota", "honda" };
            cars[0] = "suzuki"; // FIXED: Changed 'cars =' to 'cars[0] =' to prevent compilation errors
            Console.WriteLine(cars[0]);
            gap();
            Console.WriteLine(cars.Length);
            gap();
            
            for (int i = 0; i < cars.Length; i++)
            {
                Console.WriteLine(cars[i]);
                gap();
            }

            string txt = "Hello World";
            Console.WriteLine(txt.ToLower());
            gap();
            Console.WriteLine(txt.ToUpper());
            gap();

            Console.WriteLine("Enter your first name : ");
            string fname = Console.ReadLine();

            Console.WriteLine("Enter you second name : ");
            string sname = Console.ReadLine();

            // Modern strings with space formatting
            string name = fname + " " + sname;
            string Name = string.Concat(fname, " ", sname);
            string NAME = $"My full name is : {fname} {sname}"; // FIXED: Clean modern interpolation with a space
            
            Console.WriteLine("your full name is : " + name + " by using simple +");
            gap();
            Console.WriteLine("your full nmae is : " + Name + " by using concat");
            gap();
            Console.WriteLine("your full name is : " + NAME + " By using string interpolation");
            gap();


            string myString = "Hello World";
            gap();
            Console.WriteLine(myString[1]);
            gap();
            Console.WriteLine(myString.IndexOf("o"));
            gap();

            Console.WriteLine("The \" chosen\" are wrong");
            gap();
            Console.WriteLine("i\'ts hard , isnt?");
            gap();
            Console.WriteLine("the deal was for 50\\50, yet it was rejected!");
            gap();

        }
    }
}
