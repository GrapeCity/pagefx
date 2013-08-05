using System;
using System.Text;
using DataDynamics.PageFX.Common.Utilities;

namespace DataDynamics.PageFX.Flash.Core.CodeProvider
{
    internal class DebugUtils
    {
        const string prefix = "FE";

        public static string Encode(string path)
        {
            var b = Encoding.UTF8.GetBytes(path);
            var sb = new StringBuilder();
            sb.Append(prefix);
            Hex.Append(sb, b, true);
            return sb.ToString();
        }

        public static string Decode(string s)
        {
            int n = s.Length;
            if (n < 2)
                throw new ArgumentException("Invalid string");
            if (!s.StartsWith(prefix))
                throw new ArgumentException("Invalid string");
            if ((n & 1) != 0)
                throw new ArgumentException("Invalid string");
            var b = new byte[(n - prefix.Length) / 2];
            for (int i = prefix.Length, j = 0; i < n; i += 2)
            {
                b[j++] = Hex.ToByte(s[i], s[i + 1]);
            }
            return Encoding.UTF8.GetString(b);
        }
    }
}