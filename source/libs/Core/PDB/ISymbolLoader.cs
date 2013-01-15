using System;
using DataDynamics.PageFX.Core.IL;

namespace DataDynamics.PageFX.Core.Pdb
{
	internal interface ISymbolLoader : IDisposable
	{
		bool LoadSymbols(IClrMethodBody body);
	}
}
