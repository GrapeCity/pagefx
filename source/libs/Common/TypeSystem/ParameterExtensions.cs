namespace DataDynamics.PageFX.Common.TypeSystem
{
	public static class ParameterExtensions
	{
		/// <summary>
		/// Gets the flag indicating whether parameter is passed by reference.
		/// </summary>
		public static bool IsByRef(this IParameter p)
		{
			return p.Type != null && p.Type.TypeKind == TypeKind.Reference;
		}
	}
}