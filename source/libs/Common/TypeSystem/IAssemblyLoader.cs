using System;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	public interface IAssemblyLoader : IMetadataTokenResolver
	{
		void ResolveAssemblyReferences();

		IMethod ResolveEntryPoint();

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