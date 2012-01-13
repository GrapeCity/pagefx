using System;

class Test
{
    static void AsCloneable(object obj)
    {
        Console.WriteLine((obj as ICloneable) != null);
    }

    static void Test1()
    {
        Console.WriteLine("--- Test1");
        AsCloneable(new object());
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}