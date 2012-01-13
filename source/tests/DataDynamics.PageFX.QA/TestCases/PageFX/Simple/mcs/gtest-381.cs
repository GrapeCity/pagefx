using System;
using System.Collections.Generic;

class TestGoto
{
    static int x = 2;

    static void Main()
    {
        try
        {
            foreach (bool b in test())
                ;
            if (x != 0)
                throw new Exception();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
        }
        Console.WriteLine("<%END%>");
    }

    static IEnumerable<bool> setX()
    {
        x = 1;
        try
        {
            yield return true;
        }
        finally
        {
            x = 0;
        }
    }

    static IEnumerable<bool> test()
    {
        foreach (bool b in setX())
        {
            yield return true;
            // Change "goto label" to "break" to show the correct result.
            goto label;
        }
    label:
        yield break;
    }
}
