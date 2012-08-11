using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace DataDynamics.PageFX.CodeModel
{
	public static class MethodExtensions
	{
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
			if (method == null) return null;
			if (!method.IsExplicitImplementation) return null;
			var impl = method.ImplementedMethods;
			if (impl == null || impl.Length != 1)
				throw new InvalidOperationException("bad explicit implementation");
			return impl[0];
		}

		public static bool IsAccessor(this IMethod method)
		{
			var prop = method.Association as IProperty;
			return prop != null && prop.Parameters.Count <= 0;
		}

		public static string BuildSigName(this IMethod method, SigKind kind)
		{
			var impl = method.GetExplicitImpl();
			if (impl != null)
				return impl.BuildSigName(kind);

			var declType = method.DeclaringType;

			var sb = new StringBuilder();

			if (declType.IsInterface)
			{
				sb.Append(declType.SigName + ".");
			}

			bool addParams = true;
			if (method.IsConstructor)
			{
				if (kind == SigKind.Avm && !method.IsStatic)
					sb.Append(declType.Name);
				sb.Append(kind == SigKind.Js ? method.Name.Replace('.', '$') : method.Name);
			}
			else
			{
				if (method.NeedDeclaringTypePrefix())
				{
					sb.Append(declType.SigName);
					sb.Append("_");
				}

				if (method.NeedReturnTypePrefix())
				{
					sb.Append(method.Type.GetSigName());
					sb.Append("_");
				}

				var prop = method.Association as IProperty;
				if (kind == SigKind.Avm && prop != null && method.IsAccessor())
				{
					sb.Append(prop.Name);
					addParams = false;
				}
				else
				{
					sb.Append(method.Name);
				}
			}

			if (method.IsGenericInstance)
			{
				sb.AppendSigName(method.GenericArguments);
			}

			if (addParams && method.Parameters.Count > 0)
			{
				sb.Append("_");
				AppendParametersSignature(sb, method);
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
				if (baseMethod.Type != method.Type)
					return true;
			}
			return false;
		}

		private static void AppendParametersSignature(StringBuilder sb, IMethod method)
		{
			sb.AppendSigName(method.Parameters.Select(x => x.Type));
		}

		public static string GetParametersSignature(this IMethod method)
		{
			var sb = new StringBuilder();
			AppendParametersSignature(sb, method);
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
	}
}
