using System;
using System.Collections.Generic;

// comment this line to see another bug in gmcs (unrelated)
interface IB { bool foo(); }


class B : IB { public bool foo() { return true; } }

interface Filter<T> where T : IB
{
    T Is(IB x);

}

struct K : IB
{
    public bool foo() { return false; }

}

class MyFilter : Filter<K>
{
    public K Is(IB x) { return new K(); }
}

class MyBFilter : Filter<B>
{
    public B Is(IB x) { return new B(); }
}

class M
{

    static T[] foo1<T>(Filter<T> x) where T : IB
    {
        T maybe = x.Is(new B());
        if (maybe != null)
            return new T[] { maybe };
        return new T[0];
    }

    static void Test1()
    {
        MyFilter m = new MyFilter();
        System.Console.WriteLine(foo1<K>(m).Length);
        MyBFilter mb = new MyBFilter();
        System.Console.WriteLine(foo1<B>(mb).Length);
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}
