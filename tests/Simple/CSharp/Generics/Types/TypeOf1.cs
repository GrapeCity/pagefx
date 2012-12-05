using System;

class A<T>
{
    public void TestTypeOf(object obj)
    {
        try
        {
            Console.WriteLine(obj.GetType() == typeof(T));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
        }
    }
}

class Test
{
    static void Test1()
    {
        Console.WriteLine("");
        A<int> a = new A<int>();
        a.TestTypeOf(null);
        a.TestTypeOf(new object());
        a.TestTypeOf(new Test());
        a.TestTypeOf(new int());
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}