namespace DataDynamics.PageFX.Common.TypeSystem
{
	public static class ParameterExtensions
	{
		/// <summary>
		/// Indicates whether the method parameter that takes an argument where the number of arguments is variable.
		/// </summary>
		public static bool HasParams(this IParameter p)
		{
			return p.HasAttribute("System.ParamArrayAttribute");
		}

		/// <summary>
		/// Gets the flag indicating whether parameter is passed by reference.
		/// </summary>
		public static bool IsByRef(this IParameter p)
		{
			return p.Type != null && p.Type.TypeKind == TypeKind.Reference;
		}
	}
}