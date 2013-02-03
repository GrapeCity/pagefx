using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Syntax;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Core.LoaderInternals.Collections;
using DataDynamics.PageFX.Core.Metadata;

namespace DataDynamics.PageFX.Core.LoaderInternals
{
	internal sealed class InternalModule : IModule
	{
		private readonly AssemblyLoader _loader;

		public InternalModule(AssemblyLoader loader, MetadataRow row, int index)
		{
			_loader = loader;

			MetadataToken = SimpleIndex.MakeToken(TableId.Module, index + 1);
			Name = row[Schema.Module.Name].String;
			Version = row[Schema.Module.Mvid].Guid;

			CustomAttributes = new CustomAttributes(_loader, this);
		}

		public int MetadataToken { get; private set; }

		public ICustomAttributeCollection CustomAttributes { get; private set; }

		public ITypeCollection Types
		{
			get { return _loader.Types; }
		}

		public IEnumerable<ICodeNode> ChildNodes
		{
			get { return Types.Cast<ICodeNode>(); }
		}

		public object Data { get; set; }

		public IAssembly Assembly
		{
			get { return _loader.Assembly; }
		}

		public string Name { get; private set; }
		public Guid Version { get; private set; }

		public bool IsMain
		{
			get
			{
				//TODO: support multimodule assemblies
				return true;
			}
		}

		public IAssemblyCollection References
		{
			get { return _loader.AssemblyRefs; }
		}

		public IManifestFileCollection Files
		{
			get { return _loader.Files; }
		}

		public IManifestResourceCollection Resources
		{
			get { return _loader.ManifestResources; }
		}

		public object ResolveMetadataToken(IMethod method, int token)
		{
			return _loader.ResolveMetadataToken(method, token);
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			return SyntaxFormatter.Format(this, format, formatProvider);
		}
	}
}
