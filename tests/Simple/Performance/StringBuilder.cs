using System;
using System.Text;

class StringBuilderTest
{
    static void Main()
    {
        int start = Environment.TickCount;
        int n = 10000;
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < n; ++i)
        {
            sb.Append(i);
        }
        int end = Environment.TickCount;
        Console.WriteLine(end - start);
    }
}