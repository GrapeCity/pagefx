using System;

class Program
{
    static void Test1()
    {
        Type type = typeof(int);
        Console.WriteLine(type.FullName);
    }

    static void TestArray(Array arr)
    {
        if (arr is bool[])
        {
            Console.WriteLine("arr is bool[]");
        }
        else if (arr is int[])
        {
            Console.WriteLine("arr is int[]");
        }
    }

    static void Test2()
    {
        try
        {
            int[] arr = new int[5];
            Console.WriteLine(arr is int[]);
            Console.WriteLine(arr is byte[]);
            TestArray(arr);
            Console.WriteLine("ok");
        }
        catch (Exception exc)
        {
            Console.WriteLine(exc.GetType());
        }
    }

    static void Check(byte[] value, int startIndex, int size)
    {
        if (value == null)
            throw new ArgumentNullException("value");
        int n = value.Length;
        if (startIndex < 0 || startIndex >= n)
            throw new ArgumentOutOfRangeException("startIndex", "Index was"
                + " out of range. Must be non-negative and less than the"
                + " size of the collection.");
        if (n - size < startIndex)
            throw new ArgumentException("Destination array is not long"
                + " enough to copy all the items in the collection."
                + " Check array index and length.");
    }

    static void Test3()
    {
        byte[] larger = new byte[] { 0x00, 0x01, 0x02, 0x03 };
        try
        {
            Check(larger, 3, 2);
        }
        catch (ArgumentException exc)
        {
            Console.WriteLine(Equals(exc.GetType(), typeof(ArgumentException)));
        }
    }

    static void Main()
    {
        Test1();
        Test2();
        Test3();
        Console.WriteLine("<%END%>");
    }
}