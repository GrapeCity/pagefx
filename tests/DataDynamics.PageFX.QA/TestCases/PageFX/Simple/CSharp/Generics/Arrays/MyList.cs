using System;

class List<T>
{
    private T[] _list;

    public List()
    {
        _list = new T[0];
    }

    public List(params T[] arr)
    {
        int n = arr.Length;
        _list = new T[n];
        for (int i = 0; i < n; ++i)
            _list[i] = arr[i];
    }

    public int Count
    {
        get { return _list.Length; }
    }

    public T this[int index]
    {
        get { return _list[index]; }
    }

    public void Add(T item)
    {
        int n = _list.Length;
        T[] arr = new T[n + 1];
        for (int i = 0; i < n; ++i)
            arr[i] = _list[i];
        arr[n] = item;
        _list = arr;
    }
}

class Test
{
    static void Test1()
    {
        Console.WriteLine("--- Test1");
        List<int> list = new List<int>();
        list.Add(10);
        list.Add(20);
        list.Add(30);
        for (int i = 0; i < list.Count; ++i)
            Console.WriteLine(list[i]);
    }

    static void Test2()
    {
        Console.WriteLine("--- Test2");
        List<int> list = new List<int>(10, 20, 30);
        for (int i = 0; i < list.Count; ++i)
            Console.WriteLine(list[i]);
    }

    static void Main()
    {
        Test1();
        Test2();
        Console.WriteLine("<%END%>");
    }
}