using System;

class Row
{
    public int Count
    {
        get { return _data.Length; }
    }

    public object this[int i]
    {
        get { return _data[i]; }
        set { _data[i] = value; }
    }
    private readonly object[] _data = new object[10];
}

class X
{
    private static int n;

    static string GetString()
    {
        ++n;
        return n.ToString();
    }

    static void Main()
    {
        Row row = new Row();
        for (int i = 0; i < row.Count; ++i)
        {
            string s = GetString();
            row[i] += s;
        }
        Console.WriteLine("<%END%>");
    }
}