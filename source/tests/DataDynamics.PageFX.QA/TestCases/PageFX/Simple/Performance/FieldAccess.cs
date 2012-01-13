using System;

class FieldAccess
{
    class A
    {
        public int n;
        public string s;

        public int Number
        {
            get { return n; }
            set { n = value; }
        }

        public string Text
        {
            get { return s; }
            set { s = value; }
        }
    }

    static void Main()
    {
        int start = Environment.TickCount;
        A obj = new A();
        int n = 10000;
        int r = 0;
        string s = "";
        for (int i = 0; i < n; ++i)
        {
            obj.n = i;
            //obj.s = i.ToString();
        }
        for (int i = 0; i < n; ++i)
        {
            r += obj.Number;
            //s += obj.Text;
        }
        int end = Environment.TickCount;
        Console.WriteLine(end - start);
    }
}