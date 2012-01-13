using System;

class X
{
    delegate void Foo();
    public void Hello<U>(U u)
    { }

    public void Test<T>(T t)
    {
        Console.WriteLine("----Test1");
        T u = t;
        Hello(u);
        Foo foo = delegate
        {
            Hello(u);
        };
        foo();
        Hello(u);
    }

    static void Test1()
    {
        Console.WriteLine("----Test1");
        try
        {
            X x = new X();
            x.Test(3);
            Console.WriteLine("ok");
        }
        catch (Exception e)
        {
            Console.WriteLine("error " + e.GetType());
        }
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }

}
