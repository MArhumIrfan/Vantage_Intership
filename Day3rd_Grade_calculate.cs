using System;

namespace ConsoleApp1
{
    public class Class1
    {
        public static void Main(string[] args)
        {
            string gap = "----------**********-----------";
            Console.WriteLine(gap);
            Console.WriteLine("Grade Calculator");
            Console.WriteLine(gap);

            Console.WriteLine("enter your grades to verify and calculate it.");
            Console.WriteLine("Enter the number of grades given");
            int loop = Convert.ToInt32(Console.ReadLine());
            if (loop < 1 || loop > 10)
            {
                Console.WriteLine("Invalid number of subjects, please input a correct subject count.");
                return;
            }

            int sum = 0;
            for (int i = 0; i < loop; i++)
            {
                Console.WriteLine("Enter grade :" + (i + 1));
                int grade = Convert.ToInt32(Console.ReadLine());

                if (grade < 90 && grade > 99)
                {
                    Console.WriteLine(" Grade obtained : A+ ");
                }
                else if(grade<80 && grade>89)
                {
                    Console.WriteLine(" Grade Obtained : A ");
                }
                else if(grade<70 && grade>79)
                {
                    Console.WriteLine(" Grade Obainted : B+ ");
                }
                else if(grade<60 && grade>69)
                {
                    Console.WriteLine(" Grade Obtained : B ");
                }
                else if(grade<50 && grade>59)
                {
                    Console.WriteLine(" Grade Obtained : C ");
                }
                else
                Console.WriteLine("Fail");
                
                sum += grade;
            }

            Console.WriteLine("Total grades sum: " + sum);
            Console.WriteLine("Average grade: " + ((double)sum / loop));
        }
    }
}

