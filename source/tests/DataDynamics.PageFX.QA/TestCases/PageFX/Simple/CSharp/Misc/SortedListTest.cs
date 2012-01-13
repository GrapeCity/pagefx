using System;
using System.Collections;

class SortedListTest
{
    static void PrintSL(SortedList sl)
    {
        foreach (DictionaryEntry e in sl)
        {
            Console.WriteLine(e.Key);
            Console.WriteLine(" ");
            Console.WriteLine(e.Value);
            Console.WriteLine();
        }
    }

    static void TestDefaultComparer()
    {
        IComparer cmp = Comparer.Default;
        Console.WriteLine(cmp.Compare("key11", "key1"));
        Console.WriteLine(cmp.Compare("key11", "key11"));
        Console.WriteLine(cmp.Compare("key11", "key2"));
        Console.WriteLine(cmp.Compare("key11", "key12"));
    }

    static void TestMisc()
    {
        int v = 10;
        Console.WriteLine(~v);
        v = -1;
        Console.WriteLine(~v);
    }

    static void TestAdd()
    {
        SortedList sl1 = new SortedList();
        for (int i = 0; i < 15; ++i)
        {
            string key = "key" + i;
            sl1.Add(key, i);
        }
        PrintSL(sl1);
    }

    static void TestClone()
    {
        SortedList sl1 = new SortedList();
        for (int i = 0; i < 15; ++i)
            sl1.Add("key" + i, i);
        PrintSL(sl1);
        SortedList sl2 = (SortedList)sl1.Clone();
        PrintSL(sl2);
    }

    static void Main()
    {
        TestDefaultComparer();
        TestMisc();
        TestAdd();
        TestClone();
        Console.WriteLine("<%END%>");
    }
}