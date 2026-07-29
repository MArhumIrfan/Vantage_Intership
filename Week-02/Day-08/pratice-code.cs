using System;

namespace Pratice
{
    
    public class Vehicle
    {
        private string brand ="Ford";
        public string Brand
        {
            get { return brand; }
            set { brand = value; }
        }

        public void Horn()
        {
            Console.WriteLine("Beep!");
        }

    }

    class Car: Vehicle
    {
        
        public string modelName = "Mustang BOSS 429";
        private int modelYear = 1969;
        public int ModelYear
        {
            get{return modelYear;}
            set{modelYear = value;}
        }

    }


    public static class Program
    {
        public static void Main(string[] args)
        {
           Car myCar = new Car();

            myCar.Horn();
            myCar.Horn();
            Console.WriteLine("The car is : "+myCar.Brand+" and the model is : "+myCar.modelName+" which was produced in : "+myCar.ModelYear);

            
        }
    }
}