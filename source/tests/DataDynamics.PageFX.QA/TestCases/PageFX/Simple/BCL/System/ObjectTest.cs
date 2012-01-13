using System;

class A : ICloneable
{
    public int Value
    {
        get { return _value; }
        set { _value = value; }
    }
    private int _value;

    public object Clone()
    {
        return MemberwiseClone();
    }
}

class ObjectTest
{
    static void TestMemberwiseClone()
    {
        A a = new A();
        a.Value = 100;
        A b = (A)a.Clone();
        Console.WriteLine(a.Value == b.Value);
    }

    static void Main()
    {
        TestMemberwiseClone();
        Console.WriteLine("<%END%>");
    }
}