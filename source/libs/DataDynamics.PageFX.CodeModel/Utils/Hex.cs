using System.Text;

namespace DataDynamics
{
    public static class Hex
    {
        public static int ToDecimal(int c)
        {
            if (c >= '0' && c <= '9')
                return c - '0';
            if (c >= 'A' && c <= 'F')
                return c - 'A' + 10;
            if (c >= 'a' && c <= 'f')
                return c - 'a' + 10;
            return 0;
        }

        public static int ToDecimal(string s)
        {
            int n = s.Length;
            int b = 1 << (4 * (n - 1));
            int res = 0;
            for (int i = 0; i < n; ++i)
            {
                int c = s[i];
                c = ToDecimal(c);
                res += c * b;
                b >>= 4;
            }
            return res;
        }

        public static byte ToByte(int ch, int cl)
        {
            int h = ToDecimal(ch);
            int l = ToDecimal(cl);
            return (byte)((h << 4) | l);
        }

        public static string ToString(byte[] data, bool upperCase)
        {
            if (data == null) return "";
            var sb = new StringBuilder();
            Append(sb, data, upperCase);
            return sb.ToString();
        }

        public static void Append(StringBuilder sb, byte[] data, bool upperCase)
        {
            if (data != null)
            {
                int n = data.Length;
                string format = upperCase ? "X2" : "x2";
                for (int i = 0; i < n; ++i)
                    sb.Append(data[i].ToString(format));
            }
        }
    }
}