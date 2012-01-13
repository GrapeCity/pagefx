using System;
using System.Collections;

class HashtableTest
{
    static void Main()
    {
        Hashtable table = new Hashtable();
        int n = 100000;
        int r = 0;
        int start = Environment.TickCount;
        for (int i = 0; i < n; ++i)
        {
            table[i] = i;
        }
        for (int i = 0; i < n; ++i)
        {
            int v = (int)table[i];
            r += v;
        }
        int end = Environment.TickCount;
        Console.WriteLine(end - start);
    }
}