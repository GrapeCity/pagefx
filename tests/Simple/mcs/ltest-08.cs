

using System;
using System.Linq;
using System.Collections.Generic;

public class C
{
	static void Test<T, R> (Func<T, R> d)
	{
	}
	
    static int Test1()
    {
        Test((int x) => { return x + 1; });

        int[] source = new int[] { 2, 1, 0 };
        IEnumerable<int> e = source.Where((i) => i == 0).Select((i) => i + 1);

        if (e.ToList()[0] != 1)
            return 1;

        e = source.Where((int i) => i == 0).Select((int i) => i + 1);

        if (e.ToList()[0] != 1)
            return 2;

        e = source.Where(delegate(int i) { return i == 0; }).Select(delegate(int i) { return i + 1; });

        if (e.ToList()[0] != 1)
            return 3;

        return 0;
    }

	public static void Main ()
	{
	    Console.WriteLine(Test1());
        Console.WriteLine("<%END%>");
	}
}
