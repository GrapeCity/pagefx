using System;
using System.Collections;
using System.Collections.Generic;

public class BaseCollection<T> : IEnumerable<T>
{
    //protected List<T> items = new List<T>();
    protected T[] items = new T[2];

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        foreach (T item in items)
            yield return item;
        //return items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return items.GetEnumerator();
    }
}

public class BaseIntList<T> : BaseCollection<T>
{
}

public class IntList : BaseIntList<int>
{
}

class X
{
    static void Test1()
    {
        Console.WriteLine("----Test1");
        IntList list = new IntList();
        foreach (int i in list)
        {
            Console.WriteLine(i);
        }
    }

    public static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}
