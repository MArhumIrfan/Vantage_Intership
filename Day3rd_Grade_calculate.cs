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

            Console.WriteLine("Enter your grades to verify and calculate it.");
            Console.WriteLine("Enter the number of subjects taken:");
            int loop = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine(gap);
            
            if (loop < 1 || loop > 10)
            {
                Console.WriteLine("Invalid number of subjects, please input a correct subject count.");
                Console.WriteLine(gap);
                return;
            }

            int sum = 0;
            for (int i = 0; i < loop; i++)
            {
                Console.WriteLine("Enter grade for subject " + (i + 1) + " (0-100):");
                int grade = Convert.ToInt32(Console.ReadLine());

                
                if (grade < 0 || grade > 100)
                {
                    Console.WriteLine("Invalid grade! Please enter a value between 0 and 100.");
                    i--; 
                    continue;
                }

                
                if (grade >= 90)
                {
                    Console.WriteLine(" Grade obtained : A+ ");
                    Console.WriteLine(gap);
                }
                else if (grade >= 80) 
                {
                    Console.WriteLine(" Grade Obtained : A ");
                    Console.WriteLine(gap);
                }
                else if (grade >= 70) 
                {
                    Console.WriteLine(" Grade Obtained : B+ ");
                    Console.WriteLine(gap);
                }
                else if (grade >= 60) 
                {
                    Console.WriteLine(" Grade Obtained : B ");
                    Console.WriteLine(gap);
                }
                else if (grade >= 50) 
                {
                    Console.WriteLine(" Grade Obtained : C ");
                    Console.WriteLine(gap);
                }
                else
                {
                    Console.WriteLine(" Grade Obtained : Fail ");
                    Console.WriteLine(gap);
                }

                sum += grade;
            }

            double totalPossiblePoints = 100.0 * loop; 
            double percentage = (sum / totalPossiblePoints) * 100;
            
            Console.WriteLine("\n" + gap);
            Console.WriteLine("Total grades sum: " + sum);
            Console.WriteLine("Average grade: " + ((double)sum / loop));
            Console.WriteLine("The total percentage is: " + percentage + "%");
            Console.WriteLine(gap);
        }
    }
}
