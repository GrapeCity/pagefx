using System;

class A
{
    public int value;
    public int i2 = new int();
}

class Test
{
    private static int counter = 0;
    private const int One = 1;

    static void Test1()
    {
        Console.WriteLine("--- Test1");
        counter = 10;
        Console.WriteLine(counter);
        Console.WriteLine(One);
        A a = new A();
        a.value = 100;
        Console.WriteLine(a.value);
        Console.WriteLine(a.i2);
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}