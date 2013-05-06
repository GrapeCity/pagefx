using System;
using DataDynamics.PageFX.Core.IL;

namespace DataDynamics.PageFX.Core
{
	internal interface ISymbolLoader : IDisposable
	{
		bool LoadSymbols(IClrMethodBody body);
	}
}
