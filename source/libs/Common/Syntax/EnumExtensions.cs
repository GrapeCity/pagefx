using System;
using System.Collections;
using System.Reflection;

namespace DataDynamics.PageFX.Common.Syntax
{
	public static class EnumExtensions
	{
		private static Hashtable _enumCache;

		private static Hashtable LoadEnumCache<T>()
		{
			var cache = new Hashtable();
			var bf = BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField;
			var fields = typeof(T).GetFields(bf);
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

		public static string EnumString<T>(this T value, string lang) where T:struct
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
			if (String.IsNullOrEmpty(res))
				return String.Empty;
			return res;
		}
	}
}