using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.Extensions;

namespace DataDynamics.PageFX.Common.CodeModel
{
	public static class VariableExtensions
	{
		public static void UnifyAndNormalizeNames(this IEnumerable<IVariable> vars)
		{
			var list = vars.ToList();
			foreach (var v in list)
			{
				string name = NormalizeName(v.Name);
				if (name != v.Name)
				{
					v.Name = UnifyName(v, list, name);
				}
			}
		}

		private static string UnifyName(IVariable var, IList<IVariable> vars, string name)
		{
			if (vars.FirstOrDefault(x => !ReferenceEquals(x, var) && x.Name == name) == null)
				return name;

			int n = 1;
			string original = name;
			do
			{
				name = original + n++;
			} while (vars.FirstOrDefault(x => !ReferenceEquals(x, var) && x.Name == name) != null);

			return name;
		}

		private static string NormalizeName(string name)
		{
			return name.Select(
				(c, i) => Char.IsLetter(c) || c == '_' || (i != 0 && Char.IsDigit(c))
					          ? c.ToString()
					          : "x" + ((int)c).ToString("X"))
			           .Join("");
		}
	}
}