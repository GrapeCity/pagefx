using System;

class Foo<T>
{
    public int Test(T foo)
    {
        return 1;
    }

    public int Test(int foo)
    {
        return 2;
    }
}

class X
{
    static void Test1()
    {
        Console.WriteLine("----Test1");
        Foo<long> foo = new Foo<long>();
        Foo<int> bar = new Foo<int>();
        Console.WriteLine(foo.Test(4L));
        Console.WriteLine(foo.Test(3));
        Console.WriteLine(bar.Test(3));
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}
