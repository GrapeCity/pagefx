using System;

class X
{
    static void Test1()
    {
        Array arr = Array.CreateInstance(typeof(string), 3);
        arr.SetValue("a", 0);
        arr.SetValue("b", 1);
        arr.SetValue("c", 2);
        Console.WriteLine(arr.GetType().FullName);
        for (int i = 0; i < arr.Length; ++i)
            Console.WriteLine(arr.GetValue(i));
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}