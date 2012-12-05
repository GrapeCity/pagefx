using System;
using System.Collections;

class X
{
    static IEnumerator GetIt()
    {
        yield return 1;
        yield return 2;
        yield return 3;
    }

    static IEnumerable GetIt2()
    {
        yield return 1;
        yield return 2;
        yield return 3;
    }

    static void Main()
    {
        IEnumerator e = GetIt();
        int total = 0;

        while (e.MoveNext())
        {
            Console.WriteLine("Value=" + e.Current);
            total += (int)e.Current;
        }
        Console.WriteLine(total);

        total = 0;
        foreach (int i in GetIt2())
        {
            Console.WriteLine("Value=" + i);
            total += i;
        }
        Console.WriteLine(total);
        Console.WriteLine("<%END%>");
    }
}
