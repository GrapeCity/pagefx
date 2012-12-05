using System;

class Program
{
	const long minL = long.MinValue;
	const long maxL = long.MaxValue;

	const ulong minUL = ulong.MinValue;
	const ulong maxUL = ulong.MaxValue;

	static void TestToString()
	{
		Console.WriteLine("# TestToString");
		{
			long v = -11L;
			Console.WriteLine(v);
		}
		{
			ulong v = 11UL;
			Console.WriteLine(v);
		}

		Console.WriteLine(minL);
		Console.WriteLine(maxL);
		Console.WriteLine(minUL);
		Console.WriteLine(maxUL);
	}

    static void TestCompare()
    {
		Console.WriteLine("# TestCompare");
        ulong v = 11UL;
        Console.WriteLine(v < 10UL);
    }

    static void TestCast()
    {
		Console.WriteLine("# TestCast");
        {
            long a = -11L;
			Console.WriteLine(a);
			Console.WriteLine((ulong)a);
        }
        {
            ulong a = ulong.MaxValue;
            long b = (long)a;
            Console.WriteLine(a);
            Console.WriteLine(b);
        }
	    {
			long v = 45643271;
			Console.WriteLine((ulong)v);
	    }
    }

	static void TestShift()
	{
		Console.WriteLine("# TestShift");
		long v = 45643271;
		Console.WriteLine(v);
		v = v >> 10;
		Console.WriteLine(v);
	}

	static void TestNot()
	{
		Console.WriteLine("# TestNot");
		{
			long a = -11L;
			Console.WriteLine(~a);
		}
		{
			long a = 11L;
			Console.WriteLine(~a);
		}
	}

	static void TestNegate()
	{
		Console.WriteLine("# TestNegate");
		{
			long a = -11L;
			Console.WriteLine(-a);
		}
		{
			long a = 11L;
			Console.WriteLine(-a);
		}
		/*{
			ulong a = 11UL;
			Console.WriteLine(-a);
		}*/
	}

	static void TestAdd1()
	{
		Console.WriteLine("# TestAdd1");
		{
			ulong a = 11UL;
			ulong b = 10UL;
			Console.WriteLine(a + b);
		}
		{
			ulong a = uint.MaxValue;
			ulong b = 10UL;
			Console.WriteLine(a + b);
		}
	}

	static void TestAdd2()
	{
		Console.WriteLine("# TestAdd2");
		{
			long a = 11L;
			long b = 10L;
			Console.WriteLine(a + b);
		}
		{
			long a = -11L;
			long b = 10L;
			Console.WriteLine(a + b);
		}
		{
			long a = (long)uint.MaxValue;
			long b = 10L;
			Console.WriteLine(a + b);
		}
	}

	static void TestSub1()
	{
		Console.WriteLine("# TestSub1");
		{
			ulong a = 11UL;
			ulong b = 10UL;
			Console.WriteLine(a - b);
		}
		{
			ulong a = 0xfffffffff;
			ulong b = 11UL;
			Console.WriteLine(a - b);
		}
	}

	static void TestSub2()
	{
		Console.WriteLine("# TestSub2");
		{
			long a = 11L;
			long b = 10L;
			Console.WriteLine(a - b);
		}
		{
			long a = -11L;
			long b = 10L;
			Console.WriteLine(a - b);
		}
		{
			long a = 0xfffffffff;
			long b = 11L;
			Console.WriteLine(a - b);
		}
	}

	static void TestMul1()
	{
		Console.WriteLine("# TestMul1");
		{
			ulong a = 11UL;
			ulong b = 10UL;
			Console.WriteLine(a * b);
		}
		{
			ulong a = uint.MaxValue;
			ulong b = 10UL;
			Console.WriteLine(a * b);
		}
	}

	static void TestMul2()
	{
		Console.WriteLine("# TestMul2");
		{
			long a = 11L;
			long b = 10L;
			Console.WriteLine(a * b);
		}
		{
			long a = -11L;
			long b = 10L;
			Console.WriteLine(a * b);
		}
		{
			long a = int.MaxValue;
			long b = 10L;
			Console.WriteLine(a * b);
		}
	}

    static void TestDiv1()
    {
		Console.WriteLine("# TestDiv1");
        {
            ulong a = 11UL;
            ulong b = 10UL;
            Console.WriteLine(a / b);
        }
    }

	static void TestDiv2()
	{
		Console.WriteLine("# TestDiv2");
		{
			long a = 11L;
			long b = 10L;
			Console.WriteLine(a / b);
		}
		{
			long a = -11L;
			long b = 10L;
			Console.WriteLine(a / b);
		}
	}

    static void Main()
    {
		TestToString();
        TestCompare();
        TestCast();
		TestShift();
	    TestNot();
	    TestNegate();
		TestAdd1();
		TestAdd2();
		TestSub1();
		TestSub2();
        TestMul1();
        TestMul2();
		TestDiv1();
		TestDiv2();
        Console.WriteLine("<%END%>");
    }
}