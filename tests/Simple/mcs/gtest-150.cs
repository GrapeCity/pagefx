﻿using System;

class Test
{

    public interface IOp<T>
    {
        T Func(uint v);
    }

    public struct Op : IOp<ushort>, IOp<uint>
    {
        ushort IOp<ushort>.Func(uint v) { return (ushort)(v * 2); }
        uint IOp<uint>.Func(uint v) { return v * 4; }
    }

    static void Foo<T, OP>(uint v)
        where T : struct
        where OP : IOp<T>
    {
        OP op = default(OP);
        System.Console.WriteLine(op.Func(v));
    }

    static void Test1()
    {
        Console.WriteLine("----Test1");
        Foo<ushort, Op>(100);
        Foo<uint, Op>(100);
    }

    static public void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}
