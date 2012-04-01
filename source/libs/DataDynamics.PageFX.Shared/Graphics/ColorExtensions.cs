using System.Drawing;

namespace DataDynamics
{
    public static class ColorExtensions
    {
        public static bool TryParseColor(this string s, ref Color color)
        {
            if (string.IsNullOrEmpty(s)) return false;
            s = s.Trim();
            if (string.IsNullOrEmpty(s)) return false;

            if (s[0] == '#')
            {
                s = s.Substring(1);
                if (string.IsNullOrEmpty(s)) return false;
                if (!s.IsHexString()) return false;
                int n = s.Length;
                switch (n)
                {
                    case 6:
                        {
                            int r = s.Substring(0, 2).ParseHexInt32();
                            int g = s.Substring(2, 2).ParseHexInt32();
                            int b = s.Substring(4, 2).ParseHexInt32();
                            color = Color.FromArgb(r, g, b);
                        }
                        return true;

                    case 8:
                        {
                            int a = s.Substring(0, 2).ParseHexInt32();
                            int r = s.Substring(2, 2).ParseHexInt32();
                            int g = s.Substring(4, 2).ParseHexInt32();
                            int b = s.Substring(6, 2).ParseHexInt32();
                            color = Color.FromArgb(a, r, g, b);
                        }
                        return true;

                    case 3:
                        {
                            int r = Hex.ToDecimal(s[0]);
                            int g = Hex.ToDecimal(s[1]);
                            int b = Hex.ToDecimal(s[2]);
                            color = Color.FromArgb(r | (r << 4), g | (g << 4), b | (b << 4));
                        }
                        return true;

                    case 4:
                        {
                            int a = Hex.ToDecimal(s[0]);
                            int r = Hex.ToDecimal(s[1]);
                            int g = Hex.ToDecimal(s[2]);
                            int b = Hex.ToDecimal(s[3]);
                            color = Color.FromArgb(a | (a << 4), r | (r << 4), g | (g << 4), b | (b << 4));
                        }
                        return true;
                }
            }
            else
            {
                //TODO: Parse rgb() function
            }
            return false;
        }
    }
}