using System;

class A<T>
{
    public void Bar()
    {
        this.Foo(new B<T>.E());
    }

    void Foo(B<T>.E arg)
    {
        arg.PrintOk();
    }
}

class B<U> : B
{
}

class B
{
    public class E
    {
        public void PrintOk()
        {
            Console.WriteLine("ok");
        }
    }
}

class X
{
    static void Test1()
    {
        Console.WriteLine("Test1");
        A<int> a = new A<int>();
        a.Bar();
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}