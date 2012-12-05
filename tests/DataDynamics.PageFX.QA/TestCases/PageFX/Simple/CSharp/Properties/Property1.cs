using System;

class A
{
    private int _x;

    public int X
    {
        get { return _x; }
        set { _x = value; }
    }
}

class Property1
{
    static void Main()
    {
        A obj = new A();
        obj.X = 10;
        Console.WriteLine(obj.X);
        Console.WriteLine("<%END%>");
    }
}