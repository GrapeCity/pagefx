using System;

interface ITest
{
    void Test();
}

class Tester<T> where T : ITest, new()
{
    public void Do()
    {
        new T().Test();
    }
}

class Reference : ITest
{
    public void Test()
    {
        Console.WriteLine("Reference");
    }
}

struct Value : ITest
{
    public void Test()
    {
        Console.WriteLine("Value");
    }
}

class Test
{
    static void Test1()
    {
        Console.WriteLine("----Test1");
        new Tester<Reference>().Do();
        new Tester<Value>().Do();
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}


