using System;

namespace DataDynamics.PageFX.CodeModel
{
	public static class JsExtensions
	{
		public static bool IsJsid(this char c, bool start)
		{
			return Char.IsLetter(c) || c == '_' || c == '$' || (!start && Char.IsDigit(c));
		}

		public static bool IsJsid(this string id)
		{
			if (string.IsNullOrEmpty(id))
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
	}
}