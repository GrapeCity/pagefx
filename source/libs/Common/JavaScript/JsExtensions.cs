using System;
using System.Globalization;
using System.Linq;
using DataDynamics.PageFX.Common.Extensions;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Common.JavaScript
{
	public static class JsExtensions
	{
		public static bool IsJsid(this char c, bool start)
		{
			return Char.IsLetter(c) || c == '_' || c == '$' || (!start && Char.IsDigit(c));
		}

		public static bool IsJsid(this string id)
		{
			if (String.IsNullOrEmpty(id))
				return false;

			for (int i = 0; i < id.Length; i++)
			{
				if (!id[i].IsJsid(i == 0))
				{
					return false;
				}
			}

			return true;
		}

		public static string ToValidId(this string s, Runtime runtime)
		{
			if (runtime == Runtime.Avm) return s;

			if (s.IndexOf((c, i) => !c.IsJsid(i == 0)) >= 0)
			{
				return s.Select((c, j) =>
					{
						if (c.IsJsid(j == 0)) return c.ToString(CultureInfo.InvariantCulture);
						if (c == '.') return "$";
						return "x" + ((int)c).ToString("x2");
					}).ToArray().Join("");
			}

			return s;
		}
	}
}