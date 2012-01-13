using System;

class Test
{
    static object Foo(int? i)
    {
        return (object)i;
    }

    static object FooG<T>(T? i) where T : struct
    {
        return (object)i;
    }

    static void TestCore(object o)
    {
        Console.WriteLine(o != null);
        Console.WriteLine(o == null);
        if (o != null)
            Console.WriteLine("notnull");
        else
            Console.WriteLine("null");
        if (o == null)
            Console.WriteLine("null");
        else
            Console.WriteLine("notnull");
    }

    static void TestNullable<T>(T? x) where T : struct
    {
        Console.WriteLine(x != null);
        Console.WriteLine(x == null);
        if (x != null)
            Console.WriteLine("notnull");
        else
            Console.WriteLine("null");
        if (x == null)
            Console.WriteLine("null");
        else
            Console.WriteLine("notnull");
    }

    static void Test1()
    {
        Console.WriteLine("--- Test1");
        object o = Foo(null);
        TestCore(o);
    }

    static void Test2()
    {
        Console.WriteLine("--- Test2");
        TestCore(FooG<bool>(null));
        TestCore(FooG<int>(null));
    }

    static void Test3()
    {
        Console.WriteLine("--- Test3");
        Test x = new Test();
        if (x != null)
            Console.WriteLine("notnull");
        else
            Console.WriteLine("null");
    }

    static void Test4()
    {
        Console.WriteLine("--- Test4");
        TestNullable<int>(null);
        TestNullable<int>(0);
        TestNullable<bool>(null);
        TestNullable<bool>(false);
    }

    public static void Main()
    {
        Test1();
        Test2();
        Test3();
        Test4();
        Console.WriteLine("<%END%>");
    }
}