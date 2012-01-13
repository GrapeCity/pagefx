using System;
using System.Collections;

class MyCollection : CollectionBase
{
    public int Add(string item)
    {
        return List.Add(item);
    }

    public string this[int index]
    {
        get
        {
            return (string)List[index];
        }
    }

    public new MyEnumerator GetEnumerator()
    {
        return new MyEnumerator(List);
    }
}

class MyEnumerator : IEnumerator
{
    private IEnumerator ienum;
    private int i;

    public MyEnumerator(IList list)
    {
        ienum = list.GetEnumerator();
    }

    #region IEnumerator Members
    public object Current
    {
        get 
        { 
            string s = (string)ienum.Current;
            return i + ". " + s;
        }
    }

    public bool MoveNext()
    {
        ++i;
        return ienum.MoveNext();
    }

    public void Reset()
    {
        ienum.Reset();
        i = 0;
    }
    #endregion
}

class X
{
    static void Print(IEnumerable set)
    {
        foreach (object o in set)
            Console.WriteLine(o);
    }

    static void Main()
    {
        MyCollection c = new MyCollection();
        c.Add("aaa");
        c.Add("bbb");
        c.Add("ccc");
        foreach (object o in c)
            Console.WriteLine(o);
        Print(c);
        Console.WriteLine("<%END%>");
    }
}