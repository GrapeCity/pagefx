namespace System.Drawing.Flash
{
    class ColorHelper
    {
        public static Color FromFlash(double alpha, uint color)
        {
            return Color.FromArgb(
                (int)(alpha * 255),
                (int)((color >> 16) & 0xff),
                (int)((color >> 8) & 0xff),
                (int)(color & 0xff));
        }

        public static uint ToFlash(Color value)
        {
            return ((uint)value.R << 16) | ((uint)value.G << 8) | ((uint)value.B);
        }
    }
}