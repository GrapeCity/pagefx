using System;
using System.Collections;
using System.Linq;

internal class Test
{
    static void Test1()
    {
        Console.WriteLine("--- Test1");

        object[] data = new object[] { 1, 2, 3 };

        try
        {
            var e = data.Cast<IEnumerable>().GetEnumerator();
            Console.WriteLine("move");
            e.MoveNext();
        }
        catch (Exception exc)
        {
            Console.WriteLine(exc.GetType());
        }
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}