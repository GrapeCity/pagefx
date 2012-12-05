using System;
using PageFX;

class MyComponent : Component
{
    public new void OnInit()
    {
        base.OnInit();
        Console.WriteLine("MyComponent::OnInit");
    }
}

class X
{
    static void Test1()
    {
        try
        {
            MyComponent c = new MyComponent();
            c.Init();
        }
        catch(Exception exc)
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