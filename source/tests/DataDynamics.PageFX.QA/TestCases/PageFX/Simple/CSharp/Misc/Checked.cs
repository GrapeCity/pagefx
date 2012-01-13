using System;

class OverflowTest
{
    static void add(short a, short b)
    {
        Console.WriteLine("a = {0}", a);
        Console.WriteLine("b = {0}", b);
        try
        {
            int z = checked((short)(a + b));
            Console.WriteLine("checked(a + b) = {0}", z);
        }
        catch (OverflowException e)
        {
            Console.WriteLine("checked(a + b) = overflow is occured");
        }
    }

    static void add_un(ushort a, ushort b)
    {
        Console.WriteLine("a = {0}", a);
        Console.WriteLine("b = {0}", b);
        try
        {
            int z = checked((ushort)(a + b));
            Console.WriteLine("checked(a + b) = {0}", z);
        }
        catch (OverflowException e)
        {
            Console.WriteLine("checked(a + b) = overflow is occured");
        }
    }

    static void sub(short a, short b)
    {
        Console.WriteLine("a = {0}", a);
        Console.WriteLine("b = {0}", b);
        try
        {
            int z = checked((short)(a - b));
            Console.WriteLine("checked(a - b) = {0}", z);
        }
        catch (OverflowException e)
        {
            Console.WriteLine("checked(a - b) = overflow is occured");
        }
    }

    static void sub_un(ushort a, ushort b)
    {
        Console.WriteLine("a = {0}", a);
        Console.WriteLine("b = {0}", b);
        try
        {
            int z = checked((ushort)(a - b));
            Console.WriteLine("checked(a - b) = {0}", z);
        }
        catch (OverflowException e)
        {
            Console.WriteLine("checked(a - b) = overflow is occured");
        }
    }

    static void mul(short a, short b)
    {
        Console.WriteLine("a = {0}", a);
        Console.WriteLine("b = {0}", b);
        try
        {
            int z = checked((short)(a * b));
            Console.WriteLine("checked(a * b) = {0}", z);
        }
        catch (OverflowException e)
        {
            Console.WriteLine("checked(a - b) = overflow is occured");
        }
    }

    static void mul_un(ushort a, ushort b)
    {
        Console.WriteLine("a = {0}", a);
        Console.WriteLine("b = {0}", b);
        try
        {
            int z = checked((ushort)(a * b));
            Console.WriteLine("checked(a * b) = {0}", z);
        }
        catch (OverflowException e)
        {
            Console.WriteLine("checked(a - b) = overflow is occured");
        }
    }

    static void Main()
    {
        add(10, 20);
        add(32767, 32767);
        sub(20, 10);
        sub(32767, -32767);
        mul(10, 10);
        mul(3000, 5000);

        add_un(10, 20);
        add_un(32767, 32767);
        sub_un(20, 10);
        sub_un(500, 600);
        mul_un(10, 10);
        mul_un(3000, 5000);
    }
}