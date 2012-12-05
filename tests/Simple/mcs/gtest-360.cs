using System;

class C
{
    internal static int Foo<T>(T name, params object[] args)
    {
        return 2;
    }

    internal static int Foo(string name, params object[] args)
    {
        return 0;
    }

    internal static int InvokeMethod(string name, params object[] args)
    {
        return Foo(name, args);
    }

    static void Test1()
    {
        Console.WriteLine("----Test1");
        Console.WriteLine(InvokeMethod("abc"));
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}