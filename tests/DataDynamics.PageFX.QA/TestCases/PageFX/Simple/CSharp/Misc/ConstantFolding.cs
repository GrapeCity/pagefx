using System;

class Test
{
    static uint bar = (uint)int.MaxValue + 1;

    public static void Main()
    {
        Console.WriteLine(bar);

        if (Int32.MinValue == 0x80000000)
            Console.WriteLine("1");
        else
            Console.WriteLine("0");
        Console.WriteLine("<%END%>");
    }
}
