using System;
using DataDynamics.PageFX.Core.Mono;
using DataDynamics.PageFX.Core.Pdb;

namespace DataDynamics.PageFX.Core
{
	internal static class SymbolProvider
	{
		public static ISymbolLoader CreateLoader(string path)
		{
			return IsRunningOnMono() ? MdbSymbolLoader.Create(path) : PdbSymbolLoader.Create(path);
		}

		private static bool IsRunningOnMono()
		{
			return Type.GetType("Mono.Runtime") != null;
		}
	}
}
