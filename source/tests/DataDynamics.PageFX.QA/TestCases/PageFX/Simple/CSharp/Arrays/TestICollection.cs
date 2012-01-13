using System;
using System.Collections;

class Program
{
    static void Print(ICollection col)
    {
        foreach (object o in col)
        {
            Console.WriteLine(o.GetType().FullName);
            Console.WriteLine(o);
        }
    }

    static void Test1()
    {
        char[] arr = { 'a', 'b', 'c' };
        Print(arr);
    }

    static void Test2()
    {
        char[] arr1 = { 'a', 'b', 'c' };
        object[] arr2 = new object[3];
        arr2[0] = arr1[0];
        arr2[1] = arr1[1];
        arr2[2] = arr1[2];
        Print(arr2);
    }

    static void Main()
    {
        Test1();
        Test2();
        Console.WriteLine("<%END%>");
    }
}