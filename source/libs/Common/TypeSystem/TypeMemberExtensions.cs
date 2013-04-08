using DataDynamics.PageFX.Common.NUnit;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	public static class TypeMemberExtensions
	{
		public static string BuildFullName(this ITypeMember member)
		{
			var declType = member.DeclaringType;
			return declType != null ? declType.FullName + "." + member.Name : member.Name;
		}

		public static bool IsVisible(this ITypeMember member)
		{
			var genericParameter = member as IGenericParameter;
			if (genericParameter != null)
			{
				return true;
			}

			var compoundType = member as IType;
			if (compoundType != null && compoundType.ElementType != null)
			{
				return compoundType.ElementType.IsVisible();
			}

			var declType = member.DeclaringType;
			if (declType != null && !declType.IsVisible())
			{
				return false;
			}

			var genericInstance = member as IGenericInstance;
			if (genericInstance != null)
			{
				return genericInstance.Type == null || genericInstance.Type.IsVisible();
			}
			
			switch (member.Visibility)
			{
				case Visibility.Public:
				case Visibility.NestedPublic:
					return true;
			}

			return false;
		}

		public static bool IsExposed(this ITypeMember member)
		{
			if (member == null) return false;

			var type = member as IType;
			if (type != null)
			{
				if (type.HasGenericParams())
					return false;

				if (type.IsTestFixture())
					return true;
			}

			var method = member as IMethod;
			if (method != null)
			{
				if (method.DeclaringType.IsTestFixture())
				{
					if (method.IsConstructor)
						return true;
					if (method.IsNUnitMethod())
						return true;
				}

				if (method.Association != null
					&& method.Association.IsExposed())
					return true;
			}

			return member.HasAttribute("ExposeAttribute");
		}
	}
}
