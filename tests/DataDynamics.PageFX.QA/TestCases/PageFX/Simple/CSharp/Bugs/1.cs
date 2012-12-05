using System;

internal class Test
{
    private class Stack
    {
        private int[] _array;
        int _size;
        int _version;

        const int INITIAL_SIZE = 16;

        private static void Resize(ref int[] arr, int size)
        {
            arr = new int[size];
        }

        public void Push(int t)
        {
            if (_size == 0 || _size == _array.Length)
                Resize(ref _array, _size == 0 ? INITIAL_SIZE : 2 * _size);

            _version++;

            _array[_size++] = t;
        }
    }

    static void Test1()
    {
        Console.WriteLine("--- Test1");
        Stack stack = new Stack();
        stack.Push(10);
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}