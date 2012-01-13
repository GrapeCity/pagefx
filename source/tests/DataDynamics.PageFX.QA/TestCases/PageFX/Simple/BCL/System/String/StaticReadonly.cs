using System;

class A
{
    public static readonly string S1 = "aaa";
    public static readonly string S2 = "bbb";
}

class Test
{
    static void Main()
    {
        Console.WriteLine(A.S1);
        Console.WriteLine(A.S2);
        Console.WriteLine("<%END%>");
    }
}