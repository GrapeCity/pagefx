using System;
using DataDynamics.PageFX.Common.Collections;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	public interface IAssemblyLoader : IMetadataTokenResolver
	{
		void ResolveAssemblyReferences();

		IMethod ResolveEntryPoint();

		IReadOnlyList<IType> GetExposedTypes();

		event EventHandler<TypeEventArgs> TypeLoaded;
	}

	public sealed class TypeEventArgs : EventArgs
	{
		public TypeEventArgs(IType type)
		{
			Type = type;
		}

		public IType Type { get; private set; }
	}
}