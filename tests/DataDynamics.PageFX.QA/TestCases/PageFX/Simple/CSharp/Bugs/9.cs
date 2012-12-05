using System;
using System.Collections;

struct S
{
    public int X;

    public void Mod()
    {
        X++;
    }
}

internal class Test
{
    static void Main()
    {
        Stack stack = new Stack();
        S a = new S { X = 10 };
        stack.Push(a);
        a.Mod();
        S b = (S)stack.Pop();
        Console.WriteLine(b.X);
        Console.WriteLine(a.X);
        Console.WriteLine("<%END%>");
    }
}