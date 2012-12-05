using System;

public class Test
{
    static void Test1()
    {
        Console.WriteLine("----Test1");
        SimpleStruct<string> s = new SimpleStruct<string>();
        Console.WriteLine("ok");
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}

public struct SimpleStruct<T>
{
    T data;

    public SimpleStruct(T data)
    {
        this.data = data;
    }
}
