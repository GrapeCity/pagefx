using System;

class TestLock
{
    private static readonly object monitor = new object();

    public static void Main()
    {
        lock (monitor)
        {
            for (int i = 0; i < 10; ++i)
                Console.WriteLine(i);
        }
        Console.WriteLine("<%END%>");
    }
}