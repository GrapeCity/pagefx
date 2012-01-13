namespace DataDynamics
{
    public static class FloatHelper
    {
        public static ushort ToUInt16(float v, int q)
        {
            return (ushort)(v * (1 << q));
        }

        public static short ToInt16(float v, int q)
        {
            return (short)(v * (1 << q));
        }

        public static uint ToUInt32(float v, int q)
        {
            return (uint)(v * (1 << q));
        }

        public static int ToInt32(float v, int q)
        {
            return (int)(v * (1 << q));
        }

        public static float ToSingle(int value, int q)
        {
            return (float)value / (1 << q);
        }

        public static int ToFixed32(float value)
        {
            return (int)(value * 65536);
        }

        public static int ToFixed16(float value)
        {
            return (int)(value * 256);
        }
    }
}