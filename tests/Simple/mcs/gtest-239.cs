using System;

class Foo<T, U>
{
    public int Test(T t, U u)
    {
        return 1;
    }

    public int Test(int t, U u)
    {
        return 2;
    }

    public int Test(T t, float u)
    {
        return 3;
    }

    public int Test(int t, float u)
    {
        return 4;
    }
}

class X
{
   
    static void Test1()
    {
        Console.WriteLine("----Test1");
        Foo<long, float> a = new Foo<long, float>();
        Console.WriteLine("a1: " + a.Test(3L, 3.14F));
        Console.WriteLine("a2: " + a.Test(3L, 8));
        Console.WriteLine("a3: " + a.Test(3, 3.14F));
        Console.WriteLine("a4: " + a.Test(3, 8));
        
        Foo<long, double> b = new Foo<long, double>();
        Console.WriteLine("b1: " + b.Test(3L, 3.14F));
        Console.WriteLine("b2: " + b.Test(3, 3.14F));
        Console.WriteLine("b3: " + b.Test(3L, 3.14F));
        Console.WriteLine("b4: " + b.Test(3L, 5));
        
        Foo<string, float> c = new Foo<string, float>();
        Console.WriteLine("c1: " + c.Test("Hello", 3.14F));
        
        Foo<int, string> d = new Foo<int, string>();
        Console.WriteLine("d1: " + d.Test(3, "Hello"));
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");    
    }
}
