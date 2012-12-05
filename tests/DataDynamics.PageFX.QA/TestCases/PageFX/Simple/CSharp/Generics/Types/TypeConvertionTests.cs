using System;

class A<T> where T : class
{
    public void TestAs(object obj)
    {
        Test.Write(obj as T);
    }

    public void TestIs(object obj)
    {
        Console.WriteLine(obj is T);
    }
}

class Test
{
    internal static void Write(object obj)
    {
        if (obj != null)
        {
            Console.WriteLine(obj);
        }
        else
        {
            Console.WriteLine("null");
        }
    }

    static object[] set = new object[]
    {
        null,
        new Test(),
        new object(),
        0,
        "aaa",
        true,
        false
    };

    static void TestCore<T>() where T : class
    {
        A<T> a = new A<T>();
        Console.WriteLine("-- TestAs");
        foreach (var v in set)
            a.TestAs(v);
        Console.WriteLine("-- TestIs");
        foreach (var v in set)
            a.TestIs(v);
    }

    static void Test1()
    {
        Console.WriteLine("--- Test");
        TestCore<Test>();
    }

    static void Test2()
    {
        Console.WriteLine("--- Object");
        TestCore<object>();
    }

    static void Test3()
    {
        Console.WriteLine("--- String");
        TestCore<string>();
    }

    static void Main()
    {
        Test1();
        Test2();
        Test3();
        Console.WriteLine("<%END%>");
    }

    public override string ToString()
    {
        return "Test";
    }
}