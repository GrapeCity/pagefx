namespace DataDynamics.PageFX.Common.TypeSystem
{
	/// <summary>
	/// Provides resolving of metadata tokens.
	/// </summary>
	public interface IMetadataTokenResolver
	{
		/// <summary>
		/// Resolves specified token.
		/// </summary>
		/// <param name="method">The method context.</param>
		/// <param name="token">The token to resolve.</param>
		object ResolveMetadataToken(IMethod method, int token);
	}
}