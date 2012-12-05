using System;

class Property1
{
    private static int _value;

    static int Value
    {
        get { return _value; }
        set { _value = value; }
    }

    static void Main()
    {
        Value = 100;
        Console.WriteLine(Value);
        Console.WriteLine("<%END%>");
    }
}