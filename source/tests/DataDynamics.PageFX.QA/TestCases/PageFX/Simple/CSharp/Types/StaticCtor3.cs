using System;

class HTable
{
    private static readonly int[] primeTbl = {
			11,
			19,
			37,
			73,
			109,
			163,
			251,
			367,
			557,
			823,
			1237,
			1861,
			2777,
			4177,
			6247,
			9371,
			14057,
			21089,
			31627,
			47431,
			71143,
			106721,
			160073,
			240101,
			360163,
			540217,
			810343,
			1215497,
			1823231,
			2734867,
			4102283,
			6153409,
			9230113,
			13845163
		};

    internal static bool TestPrime(int x)
    {
        if ((x & 1) != 0)
        {
            int top = (int)Math.Sqrt(x);

            for (int n = 3; n < top; n += 2)
            {
                if ((x % n) == 0)
                    return false;
            }
            return true;
        }
        // There is only one even prime - 2.
        return (x == 2);
    }

    internal static int CalcPrime(int x)
    {
        for (int i = (x & (~1)) - 1; i < Int32.MaxValue; i += 2)
        {
            if (TestPrime(i)) return i;
        }
        return x;
    }

    internal static int ToPrime(int x)
    {
        for (int i = 0; i < primeTbl.Length; i++)
        {
            if (x <= primeTbl[i])
                return primeTbl[i];
        }
        return CalcPrime(x);
    }

    public HTable()
    {
        _size = ToPrime(100);
    }

    public int Size
    {
        get { return _size; }
    }
    private readonly int _size;
}

class PropCollection : HTable
{
}

class X
{
    static void Main()
    {
        PropCollection c = new PropCollection();
        Console.WriteLine(c.Size);
        Console.WriteLine("<%END%>");
    }
}