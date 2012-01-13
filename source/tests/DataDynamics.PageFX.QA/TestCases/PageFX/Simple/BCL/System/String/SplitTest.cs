using System;

class X
{
    static void print(string[] arr)
    {
        if (arr == null)
        {
            Console.WriteLine("null");
        }
        else
        {
            int n = arr.Length;
            for (int i = 0; i < n; ++i)
            {
                if (i > 0) Console.WriteLine(" - ");
                Console.WriteLine(arr[i]);
            }
            Console.WriteLine();
        }
    }

    static int N = 1;

    static void test(string s)
    {
        Console.WriteLine(N++);
        Console.WriteLine(s);
        print(s.Split('.'));
    }

    static void Main()
    {
        test("aa.aa");
        test("aaaa");
        test(".aaaa");
        test("aaaa.");
        test(".aaaa.");
        Console.WriteLine("<%END%>");
    }
}