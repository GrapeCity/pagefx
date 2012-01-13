using System;

public interface IToString
{
    string ToString();
}

class X : IToString
{
    public override string ToString()
    {
        return "A";
    }

    static void Main()
    {
        X x = new X();
        Console.WriteLine(x.ToString());
        Console.WriteLine("<%END%>");
    }
}