using DataDynamics.PageFX.Common.NUnit;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	public static class TypeMemberExtensions
	{
		public static bool IsExposed(this ITypeMember member)
		{
			if (member == null) return false;

			var type = member as IType;
			if (type != null)
			{
				if (GenericType.HasGenericParams(type))
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
