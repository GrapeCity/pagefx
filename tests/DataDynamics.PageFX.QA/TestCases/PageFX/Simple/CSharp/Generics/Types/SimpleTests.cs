using System;

class B<T>
{

    public int Bar()
    {
        return 42;
    }
    
    public override string ToString()
    {
        return string.Format("B<{0}>", typeof(T));
    }
}

class C<T>
{ 
    public B<T> Foo()
    {
        return (new B<T>());
    }

    public override string ToString()
    {
        return string.Format("C<{0}>", typeof(T));
    }
}

class D<T> where T:class
{ 
    public string Foo(T t)
    {
        return (t.ToString());
    }
    
    public static string Bar(T t)
    {
        return (t.ToString());
    }
}


class A<T>
{
    public T Value;

    public static T SharedValue;

    public A()
    {
    }

    public A(T value)
    {
        Value = value;
    }

    public void TestCtor()
    {
        A<T> a = new A<T>();
        Console.WriteLine(a.Value);
    }

    public void TestStaticCall()
    {
        Console.WriteLine(A<T>.SharedValue);
    }

    public int ABC()
    { 
        C<T> c = new C<T>();
        return (c.Foo().Bar());
    }
}

class Test
{
    static void Test1()
    {
        Console.WriteLine("--- Test1");
        A<int> a = new A<int>(10);
        Console.WriteLine(a.Value);
    }

    static void Test2()
    {
        Console.WriteLine("--- Test2");
        Console.WriteLine(A<int>.SharedValue);
        A<string>.SharedValue = "Shared String";
        Console.WriteLine(A<string>.SharedValue);
        Console.WriteLine(A<bool>.SharedValue);
    }

    static void Test3()
    {
        Console.WriteLine("--- Test3");
        A<int> a = new A<int>(10);
        a.TestCtor();
    }

    static void Test4()
    {
        Console.WriteLine("--- Test4");
        A<int> a = new A<int>(10);
        a.TestStaticCall();
    }

    static void Test5()
    {
        Console.WriteLine("--- Test5");
        A<int> a = new A<int>(10);
        Console.WriteLine(a.ABC());
    }

    static void Test6()
    {
        Console.WriteLine("--- Test5");
        D<C<int>> d = new D<C<int>>();
        Console.WriteLine(d.Foo(new C<int>()));
        Console.WriteLine(D<B<double>>.Bar(new B<double>()));
    }

    static void Main()
    {
        Test1();
        Test2();
        Test3();
        Test4();
        Test5();
        Test6();
        Console.WriteLine("<%END%>");
    }
}