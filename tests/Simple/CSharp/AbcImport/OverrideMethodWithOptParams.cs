using System;
using PageFX;

class MyComponent : Component
{
    public override void Print(Avm.String s, int n)
    {
        base.Print(s, n);
        Console.WriteLine("end");
    }
}

class X
{
    static void Test1()
    {
        try
        {
            MyComponent c = new MyComponent();
            c.Print(); //with default values
            c.Print("aaa"); //with default second argument
            c.Print("bbb", 20);
        }
        catch (Exception exc)
        {
            Console.WriteLine(exc.GetType());
        }
    }
    
    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}