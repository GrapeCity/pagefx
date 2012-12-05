using System;

internal class H : 
    ICloneable,
    IComparable<int>,
    IEquatable<int>,
    IEquatable<string>,
    IComparable<string>
{
    public int Value;

    public object Clone()
    {
        return new H {Value = Value};
    }

    public bool Equals(int other)
    {
        return Value == other;
    }

    public int CompareTo(int other)
    {
        return Value - other;
    }

    public bool Equals(string other)
    {
        int r;
        if (int.TryParse(other, out r))
            return r == Value;
        return false;
    }

    public int CompareTo(string other)
    {
        int r;
        if (int.TryParse(other, out r))
            return Value - r;
        return -1;
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    public override bool Equals(object obj)
    {
        if (obj == this) return true;
        var h = obj as H;
        if (h == null) return false;
        return h.Value == Value;
    }

    public override int GetHashCode()
    {
        return Value ^ 2352365;
    }
}

internal class Test
{
    static object[] TestObjects = new[]
    {
        null,
        new object(),
        "aaa",
        new int(),
        new H {Value = 10},
        new H {Value = 20},
        new H {Value = 100},
    };

    static void Run(Action<object> action)
    {
        foreach (var o in TestObjects)
            action(o);
    }

    static void TestICloneable(object obj)
    {
        Console.WriteLine(obj);
        try
        {
            var c = obj as ICloneable;
            if (c != null)
            {
                var obj2 = c.Clone();
                Console.WriteLine(obj2);
                Console.WriteLine(Equals(obj, obj2));
            }
            else
            {
                Console.WriteLine("null");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
        }
    }

    static void TestICloneable()
    {
        Console.WriteLine("--- TestICloneable");
        Run(TestICloneable);
    }

    static void Compare(object x, object y)
    {
        try
        {
            var c = x as IComparable;
            if (c != null)
            {
                Console.WriteLine(c.CompareTo(y));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
        }
    }

    static void CompareStr(object obj, string s)
    {
        try
        {
            var c = obj as IComparable<string>;
            if (c != null)
            {
                Console.WriteLine(c.CompareTo(s));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
        }
    }

    static void TestIComparable(object obj)
    {
        Console.WriteLine(obj);
        Compare(obj, null);
        Compare(obj, new object());
        Compare(obj, "aaa");
        Compare(obj, new int());
        Compare(obj, new H { Value = 10 });

        CompareStr(obj, "aaa");
        CompareStr(obj, "bbb");
    }

    static void TestIComparable()
    {
        Console.WriteLine("--- TestIComparable");
        Run(TestIComparable);
    }

    static void EqInt(object obj, int value)
    {
        try
        {
            var e = obj as IEquatable<int>;
            if (e != null)
            {
                Console.WriteLine(e.Equals(value));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
        }
    }

    static void EqStr(object obj, string s)
    {
        try
        {
            var e = obj as IEquatable<string>;
            if (e != null)
            {
                Console.WriteLine(e.Equals(s));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
        }
    }

    static void TestIEquatable(object obj)
    {
        EqInt(obj, 10);
        EqInt(obj, 100);
        EqStr(obj, "aaa");
        EqStr(obj, "bbb");
    }

    static void TestIEquatable()
    {
        Console.WriteLine("--- TestIEquatable");
        Run(TestIEquatable);
    }

    static void Main()
    {
        TestICloneable();
        TestIComparable();
        TestIEquatable();
        Console.WriteLine("<%END%>");
    }
}