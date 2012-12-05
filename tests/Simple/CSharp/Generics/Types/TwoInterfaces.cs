using System;
using FirstNamespace;

namespace FirstNamespace
{
    internal interface I1<T>
    {
        T foo(T val);
    }
}

namespace SecondNamespace
{
    internal interface I1<T>
    {
        T foo(T val);
    }
}

class A<T> : I1<T>, SecondNamespace.I1<T>
{
    public T foo(T val)
    {
        return val;
    }
}

class A<T1, T2> : I1<T1>, SecondNamespace.I1<T2>
{
    public T1 foo(T1 val)
    {
        return val;
    }

    public T2 foo(T2 val)
    {
        return val;
    }
}

class Test
{
    static void Test1()
    {
        A<int> a = new A<int>();
        Console.WriteLine((a as I1<int>).foo(5));
        Console.WriteLine((a as SecondNamespace.I1<int>).foo(5));
    }

    static void Test2()
    {
        A<int, double> a = new A<int, double>();
        Console.WriteLine((a as I1<int>).foo(5));
        Console.WriteLine((a as SecondNamespace.I1<double>).foo(5.5));
    }

    static void Test3()
    {
        try
        {
            A<int> a = new A<int>();
            Console.WriteLine((a as I1<string>).foo("aaa"));
        }
        catch (NullReferenceException exc)
        {
            Console.WriteLine("ok");
        }
        catch (Exception e)
        {
            Console.WriteLine("error");
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