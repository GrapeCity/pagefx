using System;

class X
{
    static int A = 100;
    static uint B = 200;

    static void Test1()
    {
        Console.WriteLine("--Test1");
        string c = "<";
        if (A < B)
            Console.WriteLine("{0} {1} {2}", A, c, B);
        else
            Console.WriteLine("{0} {1} {2}", A, c, B);
    }

    static void Test2()
    {
        Console.WriteLine("--Test2");
        string c = "<=";
        if (A <= B)
            Console.WriteLine("{0} {1} {2}", A, c, B);
        else
            Console.WriteLine("{0} {1} {2}", A, c, B);
    }

    static void Test3()
    {
        Console.WriteLine("--Test3");
        string c = ">";
        if (A > B)
            Console.WriteLine("{0} {1} {2}", A, c, B);
        else
            Console.WriteLine("{0} {1} {2}", A, c, B);
    }

    static void Test4()
    {
        Console.WriteLine("--Test4");
        string c = ">=";
        if (A >= B)
            Console.WriteLine("{0} {1} {2}", A, c, B);
        else
            Console.WriteLine("{0} {1} {2}", A, c, B);
    }

    static void Main()
    {
        Test1();
        Test2();
        Test3();
        Test4();
        Console.WriteLine("<%END%>");
    }
}