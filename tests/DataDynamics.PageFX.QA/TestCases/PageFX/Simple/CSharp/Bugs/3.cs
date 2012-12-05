using System;

internal class Test
{
    static void Test1()
    {
        Console.WriteLine(DateTime.MaxValue.Ticks);
        Console.WriteLine(DateTime.MinValue.Ticks);
        DateTime dt_local = new DateTime(1, DateTimeKind.Local);
        long toBin = (long)((ulong)dt_local.ToUniversalTime().Ticks | 0x8000000000000000);
        Console.WriteLine((ulong)toBin >> 63);

    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}