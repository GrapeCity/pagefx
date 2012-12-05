using System;
using System.Collections.Generic;

class X
{
    public IEnumerable<int> Test(int a, long b)
    {
        while (a < b)
        {
            a++;
            yield return a;
        }
    }
    
    static int Test1()
    {
        X x = new X();
        int sum = 0;
        foreach (int i in x.Test(3, 8L))
            sum += i;

        return sum == 30 ? 0 : 1;
    }

    static void Main()
    {
        Console.WriteLine(Test1());
        Console.WriteLine("<%END%>");
    }
}
