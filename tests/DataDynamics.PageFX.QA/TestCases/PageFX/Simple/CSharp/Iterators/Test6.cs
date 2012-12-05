using System;
using System.Collections;

struct S
{
    int j;

    public IEnumerable Get(int a)
    {
        Console.WriteLine("Sending: " + a);
        yield return a;
        j = 10;
        Console.WriteLine("Sending: " + j);
        yield return j;
    }

    public static IEnumerable GetS(int a)
    {
        yield return 100;
        yield return a;
        yield return 1000;
    }
}

class X
{
    IEnumerable Get(int a)
    {
        yield return 1;
        yield return 2;
        yield return a;
    }

    static IEnumerable GetS(int a)
    {
        yield return a;
        yield return a;
        yield return 1;
    }

    static void Main()
    {
        X y = new X();

        int total = 0;
        foreach (int x in y.Get(5))
        {
            total += x;
        }
        Console.WriteLine(total);
        

        total = 0;
        foreach (int x in GetS(3))
        {
            total += x;
        }
        Console.WriteLine(total);
        

        S s = new S();
        total = 0;
        foreach (int x in s.Get(100))
        {
            Console.WriteLine("Got: " + x);
            total += x;
        }
        Console.WriteLine(total);
        
        total = 0;
        foreach (int x in S.GetS(1))
        {
            total += x;
        }
        Console.WriteLine(total);
        Console.WriteLine("<%END%>");
    }
}
