using System;
using System.Collections;
using System.Reflection;

namespace DataDynamics.PageFX.CodeModel.Syntax
{
    public static class SyntaxFormatter
    {
        #region Format Enum
        private static Hashtable _enumCache;

        private static Hashtable LoadEnumCache<T>()
        {
            var cache = new Hashtable();
            const BindingFlags publicStaticField = BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField;
            var fields = typeof(T).GetFields(publicStaticField);
            foreach (var field in fields)
            {
                var v = (T)field.GetValue(null);
                var attrs = (LanguageAttribute[])field.GetCustomAttributes(typeof(LanguageAttribute), false);
            	foreach (var attr in attrs)
            	{
            		var langCache = (Hashtable)cache[attr.Language];
            		if (langCache == null)
            		{
            			langCache = new Hashtable();
            			cache[attr.Language] = langCache;
            		}
            		langCache[v] = attr.Value;
            	}
            }
            return cache;
        }

        public static string FormatEnum<T>(string lang, T value)
        {
            if (_enumCache == null)
            {
                _enumCache = new Hashtable();
            }

            string key = typeof(T).FullName;
            var cache = (Hashtable)_enumCache[key];
            if (cache == null)
            {
                cache = LoadEnumCache<T>();
                _enumCache[key] = cache;
            }

            var langCache = (Hashtable)cache[lang];
            if (langCache == null)
                return value.ToString();

            string res = langCache[value] as string;
            if (string.IsNullOrEmpty(res))
                return string.Empty;
            return res;
        }
        #endregion

        public const string DefaultLanguage = "c#";

        public static string ToString(string lang, Visibility v)
        {
            return FormatEnum(lang, v);
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