using System;
using System.Collections.Generic;

struct Point
{
    public int X, Y;

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        return string.Format("({0}, {1})", X, Y);
    }
}

class Test
{
    static void Print<T>(IEnumerable<T> arr)
    {
        if (arr == null) return;
        foreach (var item in arr)
            Console.WriteLine(item);
    }

    static void TestCopyTo<T>(ICollection<T> c, int n, int index)
    {
        Console.WriteLine("-- CopyTo({0}, {1})", n, index);
        try
        {
            T[] arr = new T[n];
            c.CopyTo(arr, index);
            Print(arr);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
        }
    }

    static void TestICollection<T>(T[] arr)
    {
        Console.WriteLine("--- ICollection<{0}>", typeof(T));

        Print(arr);

        var c = (ICollection<T>)arr;

        int n = c.Count;
        Console.WriteLine("-- Count");
        Console.WriteLine(n);

        Console.WriteLine("-- IsReadOnly");
        Console.WriteLine(c.IsReadOnly);

        Console.WriteLine("-- Add");
        try
        {
            c.Add(default(T));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
        }

        Console.WriteLine("-- Contains");
        try
        {
            Console.WriteLine(c.Contains(default(T)));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
        }

        TestCopyTo(c, n, 0);
        TestCopyTo(c, n, -1);
        TestCopyTo(c, n, 1);
        TestCopyTo(c, n, 2);

        TestCopyTo(c, -1, 0);
        TestCopyTo(c, 0, 0);
        TestCopyTo(c, 1, 0);
        TestCopyTo(c, 2, 0);

        Console.WriteLine("-- Remove");
        try
        {
            Console.WriteLine(c.Remove(default(T)));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
        }

        Console.WriteLine("-- Clear");
        try
        {
            c.Clear();
            Print(arr);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
        }
    }

    static void TestIList<T>(T[] arr)
    {
        Console.WriteLine("--- IList<{0}>", typeof(T));

        var l = (IList<T>)arr;

        Console.WriteLine("-- IndexOf");
        try
        {
            Console.WriteLine(l.IndexOf(default(T)));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
        }

        Console.WriteLine("-- Insert");
        try
        {
            l.Insert(0, default(T));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
        }

        Console.WriteLine("-- RemoveAt");
        try
        {
            l.RemoveAt(0);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
        }

        Console.WriteLine("-- this[0]");
        try
        {
            Console.WriteLine(l[0]);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
        }

        Console.WriteLine("-- this[0] = default(T)");
        try
        {
            l[0] = default(T);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
        }
    }

    static void TestICollection()
    {
        Console.WriteLine("--- TestICollection");
        TestICollection(new[] { 10, 20, 30 });
        TestICollection(new[] { "Hello", "World", "!!!" });
        TestICollection(new string[] { null, "", "!!!" });
        TestICollection(new Point[] { new Point(10, 10), new Point(), new Point(20, 20) });
    }

    static void TestIList()
    {
        Console.WriteLine("--- TestIList");
        TestIList(new int[0]);
        TestIList(new string[0]);
        TestIList(new[] { 10, 20, 30 });
        TestIList(new[] { "Hello", "World", "!!!" });
        TestIList(new string[] { null, "", "!!!" });
        TestIList(new Point[] { new Point(10, 10), new Point(), new Point(20, 20) });
    }

    static void Main()
    {
        TestICollection();
        TestIList();
        Console.WriteLine("<%END%>");
    }
}