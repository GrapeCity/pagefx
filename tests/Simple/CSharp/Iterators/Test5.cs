using System;
using System.Collections;

class X
{
    static IEnumerable GetIt()
    {
        List l = new List(3);
        l.Add(1);
        l.Add(2);
        l.Add(3);

        foreach (int i in l)
            yield return i;
    }

    static void Main()
    {
        int total = 0;
        foreach (int i in GetIt())
        {
            Console.WriteLine("Got: " + i);
            total += i;
        }

        Console.WriteLine(total);
        Console.WriteLine("<%END%>");
    }
}

public class List : IEnumerable
{

    int pos = 0;
    int[] items;

    public List(int i)
    {
        items = new int[i];
    }

    public void Add(int value)
    {
        items[pos++] = value;
    }

    public MyEnumerator GetEnumerator()
    {
        return new MyEnumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public struct MyEnumerator : IEnumerator
    {

        List l;
        int p;

        public MyEnumerator(List l)
        {
            this.l = l;
            p = -1;
        }

        public object Current
        {
            get
            {
                return l.items[p];
            }
        }

        public bool MoveNext()
        {
            return ++p < l.pos;
        }

        public void Reset()
        {
            p = 0;
        }
    }
}