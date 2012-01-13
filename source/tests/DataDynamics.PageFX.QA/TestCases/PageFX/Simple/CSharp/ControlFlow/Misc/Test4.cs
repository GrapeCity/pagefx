using System;
using System.Text;

class Test4
{
    static readonly char[] digitLowerTable = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
    //static readonly char[] digitUpperTable = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

    static void f(int exponent, bool upper, bool expMode)
    {
        StringBuilder cb = new StringBuilder();
        if (expMode)
        {
            if (upper)
                cb.Append('E');
            else
                cb.Append('e');

            if (exponent >= 0)
                cb.Append("+");
            else
            {
                cb.Append("-");
                exponent = -exponent;
            }

            if (exponent == 0)
            {
                cb.Append('0', 2);
            }
            else if (exponent < 10)
            {
                cb.Append('0');
                cb.Append(digitLowerTable[exponent]);
            }
            else if (exponent < 100)
            {
                cb.Append(digitLowerTable[exponent / 10 % 10]);
                cb.Append(digitLowerTable[exponent % 10]);
            }
            else if (exponent < 1000)
            {
                cb.Append(digitLowerTable[exponent / 100 % 10]);
                cb.Append(digitLowerTable[exponent / 10 % 10]);
                cb.Append(digitLowerTable[exponent % 10]);
            }
        }
        Console.WriteLine(cb.ToString());
    }

    static void Main()
    {
        f(0, true, true);
        f(0, false, true);
        for (int e = 1; e <= 5000; e *= 5)
        {
            f(e, true, true);
            f(e, false, true);
        }
        Console.WriteLine("<%END%>");
    }
}