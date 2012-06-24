using System.Linq;
using System.Runtime.CompilerServices;

namespace DataDynamics.PageFX.CodeModel
{
	public static class MethodExtensions
	{
		public static IMethod FindImplementation(this IType type, IMethod method, bool inherited)
		{
			if (method.IsGenericInstance)
				method = method.InstanceOf;

			while (type != null)
			{
				var impl = (from candidate in type.Methods
				            where candidate.ImplementedMethods != null &&
				                  candidate.ImplementedMethods.Any(x => x == method || x.ProxyOf == method)
				            select candidate).FirstOrDefault();
				if (impl != null)
				{
					return impl;
				}
				if (!inherited) break;
				type = type.BaseType;
			}

			return null;
		}

		public static IMethod FindImplementation(this IType type, IMethod method)
		{
			return type.FindImplementation(method, true);
		}

		public static bool IsNew(this IMethod method)
		{
			if (method == null) return false;
			if (method.IsStatic) return false;
			if (method.IsConstructor) return false;
			if (method.IsVirtual)
			{
				if (method.IsNewSlot)
				{
					if (method.IsExplicitImplementation)
						return false;
					return method.HasBaseDef();
				}
				return false;
			}
			return method.HasBaseDef();
		}

		private static bool HasBaseDef(this IMethod method)
		{
			if (method.BaseMethod != null)
				return true;
			if (method.Parameters.Count == 0)
			{
				switch (method.Visibility)
				{
					case Visibility.Private:
					case Visibility.NestedPrivate:
						return false;
				}
				string name = method.Name;
				var bt = method.DeclaringType.BaseType;
				while (bt != null)
				{
					var f = bt.Fields[name];
					if (f != null)
						return true;
					bt = bt.BaseType;
				}
			}
			return false;
		}

		public static bool IsImplemented(this IMethod method)
		{
			if (method.IsInternalCall)
			{
				return false;
			}
			switch (method.CodeType)
			{
				case MethodCodeType.Native:
				case MethodCodeType.Runtime:
					return false;
			}
			return true;
		}
	}
}
