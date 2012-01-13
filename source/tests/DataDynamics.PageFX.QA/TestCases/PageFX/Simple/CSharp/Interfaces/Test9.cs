using System;

interface I
{
    int Count { get; }
}

class X : I
{
    #region I Members
    int I.Count
    {
        get { return 5; }
    }
    #endregion

    static void Main()
    {
        X x = new X();
        Console.WriteLine(((I)x).Count);
        Console.WriteLine("<%END%>");
    }
}