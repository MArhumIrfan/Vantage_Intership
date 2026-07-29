using System;

namespace ConsoleApp1
{
    public class Class1
    {
        static void gap()
        {
            Console.WriteLine("__--__--__**|**__--__--__");
        }

        static void TotalGrades()
        {
            int sum = 0;
            Console.WriteLine("Enter the subjects taken:");
            int subject = Convert.ToInt32(Console.ReadLine());
            
            if (subject < 1 || subject > 10)
            {
                Console.WriteLine("Invalid amount of grades");
                gap();
                return;
            }

            for (int i = 0; i < subject; i++)
            {
                Console.WriteLine("Enter the grade for subject " + (i + 1) + " between (1-100):");
                int grade = Convert.ToInt32(Console.ReadLine());

                if (grade < 1 || grade > 100)
                {
                    Console.WriteLine("Invalid grade! Please re-enter this subject's grade.");
                    gap();
                    i--; // 🌟 Decrement 'i' so the user can re-try this specific subject entry!
                    continue;
                }

                if (grade >= 90)
                {
                    Console.WriteLine("Grade : A+");
                }
                else if (grade >= 80)
                {
                    Console.WriteLine("Grade : A");
                }
                else if (grade >= 70)
                {
                    Console.WriteLine("Grade : B+");
                }
                else if (grade >= 60)
                {
                    Console.WriteLine("Grade : B");
                }
                else if (grade >= 50)
                {
                    Console.WriteLine("Grade : C");
                }
                else
                {
                    Console.WriteLine("Grade : Fail");
                }

                gap();
                sum += grade;
            }

            Percentage(sum, subject);
        }

        static void Percentage(int sum, int subject)
        {
            double totalPoints = 100.0 * subject;
            double percentage = (sum / totalPoints) * 100.0;
            Console.WriteLine("Total percentage: " + percentage + "%");
            
            FinalGrade(percentage); 
        }

        static void FinalGrade(double percentage) 
        {
            if (percentage >= 90) 
            {
                Console.WriteLine("Final Grade : A*");
                gap();
            }
            else if (percentage >= 80)
            {
                Console.WriteLine("Final Grade : A");
                gap();
            }
            else if (percentage >= 70)
            {
                Console.WriteLine("Final Grade : B");
                gap();
            }
            else if (percentage >= 60) 
            {
                Console.WriteLine("Final Grade : C");
                gap();
            }
            else if (percentage >= 50) 
            {
                Console.WriteLine("Final Grade : D");
                gap();
            }
            else
            {
                Console.WriteLine("Final Grade : F");
                gap();
            }
        }

        public static void Main(string[] args)
        {
            gap();
            Console.WriteLine("Grade Calculator");
            gap();

            TotalGrades();
        }
    }
}
