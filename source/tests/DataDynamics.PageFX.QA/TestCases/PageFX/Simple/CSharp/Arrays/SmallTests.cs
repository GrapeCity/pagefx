using System;

struct Point
{
    public int x;
    public int y;
}

class X
{
    static void Test1()
    {
	    Console.WriteLine("Test1");
        int[] arr = new int[] { 10, 20, 30 };
        Console.WriteLine(arr[0]);
        Console.WriteLine(arr[1]);
        Console.WriteLine(arr[2]);
		arr = new int[] { -10, -20, -30 };
		Console.WriteLine(arr[0]);
		Console.WriteLine(arr[1]);
		Console.WriteLine(arr[2]);
    }

    static void Test2()
    {
		Console.WriteLine("Test2");
        byte[] arr = new byte[] { 0x00, 0x01, 0x02, 0x03 };
        Console.WriteLine(arr.Length);
        Console.WriteLine(arr[0]);
        Console.WriteLine(arr[1]);
        Console.WriteLine(arr[2]);
        Console.WriteLine(arr[3]);
    }

    static void Test3()
    {
		Console.WriteLine("Test3");
        string[] arr = new string[] { "aaa", "bbb", "ccc" };
        Console.WriteLine(arr[0]);
        Console.WriteLine(arr[1]);
        Console.WriteLine(arr[2]);
    }

    static void Test4()
    {
		Console.WriteLine("Test4");
        Point[] arr = new Point[3];
        arr[0].x = 1;
        arr[0].y = 1;
        arr[1].x = 2;
        arr[1].y = 2;
        arr[2].x = 3;
        arr[2].y = 3;
        Console.WriteLine(arr[0].x);
        Console.WriteLine(arr[0].y);
        Console.WriteLine(arr[1].x);
        Console.WriteLine(arr[1].y);
        Console.WriteLine(arr[2].x);
        Console.WriteLine(arr[2].y);
    }

    static void Test5()
    {
		Console.WriteLine("Test5");
        int n = 10;
        Point[] arr = new Point[n];
        for (int i = 0; i < n; ++i)
        {
            arr[i].x = i & i;
            arr[i].y = i | i;
        }
        for (int i = 0; i < n; ++i)
        {
            Console.WriteLine(arr[i].x);
            Console.WriteLine(arr[i].y);
        }
    }

    static void Test6()
    {
		Console.WriteLine("Test6");
        int inputCount = 6;
        int blockLen = 3;
        int outLen = 4;
        byte[] arr = new byte[(inputCount != 0)
                                    ? ((inputCount + 2) / blockLen) * outLen
                                    : 0];
        Console.WriteLine(arr.Length);
    }

    static void Test7()
    {
		Console.WriteLine("Test7");
        double[] arr = new double[] { 10, 20, 30 };
        for (int i = 0; i < arr.Length; ++i)
            Console.WriteLine(arr[i]);
    }

    static void Test8()
    {
		Console.WriteLine("Test8");
        byte[] buf = new byte[] { 65, 1, 32, 43, 5, 3, 54, 0 };
        int v = (buf[0] | (buf[1] << 8) | (buf[2] << 16) | (buf[3] << 24));
        Console.WriteLine(v);
    }

    static void Test9()
    {
		Console.WriteLine("Test9");
        {
            byte b = 32;
            int v = b << 16;
            Console.WriteLine(v);
        }
        {
            ushort b = ushort.MaxValue;
            int v = b >> 8;
            Console.WriteLine(v);
        }
    }

    public static void Main()
    {
        Test1();
        Test2();
        Test3();
        Test4();
        Test5();
        Test6();
        Test7();
        Test8();
        Test9();
        Console.WriteLine("<%END%>");
    }
}