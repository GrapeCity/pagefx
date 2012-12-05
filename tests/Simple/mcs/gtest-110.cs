﻿using System;

public interface IList<R>
{
    int Map<S>(S item);
}

public class List<T> : IList<T>
{
    public int Map<U>(U item)
    {
        return 1;
    }
}

public class SpecialList<V> : IList<V>
{
    public int Map<W>(W item)
    {
        return 2;
    }
}

class X
{
    static int Test1()
    {
        Console.WriteLine("----Test1");
        IList<int> list = new List<int>();
        int result = list.Map("Hello");
        if (result != 1)
            return 1;

        IList<int> list2 = new SpecialList<int>();
        int result2 = list2.Map("World");
        if (result2 != 2)
            return 2;

        return 0;
    }

    static void Main()
    {
        Console.WriteLine(Test1());
        Console.WriteLine("<%END%>");
    }
}
