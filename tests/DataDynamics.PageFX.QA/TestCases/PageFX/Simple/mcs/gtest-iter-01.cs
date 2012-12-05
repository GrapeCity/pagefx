using System;
using System.Collections.Generic;

class C
{
	class ArrayReadOnlyList<T>
	{
		T [] array;
		bool is_value_type;

		public ArrayReadOnlyList ()
		{
		}

        public ArrayReadOnlyList(T[] arr)
        {
            array = arr;
        }

		public T this [int index]
		{
			get
			{
				return array [index];
			}
		}

		public IEnumerator<T> GetEnumerator ()
		{
			for (int i = 0; i < array.Length; i++)
				yield return array [i];
		}

        public void Print()
        {
            foreach (var v in this)
                Console.WriteLine(v);
        }
	}

    static void Test1()
    {
        Console.WriteLine("--- Test1");
        var l = new ArrayReadOnlyList<int>(new [] { 10, 20 , 30});
        l.Print();
    }

	static void Main ()
	{
        Console.WriteLine("<%END%>");
	}
}