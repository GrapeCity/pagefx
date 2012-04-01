using System;
using System.Collections.Generic;
using System.Reflection;

namespace DataDynamics.PageFX.FLI.SWF
{
	internal static class EnumReflector
	{
		public static Dictionary<TEnum, TValue> GetAttributeMap<TEnum, TValue, TAttr>(Converter<TAttr, TValue> converter)
			where TAttr : Attribute
		{
			var fields = typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField);
			var map = new Dictionary<TEnum, TValue>();
			foreach (var fi in fields)
			{
				var value = (TEnum)fi.GetValue(null);
				var attr = fi.GetAttribute<TAttr>(false);
				if (attr != null)
				{
					map.Add(value, converter(attr));
				}
			}
			return map;
		}
	}
}