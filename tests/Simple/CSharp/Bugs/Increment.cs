using System;

struct S
{
    public int X;
}

class Test
{
    static void Test1()
    {
        Console.WriteLine("--- Test1");
        int i = 0;
        try
        {
            i++;
            Console.WriteLine(i);
        }
        catch (Exception exc)
        {
            Console.WriteLine(exc);
        }
    }

    static void Test2()
    {
        Console.WriteLine("--- Test2");
        S s = new S { X = 100 };
        s.X += 100;
        Console.WriteLine(s.X);
    }

    static void Main()
    {
        Test1();
        Test2();
        Console.WriteLine("<%END%>");
    }
}