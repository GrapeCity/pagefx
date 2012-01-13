using System;
using System.Collections;

public class Test
{
    public IEnumerable Foo(int a)
    {
        try
        {
            try
            {
                yield return a;
            }
            finally
            {
                Console.WriteLine("Hello World");
            }

            Console.WriteLine("Next block");

            try
            {
                yield return a * a;
            }
            finally
            {
                Console.WriteLine("Boston");
            }
        }
        finally
        {
            Console.WriteLine("Outer finally");
        }

        Console.WriteLine("Outer block");
        yield break;
    }
}

class X
{
    static void Main()
    {
        Test test = new Test();

        ArrayList list = new ArrayList();
        foreach (object o in test.Foo(5))
            list.Add(o);

        Console.WriteLine(list.Count);
        Console.WriteLine(list[0]);
        Console.WriteLine(list[1]);
        
        IEnumerable a = test.Foo(5);

        IEnumerator b = a as IEnumerator;
        if (b != null)
        {
            if (b.MoveNext())
            {
                Console.WriteLine("4");
                return;
            }
        }

        IEnumerator c = a.GetEnumerator();
        if (!c.MoveNext())
        {
            Console.WriteLine("5");
            return;
        }

        if ((int)c.Current != 5)
        {
            Console.WriteLine("6");
            return;
        }

        if (!c.MoveNext())
        {
            Console.WriteLine("7");
            return;
        }

        if ((int)c.Current != 25)
        {
            Console.WriteLine("8");
            return;
        }

        IEnumerator d = a.GetEnumerator();

        if ((int)c.Current != 25)
        {
            Console.WriteLine("9");
            return;
        }
        if (!d.MoveNext())
        {
            Console.WriteLine("10");
            return;
        }

        if ((int)c.Current != 25)
        {
            Console.WriteLine("11");
            return;
        }

        if ((int)d.Current != 5)
        {
            Console.WriteLine("12");
            return;
        }

        if (c.MoveNext())
        {
            Console.WriteLine("13");
            return;
        }

        ((IDisposable)a).Dispose();
        Console.WriteLine("<%END%>");
    }
}
