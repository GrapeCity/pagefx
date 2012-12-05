// Generic delegates.

using System;

delegate void Test<T>(T t);

class Foo<T>
{
    public event Test<T> MyEvent;

    public void Hello(T t)
    {
        if (MyEvent != null)
            MyEvent(t);
    }
}

class X
{
    static void do_hello(string hello)
    {
        Console.WriteLine("Hello: {0}", hello);
    }
    
    static void Test1()
    {
        Console.WriteLine("----Test1");
        try
        {
            Foo<string> foo = new Foo<string>();
            foo.MyEvent += new Test<string>(do_hello);
            foo.Hello("Boston");
        }
        catch (Exception e)
        {
            Console.WriteLine("error " + e);
        }
    }
    
    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}
