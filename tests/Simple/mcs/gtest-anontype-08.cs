using System;

static class Test
{
    public static void Main ()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }

    private static void Test1()
    {
        var a = new { X = 1, Y = 2, };
        Console.WriteLine(a);
    }
}
