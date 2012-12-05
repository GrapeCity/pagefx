using System;
using System.Text;

internal class Test
{
    static void Test1()
    {
        Console.WriteLine("---Test1");
        StringBuilder sb = new StringBuilder();
        char[] arr = new char[] { 'a', 'b', 'c', 'd', 'e' } ;
        sb.Append(arr, 1, 3);
        Console.WriteLine(sb);
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}