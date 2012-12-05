using System;

class Test1
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

    static void Main()
    {
        int i = 0;
        f(i);
        f(i++);
        f(++i);
        f(i);
        f(i--);
        f(--i);
        f(i);

        f(_value);
        f(_value++);
        f(++_value);
        f(_value);
        f(_value--);
        f(--_value);
        f(_value);

        f(Value);
        f(Value++);
        f(++Value);
        f(Value);
        f(Value--);
        f(--Value);
        f(Value);

        ++i;
        f(i);
        i--;
        f(i);
        Console.WriteLine("<%END%>");
    }
}