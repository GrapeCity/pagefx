using System;
using System.Collections;

class X
{
    static IEnumerable GetIt(int[] args)
    {
        foreach (int a in args)
            yield return a;
    }

    static IEnumerable GetMulti(int[,] args)
    {
        foreach (int a in args)
            yield return a;
    }

    static void Main()
    {
        int total = 0;
        foreach (int i in GetIt(new int[] { 1, 2, 3 }))
        {
            Console.WriteLine("Got: " + i);
            total += i;
        }

        Console.WriteLine(total);
        
        total = 0;
        foreach (int i in GetMulti(new int[,] { { 10, 20 }, { 30, 40 } }))
        {
            Console.WriteLine("Got: " + i);
            total += i;
        }

        Console.WriteLine(total);
        Console.WriteLine("<%END%>");
    }

}
