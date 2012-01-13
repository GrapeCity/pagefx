using System;
using System.Drawing;

namespace DataDynamics
{
    public static class ColorHelper
    {
        static readonly Random random = new Random();

        public static Color Random()
        {
            int r = random.Next(0, 255);
            int g = random.Next(0, 255);
            int b = random.Next(0, 255);
            return Color.FromArgb(r, g, b);
        }

        public static bool TryParse(string s, ref Color color)
        {
            if (string.IsNullOrEmpty(s)) return false;
            s = s.Trim();
            if (string.IsNullOrEmpty(s)) return false;

            if (s[0] == '#')
            {
                s = s.Substring(1);
                if (string.IsNullOrEmpty(s)) return false;
                if (!ParseHelper.IsHex(s)) return false;
                int n = s.Length;
                switch (n)
                {
                    case 6:
                        {
                            int r = ParseHelper.ParseHex(s.Substring(0, 2));
                            int g = ParseHelper.ParseHex(s.Substring(2, 2));
                            int b = ParseHelper.ParseHex(s.Substring(4, 2));
                            color = Color.FromArgb(r, g, b);
                        }
                        return true;

                    case 8:
                        {
                            int a = ParseHelper.ParseHex(s.Substring(0, 2));
                            int r = ParseHelper.ParseHex(s.Substring(2, 2));
                            int g = ParseHelper.ParseHex(s.Substring(4, 2));
                            int b = ParseHelper.ParseHex(s.Substring(6, 2));
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