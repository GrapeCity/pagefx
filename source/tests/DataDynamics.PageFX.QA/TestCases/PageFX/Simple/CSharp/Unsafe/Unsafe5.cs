using System;

unsafe class TestUnsafe5
{
    static void i1()
    {
        sbyte i1 = 100;
        int v = 0;
        int* ptr = &v;
        *ptr = i1;
        Console.WriteLine(*ptr);
    }

    static void i2()
    {
        short i1 = 100;
        int v = 0;
        int* ptr = &v;
        *ptr = i1;
        Console.WriteLine(*ptr);
        i1 = (short)*ptr;
        Console.WriteLine(i1);
    }

    static void Main()
    {
        i1();
        i2();
    }
}