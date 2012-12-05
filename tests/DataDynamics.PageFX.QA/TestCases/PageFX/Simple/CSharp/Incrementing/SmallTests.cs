using System;

class X
{
    static int Value
    {
        get { return _value; }
        set { _value = value; }
    }
    static int _value;

    static void f(int i)
    {
        Console.WriteLine(i);
    }

    static void Test1()
    {
        f(Value++);
        f(++Value);
        f(Value);
    }

    static void foo(int i)
    {
        Console.WriteLine(i++ + ++i);
    }

    static void Test2()
    {
        foo(5);
    }

    static void Main()
    {
        Test1();
        Test2();
        Console.WriteLine("<%END%>");
    }
}