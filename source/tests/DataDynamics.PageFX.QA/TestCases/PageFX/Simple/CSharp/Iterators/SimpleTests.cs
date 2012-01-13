using System;
using System.Collections;

class Program
{
    static IEnumerable GetRange(int a, int b)
    {
        for (int i = a; i <= b; ++i)
            yield return i;
    }

    static void Test1()
    {
        foreach (object o in GetRange(0, 9))
        {
            Console.WriteLine(o);
        }
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}