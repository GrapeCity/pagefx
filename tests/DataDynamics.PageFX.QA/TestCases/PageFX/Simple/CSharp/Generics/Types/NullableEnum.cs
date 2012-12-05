using System;

enum E { A, B, C, D }

class Test
{
    static void print<T>(T? value) where T : struct
    {
        Console.WriteLine(value);
    }

    static void Test1()
    {
        Console.WriteLine("--- Test1");
        //Console.WriteLine(typeof(E));
        //Console.WriteLine(typeof(int));
        print<E>(E.A);
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}