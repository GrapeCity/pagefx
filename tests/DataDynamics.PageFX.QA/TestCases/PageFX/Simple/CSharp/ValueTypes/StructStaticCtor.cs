using System;

struct S
{
    public int Value;

    public S(int value)
    {
        Value = value;
    }

    public static S One = new S(1);
    public static S Two = new S(2);

    public override string ToString()
    {
        return Value.ToString();
    }
}

class Test
{
    static void Main()
    {
        Console.WriteLine(S.One);
        Console.WriteLine(S.Two);
        Console.WriteLine("<%END%>");
    }
}