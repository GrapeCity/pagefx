using System;

class StaticCallTest
{
    static bool f1()
    {
        return false;
    }

    static bool f2()
    {
        return true;
    }

    static bool f3(int n)
    {
        return n > 0;
    }

    static void Test1()
    {
        {
            bool a = f1();
            bool b = f2();
            bool c = f3(10);
            //Console.WriteLine("hello");
            Console.WriteLine(a && f3(-10) ? (b && c ? "a" : "b") : "c");
        }
    }

    static void Assert(string msg, bool cond)
    {
        Console.WriteLine(msg);
        Console.WriteLine(cond);
    }

    static void Test2()
    {
        Random r = new Random(42);
        for (int i = 0; i < 10; i++)
        {
            int c = r.Next(10);
            Assert("NextMax(" + i + ")", c < 10 && c >= 0);
        }
    }

    static string FormatFlags(int[] values, string[] names, int flags)
    {
        string s = "";
        int n = values.Length;
        for (int i = n - 1; i >= 0; i--)
        {
            int enumValue = values[i];
            if (enumValue == 0) continue;

            if ((flags & enumValue) == enumValue)
            {
                if (s.Length > 0)
                    s = names[i] + ", " + s;
                else
                    s = names[i];
                flags -= enumValue;
            }
        }
        return s;
    }

    static void Test3()
    {
        int[] vals = new int[] { 0, 1, 2, 4 };
        string[] names = new string[] { "None", "A", "B", "C" };
        string s = FormatFlags(vals, names, 3);
        Console.WriteLine(s);
    }

    private static int _ParseNumber(string s, int valuePos,
                                        int min_digits,
                                        int digits,
                                        bool leadingzero,
                                        bool sloppy_parsing,
                                        out int num_parsed)
    {
        num_parsed = 0;
        return -1;
    }

    static bool Test4(string s, int num, bool exact)
    {
        bool sloppy_parsing = false;
        bool longYear = false;
        int num_parsed = 0;
        int valuePos = 0;
        int year = 0;
        int month = 0;

        for (; valuePos < s.Length; valuePos++)
        {
            switch (s[valuePos])
            {
                case 'M':
                    if (month != -1)
                        return false;

                    year = _ParseNumber(s, valuePos, exact ? 4 : 3, 4, false, sloppy_parsing, out num_parsed);
                    break;

                case 'y':

                    if (year != -1)
                        return false;

                    if (num == 0)
                    {
                        year = 1;
                    }
                    else if (num < 3)
                    {
                        year = 2;
                    }
                    else
                    {
                        year = _ParseNumber(s, valuePos, exact ? 4 : 3, 4, false, sloppy_parsing, out num_parsed);
                        if ((year >= 1000) && (num_parsed == 4) && (!longYear) && (s.Length > 4 + valuePos))
                        {
                            int np = 0;
                            int ly = _ParseNumber(s, valuePos, 5, 5, false, sloppy_parsing, out np);
                            longYear = (ly > 9999);
                        }
                        num = 3;
                    }

                    //FIXME: We should do use dfi.Calendat.TwoDigitYearMax
                    if (num_parsed <= 2)
                        year += (year < 30) ? 2000 : 1900;

                    break;
            }
        }

        Console.WriteLine(year);

        return true;
    }

    static void Main()
    {
        Test1();
        Test2();
        Test3();
        Test4("aaa", 3, true);
        Console.WriteLine("<%END%>");
    }
}