using System;

class StringConcat
{
    static void Main()
    {
        int start = Environment.TickCount;
        int n = 10000;
        string s = "";
        for (int i = 0; i < n; ++i)
        {
            s += i.ToString();
        }
        int end = Environment.TickCount;
        Console.WriteLine(end - start);
    }
}