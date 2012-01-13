using System;
using Avm;
using __AS3__.vec;

class X
{
    static void Main()
    {
        Vector<double> v1 = new Vector<double>();
        v1.push(10, 20, 30);
        Vector<double> v2 = new Vector<double>();
        v2.push(100, 200, 300);

        Vector<Vector<double>> vv = new Vector<Vector<double>>();
        vv.push(v1, v2);

        for (int i = 0; i < (int)vv.length; ++i)
        {
            Vector<double> v = vv[i];
            for (int j = 0; j < (int)v.length; ++j)
                Console.WriteLine(v[j]);
        }
        Console.WriteLine("<%END%>");
    }
}