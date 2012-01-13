using System;
using System.Collections;

public class Foo : IDisposable
{
    public readonly int Data;

    public Foo(int data)
    {
        this.Data = data;
    }

    public bool disposed;

    public void Dispose()
    {
        disposed = true;
    }
}

class X
{
    public static IEnumerable Test(int a, int b)
    {
        Foo foo3, foo4;

        using (Foo foo1 = new Foo(a), foo2 = new Foo(b))
        {
            yield return foo1.Data;
            yield return foo2.Data;

            foo3 = foo1;
            foo4 = foo2;
        }

        yield return foo3.disposed;
        yield return foo4.disposed;
    }

    static void Main()
    {
        ArrayList list = new ArrayList();
        foreach (object data in Test(3, 5))
            list.Add(data);

        if (list.Count != 4)
        {
            Console.WriteLine("1");
            return;
        }

        if ((int)list[0] != 3)
        {
            Console.WriteLine("2");
            return;
        }

        if ((int)list[1] != 5)
        {
            Console.WriteLine("3");
            return;
        }

        if (!(bool)list[2])
        {
            Console.WriteLine("4");
            return;
        }

        if (!(bool)list[3])
        {
            Console.WriteLine("5");
            return;
        }
        Console.WriteLine("<%END%>");
    }
}
