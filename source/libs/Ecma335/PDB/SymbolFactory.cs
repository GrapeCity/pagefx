namespace DataDynamics.PageFX.Ecma335.Pdb
{
	internal static class SymbolFactory
	{
		public static ISymbolLoader CreateSymbolLoader(string location)
		{
			return SymbolLoader.Create(location);
		}
	}
}
