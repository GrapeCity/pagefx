using System;

class StringFormatTest
{
    static void Main()
    {
        int start = Environment.TickCount;
        int n = 10000;
        for (int i = 0; i < n; ++i)
        {
            string s = string.Format("{0}.{1}.{2}.{3}.{4}.{5}.{6}.{7}.{8}.{9}",
                                     i, i - 1, i - 2, i - 3, i - 4,
                                     i - 5, i - 6, i - 7, i - 8, i - 9);
        }
        int end = Environment.TickCount;
        Console.WriteLine(end - start);
    }
}