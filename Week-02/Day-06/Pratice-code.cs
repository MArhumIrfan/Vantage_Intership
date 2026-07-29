using System;


namespace CLCode
{

       
    class car
    {
        public string color = "blue";
        public int speed = 150;

        public string model = "Jaguar E-type";
        public void Drive()
        {
            Console.WriteLine("Im going very fast!");
        }

    }


    class Imp
    {
        static void Main(string[] args)
        {
            


            car myObj1 = new car();
            car myObj2 = new car();

            Console.WriteLine(myObj1.color);
            Console.WriteLine(myObj1.speed);
            Console.WriteLine(myObj2.color);
            Console.WriteLine(myObj2.speed);
            Console.WriteLine("there is a "+myObj1.color+" car with make "+myObj1.model+" that is going "+myObj1.speed+" Km/h and the driver is saying: ");
            myObj1.Drive();

        }
    }

}