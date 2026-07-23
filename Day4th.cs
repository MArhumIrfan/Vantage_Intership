using System;

namespace Day4th{
class Methods
{   

    static void Buffer()
    {
    Console.WriteLine("_____-----*****-----_____");
    }
    
    static void callFunction()
    {
        Console.WriteLine("Hello World!");
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



    }


}
}