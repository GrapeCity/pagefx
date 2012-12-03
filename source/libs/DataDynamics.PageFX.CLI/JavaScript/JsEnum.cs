using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DataDynamics.PageFX.Common;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.CLI.JavaScript
{
	internal sealed class JsEnum
	{
		public static void Compile(JsCompiler compiler, JsClass klass, ObjectMethodId id)
		{
			var type = klass.Type;
			if (!type.IsEnum)
				throw new InvalidOperationException("Type is not enum.");

			switch (id)
			{
				case ObjectMethodId.Equals:
					//TODO: implement Equals for enums here
					JsStruct.Compile(compiler, klass, id);
					break;
				case ObjectMethodId.GetHashCode:
					CompileGetHashCode(klass);
					break;
				case ObjectMethodId.ToString:
					CompileToString(compiler, klass);
					break;
				default:
					throw new ArgumentOutOfRangeException("id");
			}
		}

		private static void CompileGetHashCode(JsClass klass)
		{
			var type = klass.Type;
			var func = new JsFunction(null);

			var value = "this.$value".Id();

			if (type.ValueType.IsInt64())
			{
				//TODO: inline [U]Int64.GetHashCode implementation
				func.Body.Add(value.Get("GetHashCode").Call().Return());
			}
			else if (!type.ValueType.Is(SystemTypeCode.Int32))
			{
				func.Body.Add("$conv".Id().Call(value, type.ValueType.JsTypeCode(), SystemTypes.Int32.JsTypeCode()).Return());
			}
			else
			{
				func.Body.Add(value.Return());
			}

			klass.ExtendPrototype(func, "GetHashCode");
		}

		private static void CompileToString(JsCompiler compiler, JsClass klass)
		{
			var type = klass.Type;
			
			CompileValues(compiler, klass);

			var func = new JsFunction(null);
			if (type.HasAttribute("System.FlagsAttribute"))
			{
				func.Body.Add(new JsText("return $enum.flags(this);"));
			}
			else
			{
				func.Body.Add(new JsText("return $enum.stringify(this);"));
			}

			klass.ExtendPrototype(func, "toString");
		}

		private static void CompileValues(JsCompiler compiler, JsClass klass)
		{
			var type = klass.Type;

			var fields = type.GetEnumFields()
				.Select(x =>
					{
						var value = GetValue(compiler, x, type.ValueType);
						return new {Name = x.Name, Value = value};
					});

			var typeName = type.JsFullName();
			var valuesField = string.Format("{0}.$$values", typeName);
			object values;

			if (type.ValueType.IsInt64())
			{
				values = new JsArray(fields.Select(x => (object)new JsObject
					{
						{"name", x.Name},
						{"value", x.Value},
					}));
			}
			else
			{

				values = new JsObject(fields.Select(x => new KeyValuePair<object, object>(x.Value, x.Name)));
			}

			klass.Add(new JsGeneratedField(valuesField, values));

			var func = new JsFunction(null);
			func.Body.Add(valuesField.Id().Return());

			klass.ExtendPrototype(func, "$values");
		}

		private static object GetValue(JsCompiler compiler, IField field, IType valueType)
		{
			var value = field.Value;
			if (value == null)
				throw new InvalidOperationException();
			return CompileValue(compiler, valueType, value);
		}

		private static object CompileValue(JsCompiler compiler, IType type, object value)
		{
			var st = type.SystemType();
			if (st == null)
				throw new ArgumentException("Type is not system");
			switch (st.Code)
			{
				case SystemTypeCode.Int8:
					return Convert.ToSByte(value, CultureInfo.InvariantCulture);
				case SystemTypeCode.UInt8:
					return Convert.ToByte(value, CultureInfo.InvariantCulture);
				case SystemTypeCode.Int16:
					return Convert.ToInt16(value, CultureInfo.InvariantCulture);
				case SystemTypeCode.UInt16:
					return Convert.ToUInt16(value, CultureInfo.InvariantCulture);
				case SystemTypeCode.Int32:
					return Convert.ToInt32(value, CultureInfo.InvariantCulture);
				case SystemTypeCode.UInt32:
					return Convert.ToUInt32(value, CultureInfo.InvariantCulture);
				case SystemTypeCode.Int64:
					return compiler.CompileInt64(Convert.ToInt64(value, CultureInfo.InvariantCulture));
				case SystemTypeCode.UInt64:
					return compiler.CompileUInt64(Convert.ToUInt64(value, CultureInfo.InvariantCulture));
				default:
					throw new ArgumentException("Invalid type");
			}
		}
	}
}
