using System.Collections.Generic;
using System.Linq;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	public static class GenericExtensions
	{
		public static bool HasGenericParams(IEnumerable<IType> set)
		{
			return set != null && set.Any(HasGenericParams);
		}

		public static bool HasGenericParams(this IType type)
		{
			if (type == null) return false;
            
			if (type.GenericParameters.Count > 0 || type.IsGenericParameter())
				return true;

			if (type.ElementType != null)
				return type.ElementType.HasGenericParams();

			return type.GenericArguments.Any(HasGenericParams);
		}

		public static bool IsGenericContext(this ITypeMember member)
		{
			var method = member as IMethod;
			if (method != null)
			{
				if (method.IsGeneric)
					return true;
				if (method.IsGenericInstance)
				{
					if (HasGenericParams(method.GenericArguments))
						return true;
				}
				if (method.Parameters.Any(p => p.Type.HasGenericParams()))
				{
					return true;
				}
			}

			var type = member as IType;
			if (type != null)
			{
				if (type.HasGenericParams())
					return true;
			}

			if (member.DeclaringType.HasGenericParams())
				return true;

			if (member.Type.HasGenericParams())
				return true;

			return false;
		}
	}
}
