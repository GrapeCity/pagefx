using System;

class X
{
    private static readonly ulong[] ULongList = new ulong[]
                {
                    1,
                    10,
                    100,
                    1000,
                    10000,
                    100000,
                    1000000,
                    10000000,
                    100000000,
                    1000000000,
                    10000000000,
                    100000000000,
                    1000000000000,
                    10000000000000,
                    100000000000000,
                    1000000000000000,
                    10000000000000000,
                    100000000000000000,
                    1000000000000000000,
                    10000000000000000000,
                };

    private static int GetMaxIndex(ulong v)
    {
        if (v < 10UL) return 0;
        if (v < 100UL) return 1;
        if (v < 1000UL) return 2;
        if (v < 10000UL) return 3;
        if (v < 100000UL) return 4;
        if (v < 1000000UL) return 5;
        if (v < 10000000UL) return 6;
        if (v < 100000000UL) return 7;
        if (v < 1000000000UL) return 8;
        if (v < 10000000000UL) return 9;
        if (v < 100000000000UL) return 10;
        if (v < 1000000000000UL) return 11;
        if (v < 10000000000000UL) return 12;
        if (v < 100000000000000UL) return 13;
        if (v < 1000000000000000UL) return 14;
        if (v < 10000000000000000UL) return 15;
        if (v < 100000000000000000UL) return 16;
        if (v < 1000000000000000000UL) return 17;
        if (v < 10000000000000000000UL) return 18;
        return 19;
    }

    static void InitDigits(ulong value)
    {
        int i = GetMaxIndex(value);
        int j = 0;

        do
        {
            ulong b = ULongList[i];
            ulong n = value / b;
            j++;
            Console.WriteLine((byte)n);
            value -= b * n;
            i--;
        } while (i >= 0);

        Console.WriteLine(j);
    }

    static void Main()
    {
        int n = ULongList.Length;
        Console.WriteLine(n);

        InitDigits(10);
        Console.WriteLine("<%END%>");
    }
}