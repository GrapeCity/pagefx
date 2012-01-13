using System;

class NumberStore
{
    bool _NaN;
    bool _infinity;
    bool _positive;
    int _decPointPos;
    int _defPrecision;
    int _defMaxPrecision;
    readonly int _defByteSize;
    byte[] _digits;

    static readonly uint[] IntList = new uint[] {
				1,
				10,
				100,
				1000,
				10000,
				100000,
				1000000,
				10000000,
				100000000,
				1000000000,
			};

    public NumberStore(int value)
    {
        _infinity = _NaN = false;
        _defByteSize = 4;
        _defMaxPrecision = _defPrecision = 10;
        _positive = value >= 0;

        if (value == 0)
        {
            _digits = new byte[] { 0 };
            _decPointPos = 1;
            return;
        }

        uint v = (uint)(_positive ? value : -value);

        int i = 9, j = 0;

        if (v < 10)
            i = 0;
        else if (v < 100)
            i = 1;
        else if (v < 1000)
            i = 2;
        else if (v < 10000)
            i = 3;
        else if (v < 100000)
            i = 4;
        else if (v < 1000000)
            i = 5;
        else if (v < 10000000)
            i = 6;
        else if (v < 100000000)
            i = 7;
        else if (v < 1000000000)
            i = 8;
        else
            i = 9;

        _digits = new byte[i + 1];
        do
        {
            uint n = v / IntList[i];
            _digits[j++] = (byte)n;
            v -= IntList[i--] * n;
        } while (i >= 0);

        _decPointPos = _digits.Length;
    }

    public bool IsNaN
    {
        get { return _NaN; }
    }
    public bool IsInfinity
    {
        get { return _infinity; }
    }
    public int DecimalPointPosition
    {
        get { return _decPointPos; }
    }
    public bool Positive
    {
        get { return _positive; }
        set { _positive = value; }
    }

    public int DefaultPrecision
    {
        get { return _defPrecision; }
    }

    public bool IsDecimalSource
    {
        get { return _defPrecision > 30; }
    }

    public int IntegerDigits
    {
        get { return _decPointPos > 0 ? _decPointPos : 1; }
    }

    public void Multiply10(int count)
    {
        if (count <= 0) return;
        _decPointPos += count;
    }

    public void Divide10(int count)
    {
        if (count <= 0) return;
        _decPointPos -= count;
    }

    public char GetChar(int pos)
    {
        if (_decPointPos <= 0)
            pos += _decPointPos - 1;

        if (pos < 0 || pos >= _digits.Length)
            return '0';
        else
            return (char)('0' + _digits[pos]);
    }
}

class Test6
{
    static void Format(NumberStore ns, int precision)
    {
        precision = precision > 0 ? precision : ns.DefaultPrecision;

        int exponent = 0;
        bool expMode = (ns.IsDecimalSource && precision == ns.DefaultPrecision ? false : (ns.IntegerDigits > precision || ns.DecimalPointPosition <= -4));
        if (expMode)
        {
            while (!(ns.DecimalPointPosition == 1 && ns.GetChar(0) != '0'))
            {
                if (ns.DecimalPointPosition > 1)
                {
                    ns.Divide10(1);
                    exponent++;
                }
                else
                {
                    ns.Multiply10(1);
                    exponent--;
                }
            }
        }
    }

    static void f(int value)
    {
        NumberStore ns = new NumberStore(value);
        Format(ns, -1);
    }

    static void Main()
    {
        for (int i = 0; i < 1000; i += 10)
            f(i);
        Console.WriteLine("<%END%>");
    }
}