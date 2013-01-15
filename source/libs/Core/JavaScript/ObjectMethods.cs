using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Core.JavaScript
{
	internal static class ObjectMethods
	{
		public static IMethod Find(IType objectType, ObjectMethodId id)
		{
			return objectType.Methods.FirstOrDefault(x => x.Is(id));
		}

		public static bool IsGetType(this IMethod method)
		{
			if (method.DeclaringType.IsInterface) return false;
			return !method.IsStatic
			       && method.Parameters.Count == 0
			       && method.Type.Is(SystemTypeCode.Type)
			       && method.Name == "GetType";
		}

		public static bool Is(this IMethod method, ObjectMethodId id)
		{
			if (method.DeclaringType.IsInterface || method.IsStatic)
				return false;

			string methodName = id.ToString();
			if (method.Name != methodName)
				return false;

			switch (id)
			{
				case ObjectMethodId.Equals:
					return method.Parameters.Count == 1
					       && method.Parameters[0].Type.Is(SystemTypeCode.Object)
					       && method.Type.Is(SystemTypeCode.Boolean);
				case ObjectMethodId.GetHashCode:
					return method.Parameters.Count == 0
					       && method.Type.Is(SystemTypeCode.Int32);
				case ObjectMethodId.ToString:
					return method.Parameters.Count == 0
					       && method.Type.Is(SystemTypeCode.String);
				default:
					throw new ArgumentOutOfRangeException("id");
			}
		}

		public static bool IsToString(this IMethod method)
		{
			return Is(method, ObjectMethodId.ToString);
		}

		private static readonly Dictionary<string, ObjectMethodId> NameToId = new Dictionary<string, ObjectMethodId>
			{
				{"Equals", ObjectMethodId.Equals},
				{"GetHashCode", ObjectMethodId.GetHashCode},
				{"ToString", ObjectMethodId.ToString},
			};

		public static ObjectMethodId GetObjectMethodId(this IMethod method)
		{
			ObjectMethodId id;
			if (NameToId.TryGetValue(method.Name, out id)
				&& method.Is(id))
			{
				return id;
			}
			return ObjectMethodId.Unknown;
		}
	}

	internal enum ObjectMethodId
	{
		Unknown,
		Equals,
		GetHashCode,
		ToString
	}
}