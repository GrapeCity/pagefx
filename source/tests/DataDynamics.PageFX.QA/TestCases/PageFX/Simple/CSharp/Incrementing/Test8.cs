using System;

class Test8
{
    static int Value
    {
        get { return _value; }
        set { _value = value; }
    }
    static int _value;

    static void f1(int i)
    {
        Console.WriteLine(i);
    }

    static void f2(int a, int b)
    {
        Console.WriteLine(a);
        Console.WriteLine(b);
    }

    static void Main()
    {
        //f1(_value);
        //f1(_value++);

        f1(Value++);
        f2(Value++, Value++);
        Console.WriteLine("<%END%>");
    }
}