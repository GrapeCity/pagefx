using System;

class X
{
    public delegate void Simple();

    public delegate Simple Foo();

    public void Hello<U>(U u)
    { }

    public void Test<T>(T t)
    {
        T u = t;
        Hello(u);
        Foo foo = delegate
        {
            T v = u;
            Hello(u);
            return delegate
            {
                Hello(u);
                Hello(v);
            };
        };
        Simple simple = foo();
        simple();
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
