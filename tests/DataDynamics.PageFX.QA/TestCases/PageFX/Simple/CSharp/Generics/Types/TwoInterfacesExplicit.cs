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
    #region Implementation of I1<T>

    T I1<T>.foo(T val)
    {
        return val;
    }

    #endregion

    #region Implementation of I1<T>

    T SecondNamespace.I1<T>.foo(T val)
    {
        return val;
    }

    #endregion
}

class A<T1, T2> : I1<T1>, SecondNamespace.I1<T2>
{
    #region Implementation of I1<T1>

    T1 I1<T1>.foo(T1 val)
    {
        return val;
    }

    #endregion

    #region Implementation of I1<T2>

    T2 SecondNamespace.I1<T2>.foo(T2 val)
    {
        return val;
      
    }

    #endregion
}

class B<T> : I1<T>, SecondNamespace.I1<T>
{
    public T foo(T val)
    {
        return default(T);
    }
}

class B<T1, T2> : I1<T1>, SecondNamespace.I1<T2>
{
    public T1 foo(T1 val)
    {
        return default(T1);
    }

    public T2 foo(T2 val)
    {
        return default(T2);
    }
}

class Test
{
    static void Test1()
    {
        Console.WriteLine("----Test1");
        A<int> a = new A<int>();
        Console.WriteLine((a as I1<int>).foo(5));
        Console.WriteLine((a as SecondNamespace.I1<int>).foo(5));
    }

    static void Test2()
    {
        Console.WriteLine("----Test2");
        A<int, double> a = new A<int, double>();
        Console.WriteLine((a as I1<int>).foo(5));
        Console.WriteLine((a as SecondNamespace.I1<double>).foo(2.718281828459045235360287));
    }

    static void Test3()
    {
        Console.WriteLine("----Test3");
        B<int> b = new B<int>();
        Console.WriteLine(b.foo(5));
        Console.WriteLine(b.foo(5));
    }

    static void Test4()
    {
        Console.WriteLine("----Test4");
        B<int, double> b = new B<int, double>();
        Console.WriteLine(b.foo(5));
        Console.WriteLine(b.foo(2.718281828459045235360287));
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