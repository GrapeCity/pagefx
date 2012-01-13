using System;
using System.Collections;

class X
{
    static int start, end;
    static int i;

    static IEnumerator GetRange()
    {
        yield return 1;
        for (i = start; i < end; i++)
            yield return i;
        yield return 100;
    }

    static void Main()
    {
        start = 10;
        end = 30;

        int total = 0;

        IEnumerator e = GetRange();
        while (e.MoveNext())
        {
            Console.WriteLine("Value=" + e.Current);
            total += (int)e.Current;
        }
        Console.WriteLine(total);
        Console.WriteLine("<%END%>");
    }
}
