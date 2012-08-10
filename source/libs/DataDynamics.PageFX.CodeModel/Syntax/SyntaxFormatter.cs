using System;

namespace DataDynamics.PageFX.CodeModel.Syntax
{
	public static class SyntaxFormatter
    {
        public const string DefaultLanguage = "c#";

        public static string ToString(string lang, Visibility v)
        {
            return v.EnumString(lang);
        }

        public static string ToString(Visibility v)
        {
            return ToString(DefaultLanguage, v);
        }

        public static string Format(ICodeNode node, string format, IFormatProvider formatProvider)
        {
            var writer = new SyntaxWriter();
            writer.Write(node, format);
            return writer.ToString();
        }
    }
}