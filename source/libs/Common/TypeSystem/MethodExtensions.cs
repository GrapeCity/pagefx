using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using DataDynamics.PageFX.Common.JavaScript;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	public static class MethodExtensions
	{
		public static bool IsGetter(this IMethod method)
		{
			if (method is GenericMethodInstance)
			{
				return IsGetter(method.InstanceOf);
			}

			if (method is MethodProxy)
			{
				return IsGetter(method.ProxyOf);
			}

			var prop = method.Association as IProperty;
			return prop != null && prop.Getter == method;
		}

		public static bool IsSetter(this IMethod method)
		{
			if (method is GenericMethodInstance)
			{
				return IsSetter(method.InstanceOf);
			}

			if (method is MethodProxy)
			{
				return IsSetter(method.ProxyOf);
			}

			var prop = method.Association as IProperty;
			return prop != null && prop.Setter == method;
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

		public static IMethod GetExplicitImpl(this IMethod method)
		{
			if (method == null || !method.IsExplicitImplementation)
				return null;
			var impl = method.Implements;
			if (impl == null || impl.Count != 1)
				throw new InvalidOperationException("bad explicit implementation");
			return impl[0];
		}

		public static bool IsAccessor(this IMethod method)
		{
			var prop = method.Association as IProperty;
			return prop != null && prop.Parameters.Count <= 0;
		}

		public static string GetSigName(this IType type, Runtime runtime)
		{
			return type.GetSigName().ToValidId(runtime);
		}

		public static string BuildSigName(this IMethod method, Runtime runtime)
		{
			var impl = method.GetExplicitImpl();
			if (impl != null)
				return impl.BuildSigName(runtime);

			var declType = method.DeclaringType;

			var sb = new StringBuilder();

			var sigName = declType.GetSigName(runtime);

			if (declType.IsInterface)
			{
				sb.Append(sigName);
				sb.Append(runtime == Runtime.Avm ? "." : "$");
			}

			bool addParams = true;
			if (method.IsConstructor)
			{
				if (!method.IsStatic)
					sb.Append(declType.Name.ToValidId(runtime));
				sb.Append(method.Name.ToValidId(runtime));
			}
			else
			{
				if (method.NeedDeclaringTypePrefix())
				{
					sb.Append(sigName);
					sb.Append("_");
				}

				if (method.NeedReturnTypePrefix())
				{
					sb.Append(method.Type.GetSigName(runtime));
					sb.Append("_");
				}

				var prop = method.Association as IProperty;
				if (runtime == Runtime.Avm && prop != null && method.IsAccessor())
				{
					sb.Append(prop.Name.ToValidId(runtime));
					addParams = false;
				}
				else
				{
					sb.Append(method.Name.ToValidId(runtime));
				}
			}

			if (method.IsGenericInstance)
			{
				sb.AppendSigName(method.GenericArguments, runtime);
			}

			if (addParams && method.Parameters.Count > 0)
			{
				sb.Append("_");
				AppendParametersSignature(sb, method, runtime);
			}

			return sb.ToString();
		}

		private static bool NeedDeclaringTypePrefix(this IMethod method)
		{
			if (method.IsInternalCall)
			{
				var type = method.DeclaringType;
				if (type.IsArray) return true;
			}
			return method.IsNew();
		}

		private static bool NeedReturnTypePrefix(this IMethod method)
		{
			// fix for cast operators
			if (method.IsStatic && method.Name.StartsWith("op_"))
				return true;

			var baseMethod = method.BaseMethod;
			if (baseMethod != null)
			{
				if (!ReferenceEquals(baseMethod.Type, method.Type))
					return true;
			}

			return false;
		}

		private static void AppendParametersSignature(StringBuilder sb, IMethod method, Runtime runtime)
		{
			sb.AppendSigName(method.Parameters.Select(x => x.Type), runtime);
		}

		public static string GetParametersSignature(this IMethod method, Runtime runtime)
		{
			var sb = new StringBuilder();
			AppendParametersSignature(sb, method, runtime);
			return sb.ToString();
		}

		public static IMethod ResolveGenericInstance(this IMethod method, IType type, IMethod context)
		{
			if (method.IsGeneric)
			{
				if (!context.IsGenericInstance)
					throw new InvalidOperationException("invalid context");
				method = GenericType.CreateMethodInstance(type, method, context.GenericArguments);
			}
			return method;
		}

		public static int GetSpecificity(this IMethod method)
		{
			return method.Parameters.Count(p => !p.HasResolvedType());
		}

		private static bool HasResolvedType(this IParameter p)
		{
			var proxy = p as IParameterProxy;
			return proxy != null && !ReferenceEquals(proxy.Type, proxy.ProxyOf.Type);
		}

		public static bool SignatureChanged(this IMethod method)
		{
			var proxy = method as MethodProxy;
			var instance = method as GenericMethodInstance;
			if (proxy == null && instance == null)
				return false;

			var parent = proxy != null ? proxy.ProxyOf : instance.InstanceOf;

			if (!ReferenceEquals(method.Type, parent.Type))
				return true;

			for (int i = 0; i < parent.Parameters.Count; i++)
			{
				var p1 = parent.Parameters[i];
				var p2 = method.Parameters[i];
				if (!ReferenceEquals(p1.Type, p2.Type))
					return true;
			}

			return false;
		}

		public static bool IsPrivate(this IMethod method)
		{
			switch (method.Visibility)
			{
				case Visibility.PrivateScope:
				case Visibility.NestedPrivate:
				case Visibility.Private:
					return true;
				default:
					return false;
			}
		}
	}
}
