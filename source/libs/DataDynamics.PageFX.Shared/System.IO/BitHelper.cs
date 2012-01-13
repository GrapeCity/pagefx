using DataDynamics;

namespace System
{
    public static class BitHelper
    {
        public static int URShift(int x, int y)
        {
            return (int)(((uint)x) >> y);
        }

        public static int GetBytes(int value)
        {
            if (value <= 0xFF) return 1;
            if (value <= 0xFFFF) return 2;
            if (value <= 0xFFFFFF) return 3;
            return 4;
        }

        public static bool BytesEqual(int x, int y)
        {
            return GetBytes(x) == GetBytes(y);
        }

        public static int GetMinBits(byte value)
        {
            return GetMinBits((uint)value);
        }

        public static int GetMinBits(sbyte value)
        {
            return GetMinBits((uint)Math.Abs(value)) + 1;
        }

        public static int GetMinBits(ushort value)
        {
            return GetMinBits((uint)value);
        }

        public static int GetMinBits(short value)
        {
            return GetMinBits((uint)Math.Abs(value)) + 1;
        }

        public static int GetMinBits(uint value)
        {
            return 32 - Tricks.Nlz(value);
        }

        public static int GetMinBits(int value)
        {
            uint v = (uint)value;
            if (value >= 0)
                return GetMinBits(v) + 1;
            v = ~v;
            int n = Tricks.Nlz(v);
            return 33 - n;
        }

        public static int GetMinBits(params int[] v)
        {
            int bits = GetMinBits(v[0]);
            int n = v.Length;
            for (int i = 1; i < n; ++i)
            {
                int n2 = GetMinBits(v[i]);
                if (n2 > bits) bits = n2;
            }
            return bits;
        }

        public static int GetMinBits(params uint[] v)
        {
            int bits = GetMinBits(v[0]);
            int n = v.Length;
            for (int i = 1; i < n; ++i)
            {
                int n2 = GetMinBits(v[i]);
                if (n2 > bits) bits = n2;
            }
            return bits;
        }

        public static int GetMinBits(params short[] v)
        {
            int bits = GetMinBits(v[0]);
            int n = v.Length;
            for (int i = 1; i < n; ++i)
            {
                int n2 = GetMinBits(v[i]);
                if (n2 > bits) bits = n2;
            }
            return bits;
        }

        public static int GetMinBits(params ushort[] v)
        {
            int bits = GetMinBits(v[0]);
            int n = v.Length;
            for (int i = 1; i < n; ++i)
            {
                int n2 = GetMinBits(v[i]);
                if (n2 > bits) bits = n2;
            }
            return bits;
        }

        public static int GetMinBits(int q, params float[] nums)
        {
            int n = nums.Length;
            var vals = new short[n];
            for (int i = 0; i < n; ++i)
                vals[i] = FloatHelper.ToInt16(nums[i], q);
            return GetMinBits(vals);
        }
    }
}