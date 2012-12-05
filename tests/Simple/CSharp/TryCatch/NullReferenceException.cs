using System;

class A
{
    public void foo()
    {
        Console.WriteLine("A::foo()");
    }
}

class X
{
    static void Test1(A obj)
    {
        try
        {
            obj.foo();
        }
        catch (Exception exc)
        {
            Console.WriteLine(exc.GetType().FullName);
        }
    }

    static void Test1()
    {
        Test1(null);
        Test1(new A());
    }

    static void Main()
    {
        Console.WriteLine("Hello");
        Test1();
        Console.WriteLine("<%END%>");
    }
}