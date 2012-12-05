using System;

struct Slot
{
    public Object Key;
    public Object Value;
}

class MyList
{
    private Slot[] table;

    public void Add(object key, object value)
    {
        if (this[key] != null)
            throw new ArgumentException();

        Slot s = new Slot();
        s.Key = key;
        s.Value = value;
        if (table == null)
        {
            table = new Slot[1];
            table[0] = s;
        }
        else
        {
            int n = table.Length;
            Slot[] newarr = new Slot[n + 1];
            Array.Copy(table, newarr, n);
            newarr[n] = s;
        }
    }

    public int Count
    {
        get
        {
            if (table != null)
                return table.Length;
            return 0;
        }
    }

    public object this[object key]
    {
        get
        {
            int n = Count;
            for (int i = 0; i < n; ++i)
            {
                if (Equals(table[i].Key, key))
                    return table[i].Value;
            }
            return null;
        }
    }
}

class X
{
    static void Print(Slot[] arr)
    {
        for (int i = 0; i < arr.Length; ++i)
        {
            Slot s = arr[i];
            Console.WriteLine(s.Key);
            Console.WriteLine(" ");
            Console.WriteLine(s.Value);
            Console.WriteLine();
        }
    }

    static void Test1()
    {
        int n = 2;
        Slot[] arr = new Slot[n];
        Slot a = new Slot();
        a.Key = "a";
        a.Value = 0;
        arr[0] = a;

        Slot b = new Slot();
        b.Key = "b";
        b.Value = 1;
        arr[1] = b;

        Slot[] newarr = new Slot[n];
        Array.Copy(arr, newarr, n);

        Print(arr);
        Print(newarr);

        arr[0] = arr[1];
        Print(arr);
        arr[0].Key = "c";
        Print(arr);
    }

    static void Test2()
    {
        try
        {
            string[] s = new string[] { "blue", "green", "red" };
            Char[] o = new Char[3];
            Array.Copy(s, o, 3);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
        }
    }

    static void Main()
    {
        Test1();
        Test2();
        Console.WriteLine("<%END%>");
    }
}