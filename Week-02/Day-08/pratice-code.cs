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
        public string ModelYear
        {
            get{return modelYear;}
            set{modelYear = date;}
        }

    }


    public static class Program
    {
        public static void Main(string[] args)
        {
           vehicle Vehicle = new vehicle();

            vehicle.honk();
            vehicle.honk();
            Console.WriteLine("The car is : "+Vehicle.Brand()+" and the model is :"+Vehicle.modelName()+" which was produced in :"+Vehicle.modelYear());

            
        }
    }
}