namespace System.Drawing
{
    static class FloatHelper
    {
        public static bool NearZero(float value)
        {
            return ((value >= -0.0001f) && (value <= 0.0001f));
        }

        public static bool NearOne(float value)
        {
            return ((value >= 0.9999f) && (value <= 1.0001f));
        }
    }
}