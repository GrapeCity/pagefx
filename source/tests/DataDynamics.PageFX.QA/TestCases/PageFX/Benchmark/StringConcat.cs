using System;

class Test
{
    static void Main()
    {
        int start = Environment.TickCount;
        int n = 100000;
        string s = "";
        for (int i = 0; i < n; ++i)
        {
            s += i.ToString();
        }
        int end = Environment.TickCount;
        Console.WriteLine(end - start);
        Console.WriteLine("<%END%>");
    }
}