using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace DataDynamics.PageFX.CLI.Execution
{
	using Methods = IReadOnlyList<MethodBase>;

	internal class NativeClass
	{
		private Dictionary<string, Dictionary<int, Methods>> _methods;

		public Type Type { get; private set; }

		public NativeClass(Type type)
		{
			if (type == null) throw new ArgumentNullException("type");

			Type = type;
		}

		private void Init()
		{
			if (_methods != null) return;

			_methods = new Dictionary<string, Dictionary<int, Methods>>();

			AddGroup(".ctor", Type.GetConstructors());

			foreach (var group in Type.GetMethods().GroupBy(x => x.Name))
			{
				AddGroup(group.Key, group.Cast<MethodBase>());
			}
		}

		private void AddGroup(string name, IEnumerable<MethodBase> group)
		{
			var methodGroup = new Dictionary<int, Methods>();

			foreach (var subGroup in group.GroupBy(x => x.GetParameters().Length))
			{
				methodGroup.Add(subGroup.Key, subGroup.AsReadOnlyList());
			}

			var withParams = group.Where(HasParamsArgument).AsReadOnlyList();
			if (withParams.Count > 0)
			{
				methodGroup.Add(-1, withParams);
			}

			//TODO: support optional parameters

			_methods.Add(name, methodGroup);
		}

		public override string ToString()
		{
			return Type.ToString();
		}

		private static bool HasParamsArgument(MethodBase method)
		{
			var p = method.GetParameters();
			if (p.Length == 0) return false;

			var pi = p[p.Length - 1];

			if (pi.IsDefined(typeof(ParamArrayAttribute), false))
			{
				return true;
			}

			return false;
		}

		public virtual object Invoke(string name, object instance, object[] args)
		{
			Init();

			//TODO: optional parameters

			Dictionary<int, Methods> group;
			if (_methods.TryGetValue(name, out group))
			{
				Methods methods;
				if (group.TryGetValue(args.Length, out methods))
				{
					if (methods.Count == 1)
					{
						var method = methods[0];
						return Invoke(method, instance, args);
					}
					else
					{
						bool exact;
						var method = ResolveMethod(methods, args, out exact);
						if (exact)
						{
							return Invoke(method, instance, args);
						}

						ConvertArgs(method, args, args.Length);

						return Invoke(method, instance, args);
					}
				}

				// params
				if (group.TryGetValue(-1, out methods))
				{
					var method = methods[0];

					var paramCount = method.GetParameters().Length;
					ConvertArgs(method, args, paramCount - 1);
					
					var methodArgs = new object[paramCount];
					Array.Copy(args, methodArgs, paramCount - 1);

					var last = new List<object>();
					for (int i = paramCount - 1; i < args.Length; i++)
					{
						last.Add(args[i]);
					}

					methodArgs[paramCount - 1] = last.ToArray();
					
					return Invoke(method, instance, methodArgs);
				}
			}

			if (Type == typeof(Exception))
			{
				return null;
			}

			throw new InvalidOperationException(string.Format("Method {0}.{1} was not found.", Type.FullName, name));
		}

		private static object Invoke(MethodBase method, object instance, object[] args)
		{
			var ctor = method as ConstructorInfo;
			if (ctor != null)
			{
				return ctor.Invoke(args);
			}

			return method.Invoke(instance, args);
		}

		private static void ConvertArgs(MethodBase method, object[] args, int length)
		{
			//determine if any of these arguments need narrowing/widening
			for (int i = 0; i < length; i++)
			{
				object arg = args[i];
				if (arg == null)
				{
					continue;
				}
				var argType = arg.GetType();
				var parameterType = method.GetParameters()[i].ParameterType;
				if (argType == parameterType)
				{
					continue;
				}

				//If this is a narrowing method, try and convert the argument to the expected type
				if (CanNarrow(argType.ToString(), parameterType.ToString()))
				{
					args[i] = Convert.ChangeType(arg, parameterType);
				}
				else if (CanWiden(argType.ToString(), parameterType.ToString()))
				{
					args[i] = Convert.ChangeType(arg, parameterType);
				}
			}
		}

		private static MethodBase ResolveMethod(IEnumerable<MethodBase> methods, object[] args, out bool exact)
		{
			exact = false;
			var matches = new List<MethodBase>();
			int lastWeight = 0;
			int exactMatchWeight = GetWeight(TypeMatch.Exact) * args.Length;
			foreach (var method in methods)
			{
				int weight = CalculateArgumentsMatchWeight(method, args);
				if (weight == 0) continue;

				if (weight == exactMatchWeight)
				{
					//All the types are an exact match.  This is the one and only definition.
					//Note: There is problem with duplicate functions loaded from custom code assemblies.
					if (method.GetParameters().Length == args.Length)
					{
						exact = true;
						return method;
					}
					// function with params, continue search to find function with exact match without params
					matches.Add(method);
					continue;
				}

				if (matches.Count == 0)
				{
					matches.Add(method);
					lastWeight = weight;
				}
				// weight with larger value wins
				// since it requires less cast operations
				// to call function with given arguments
				else if (weight > lastWeight)
				{
					matches.Clear();
					matches.Add(method);
					lastWeight = weight;
				}
				else if (weight == lastWeight)
				{
					matches.Add(method);
				}
			}

			if (matches.Count > 0)
			{
				return matches[0];
			}

			return null;
		}

		/// <summary>
		/// Calculates a weight specifying how given function is matched to the given arguments.
		/// </summary>
		/// <param name="function">The function to examine.</param>
		/// <param name="arguments">The passed function arguments.</param>
		/// <returns>The function match weight.</returns>
		private static int CalculateArgumentsMatchWeight(MethodBase function, object[] arguments)
		{
			if (function == null) return 0;

			int paramCount = function.GetParameters().Length;

			// no enough arguments for function - no match
			if (arguments.Length < paramCount)
			{
				return 0;
			}

			// function is parameterless?
			if (paramCount == 0)
			{
				return arguments.Length > 0 ? 0 : GetWeight(TypeMatch.Exact);
			}

			// paramCount > 0
			// arguments.Length >= paramCount
			bool hasParams = HasParamsArgument(function);
			if (arguments.Length != paramCount && !hasParams)
			{
				return 0;
			}

			int weight = 0;

			Type paramType = null;
			for (int index = 0; index < arguments.Length; index++)
			{
				object argument = arguments[index];
				var argType = argument == null ? null : argument.GetType();
				if (index < paramCount)
				{
					paramType = function.GetParameters()[index].ParameterType;
				}

				if (hasParams && index == paramCount - 1
				    && paramType != null && paramType.IsArray
					&& argType != null && !argType.IsArray)
				{
					paramType = paramType.GetElementType();
				}

				var argumentMatch = ResolveTypeMatch(paramType, argType);
				// if one of argument does not match
				if (argumentMatch == TypeMatch.None)
				{
					return 0;
				}

				weight += GetWeight(argumentMatch);
			}

			return weight;
		}

		/// <summary>
		/// Gets weight of the given type match.
		/// </summary>
		/// <param name="match">The type match to get weight for.</param>
		/// <returns>The weight of given type match.</returns>
		private static int GetWeight(TypeMatch match)
		{
			switch (match)
			{
				case TypeMatch.Exact:
					return 5;
				case TypeMatch.Assignable:
					return 4;
				case TypeMatch.Widened:
					return 3;
				case TypeMatch.Narrowed:
					return 2;
				case TypeMatch.Unknown:
					return 1;
				case TypeMatch.None:
					return 0;
				default:
					throw new ArgumentOutOfRangeException("match");
			}
		}

		/// <summary>
		/// Gets type match between type of function parameter and type of passed argument.
		/// </summary>
		/// <param name="paramType">The resolved type of function parameter.</param>
		/// <param name="argType">The resolved type of passed argument definition.</param>
		/// <returns></returns>
		private static TypeMatch ResolveTypeMatch(Type paramType, Type argType)
		{
			if (paramType == argType)
			{
				return TypeMatch.Exact;
			}
			if (argType != null && paramType.IsAssignableFrom(argType))
			{
				return TypeMatch.Assignable;
			}
			var argTypeName = argType == null ? "nothing" : argType.FullName;
			if (CanWiden(argTypeName, paramType.FullName))
			{
				return TypeMatch.Widened;
			}
			if (CanNarrow(argTypeName, paramType.FullName))
			{
				return TypeMatch.Narrowed;
			}
			return TypeMatch.None;
		}

		/// <summary>
		/// Describes the different type matching results.
		/// </summary>
		private enum TypeMatch
		{
			/// <summary>
			/// Types are equal.
			/// </summary>
			Exact,
			/// <summary>
			/// Types matched, though at least one argument would need an implicit conversion.
			/// </summary>
			Assignable,
			/// <summary>
			/// Types matched, though at least one argument was unknown.
			/// </summary>
			Unknown,
			/// <summary>
			/// Types matched, though at least one argument would need an implicit conversion.
			/// </summary>
			Widened,
			/// <summary>
			/// Types matched, though at least one argument would need a narrowing conversion.
			/// </summary>
			Narrowed,
			/// <summary>
			/// Types do not match.
			/// </summary>
			None,
		}

		//TODO: refactor code below, simplify it using TypeCode

		#region Widening

		private static readonly string[] WideningAndNarrowingTypes = new[]{"system.byte",
			"system.int16","system.uint16","system.int32","system.uint32",
			"system.int64","system.uint64","system.decimal","system.single","system.double",
			"system.boolean","system.datetime","system.char","system.string","system.sbyte"
		};

		private static readonly bool[][] WideningConversion = new[]
		                                                      	{
/*from byte*/	new[]{true, true, true, true, true, true, true, true, true, true,false,false,false,false, false},
/*from int16*/	new[]{false,true, true, true, true, true, true, true, true, true,false,false,false,false, false},
/*from uint16*/	new[]{false,false,true, true, true, true, true, true, true, true,false,false,false,false, false},
/*from int32*/	new[]{false,false,false,true, true, true, true, true, true, true,false,false,false,false, false},
/*from uint32*/	new[]{false,false,false,false,true, true, true, true, true, true,false,false,false,false, false},
/*from int64*/	new[]{false,false,false,false,false,true, true, true, true, true,false,false,false,false, false},
/*from uint64*/	new[]{false,false,false,false,false,false,true, true, true, true,false,false,false,false, false},
/*from decima*/	new[]{false,false,false,false,false,false,false,true, true, true,false,false,false,false, false},
/*from single*/	new[]{false,false,false,false,false,false,false,false,true, true,false,false,false,false, false},
/*from double*/	new[]{false,false,false,false,false,false,false,false,false,true,false,false,false,false, false},
/*from bool*/	new[]{false,false,false,false,false,false,false,false,false,false,true,false,false,false, false},
/*from datetm*/	new[]{false,false,false,false,false,false,false,false,false,false,false,true,false,false, false},
/*from char*/	new[]{false,false,false,false,false,false,false,false,false,false,false,false,true,true, false},
/*from string*/	new[]{false,false,false,false,false,false,false,false,false,false,false,false,false, true, false},
/*from sbyte*/  new[]{false, true, false, true, false, true, false, true, true, true, false, false, false, false, true }
		                                                      	};
		private static readonly List<string> WideningList = new List<string>(WideningAndNarrowingTypes);

		/// <summary>
		/// Returns whether a <see cref="System.Type"/> can widen (implicit conversion) to another <see cref="System.Type"/>.
		/// </summary>
		/// <param name="fromType">The name of the <see cref="System.Type"/> that should be tested to see if it can be widened</param>
		/// <param name="toType">The name of the <see cref="System.Type"/> that is tested as the destination</param>
		/// <returns>true if fromType can be widened to toType, otherwise false.</returns>
		private static bool CanWiden(string fromType, string toType)
		{
			if (string.Compare(fromType, toType, true, CultureInfo.InvariantCulture) == 0)
			{//same type
				return true;
			}
			if (string.Compare(toType, "system.object", true, CultureInfo.InvariantCulture) == 0)
			{//anything can widen to object
				return true;
			}
			if (string.Compare(fromType, "nothing", true, CultureInfo.InvariantCulture) == 0)
			{//anything can widen from Nothing
				return true;
			}
			//Conversions from any string to char[]
			if ((string.Compare(fromType, "system.char[]", true, CultureInfo.InvariantCulture) == 0) &&
				(string.Compare(toType, "system.string", true, CultureInfo.InvariantCulture) == 0))
			{
				return true;
			}
			int fromIndex = WideningList.IndexOf(fromType.ToLower(CultureInfo.InvariantCulture));
			int toIndex = WideningList.IndexOf(toType.ToLower(CultureInfo.InvariantCulture));
			if (fromIndex == -1 || toIndex == -1)
				return false;
			return WideningConversion[fromIndex][toIndex];
		}

		#endregion

		#region Narrowing

		private static readonly bool[][] NarrowingConversion = new[]
		                                                       	{
/*from byte*/	new[]{false,false,false,false,false,false,false,false,false,false, true, true, true, true,false},
/*from int16*/	new[]{ true,false,false,false,false,false,false,false,false,false, true, true, true, true,false},
/*from uint16*/	new[]{ true, true,false,false,false,false,false,false,false,false, true, true, true, true,false},
/*from int32*/	new[]{ true, true, true,false,false,false,false,false,false,false, true, true, true, true,false},
/*from uint32*/	new[]{ true, true, true, true,false,false,false,false,false,false, true, true, true, true,false},
/*from int64*/	new[]{ true, true, true, true, true,false,false,false,false,false, true, true, true, true,false},
/*from uint64*/	new[]{ true, true, true, true, true, true,false,false,false,false, true, true, true, true,false},
/*from decima*/	new[]{ true, true, true, true, true, true, true,false,false,false, true, true, true, true,false},
/*from single*/	new[]{ true, true, true, true, true, true, true, true,false,false, true, true, true, true,false},
/*from double*/	new[]{ true, true, true, true, true, true, true, true, true,false, true, true, true, true,false},
/*from bool*/	new[]{ true, true, true, true, true, true, true, true, true, true,false,false,false, true,false},
/*from datetm*/	new[]{ true, true, true, true, true, true, true, true, true, true,false,false, true, true,false},
/*from char*/	new[]{false,false,false,false,false,false,false,false,false,false,false, true,false,false,false},
/*from string*/	new[]{ true, true, true, true, true, true, true, true, true, true, true, true, true,false,false},
/*from sbyte*/  new[]{false,false,false,false,false,false,false,false,false,false, true, true, true, true, false}
		                                                       	};
		private static readonly List<string> NarrowingList = new List<string>(WideningAndNarrowingTypes);

		/// <summary>
		/// Returns whether a <see cref="System.Type"/> can narrow (with possible information loss) to another <see cref="System.Type"/>.
		/// </summary>
		/// <param name="fromType">The name of the <see cref="System.Type"/> that should be tested to see if it can be narrowed</param>
		/// <param name="toType">The name of the <see cref="System.Type"/> that is tested as the destination</param>
		/// <returns>true if fromType can be narrowed to toType, otherwise false.</returns>
		/// <remarks>http://msdn.microsoft.com/library/default.asp?url=/library/en-us/vbls7/html/vblrfvbspec10_4.asp</remarks>
		private static bool CanNarrow(string fromType, string toType)
		{
			//If the types are the same it would not need a narrowing conversion.
			if (string.Compare(fromType, toType, true, CultureInfo.InvariantCulture) == 0)
			{
				return false;
			}
			//Conversions from any string to char[]
			if ((string.Compare(fromType, "system.string", true, CultureInfo.InvariantCulture) == 0) &&
				(string.Compare(toType, "system.char[]", true, CultureInfo.InvariantCulture) == 0))
			{
				return true;
			}
			if (string.Compare(fromType, "nothing", true, CultureInfo.InvariantCulture) == 0)
			{
				return false;
			}
			int fromIndex = NarrowingList.IndexOf(fromType.ToLower(CultureInfo.InvariantCulture));
			int toIndex = NarrowingList.IndexOf(toType.ToLower(CultureInfo.InvariantCulture));
			if (fromIndex == -1 || toIndex == -1)
				return false;

			//Numeric type conversions in the following direction: Double, Single, Decimal, Long, Integer, Short, Byte. 
			return NarrowingConversion[fromIndex][toIndex];
		}

		#endregion
	}
}