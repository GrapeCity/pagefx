using System;
using DataDynamics.PageFX.Ecma335.IL;

namespace DataDynamics.PageFX.Ecma335.Pdb
{
	internal interface ISymbolLoader : IDisposable
	{
		bool LoadSymbols(IClrMethodBody body);
	}
}
