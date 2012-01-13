using System;
using System.Collections.Generic;

class X
{
    class Pool<T>
    {
        readonly List<T> _values = new List<T>();
        readonly Dictionary<T, int> _dic = new Dictionary<T, int>();
        readonly int _startIndex;

        public Pool()
        {
        }

        public Pool(int startIndex)
        {
            _startIndex = startIndex;
        }

        public int Count
        {
            get { return _values.Count; }
        }

        public T this[int index]
        {
            get { return _values[index - _startIndex]; }
        }

        public int Define(T value)
        {
            int i;
            if (!_dic.TryGetValue(value, out i))
            {
                i = _startIndex + _dic.Count;
                _dic.Add(value, i);
                _values.Add(value);
            }
            return i;
        }
    }

    static void Test1()
    {
        Console.WriteLine("--Test1");
        var ints = new Pool<int>();
        Console.WriteLine(ints.Define(10));
        Console.WriteLine(ints.Define(20));
        Console.WriteLine(ints.Define(30));
        
        var uints = new Pool<uint>();
        Console.WriteLine(uints.Define(10));
        Console.WriteLine(uints.Define(20));
        Console.WriteLine(uints.Define(30));
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}