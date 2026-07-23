using System;


namespace Day4th
{
class Methods
{   

    static void Buffer()
    {
    Console.WriteLine("_____-----*****-----_____");
    }
    
    static void callFunction()
    {
        Console.WriteLine("Hello World!");
        Buffer();
    }
    
    static void nameFunction(string name)
    {
        Console.WriteLine(name+" is amazing ! ");
        Console.WriteLine(name +" is a part of parameter being to used in a method in Csharp. ");
        Buffer();
    }

    static void Method(string fname,int age)
        {
            Buffer();
            Console.WriteLine(fname+" is "+ age+" years old. ");
            Buffer();
        }

    static void defualtMethod(string  text = " If you are seeing this text, no parameter were passed.")
    {
        Buffer();
        Console.WriteLine(" Hello ! "+text);
        Buffer();


    }    

    static void sumMethod(int num1,int num2)
    {
        Buffer();
        Console.Beep();
        Console.WriteLine("The sum of "+num1+" and "+num2+" = "+(num1+num2));
        Buffer();
    }

    static void MMethod(int num)
    {
        
        Console.WriteLine("The result of the number " + num + " is " + (num * 5));

        Buffer();
    }

    static void Mymethod(string nam1,string nam2,string nam3)
    {

        Console.WriteLine("im currently studying at : "+nam3);
        Buffer();

    }

    static int PlusIntMethod(int x, int y)
    {
        Buffer();
        Console.WriteLine("This is the method overloading");
        Console.WriteLine("this method will ads to int number and give you and output");
        Console.WriteLine("THe sum of two numbers "+ x +" and "+ y +" = "+(x+y));
        return(x+y);
        Buffer();
    }


    static double PlusDoubleMethod(Double x, Double y)
    {
        Buffer();
        Console.WriteLine("This is the method overloading");
        Console.WriteLine("this method will ads to double number and give you and output");
        Console.WriteLine("THe sum of two numbers "+ x +" and "+ y +" = "+(x+y));
        return(x+y);
        Buffer();
    }

    static void Main(string [] args)
    {  
        
        
        callFunction();
        callFunction();
        callFunction();
        
        Buffer();
        nameFunction("Arhum");
        Buffer();
        nameFunction("Ali");
        Buffer();
    
        Method("Arhum",23);
        Buffer();
        Method("Ghayyur",23);
        Buffer();
        Method("Wazir",12);
        Buffer();
        
        defualtMethod("Arhum");
        defualtMethod("Ghayyur");
        defualtMethod("Wazir");
        defualtMethod();
        Buffer();

        sumMethod(10,95);
        sumMethod(20,30);
        Buffer();

        for (int i=0;i<11;i++)
        {
          MMethod(i);      
        }

        Mymethod(nam1: "Beaconhouse", nam2: "Concordia", nam3: "Iqra University");

        int Num1 = PlusIntMethod(50,50);
        double Num2 = PlusDoubleMethod(45.39,329.3);
        

    }


}
}