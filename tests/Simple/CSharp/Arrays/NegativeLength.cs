using System;

internal class Test
{
    static void Foo(int n)
    {
        try
        {
            int[] arr = new int[n];
            Console.WriteLine(arr.Length);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
        }
    }

    static void Test1()
    {
        Foo(-1);
        Foo(-10);
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}