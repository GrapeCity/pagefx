using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Core.LoaderInternals.Collections;
using DataDynamics.PageFX.Core.Metadata;
using DataDynamics.PageFX.Core.Tools;

namespace DataDynamics.PageFX.Core.LoaderInternals
{
	internal sealed class InternalAssembly : IAssembly
	{
		private readonly AssemblyLoader _loader;
		private SystemTypes _systemTypes;
		private TypeFactory _typeFactory;

		public InternalAssembly(AssemblyLoader loader, MetadataRow row)
		{
			_loader = loader;

			var flags = ((AssemblyFlags)row[Schema.Assembly.Flags].Value);
			var hashAlgorithm = (HashAlgorithmId)row[Schema.Assembly.HashAlgId].Value;

			byte[] publicKey = null;
			byte[] publicKeyToken = null;
			if ((flags & AssemblyFlags.PublicKey) != 0)
			{
				publicKey = row[Schema.Assembly.PublicKey].Blob.ToArray();
				publicKeyToken = publicKey.ComputePublicKeyToken(hashAlgorithm);
			}

			Name = row[Schema.Assembly.Name].String;
			Version = GetVersion(row, 1);
			Flags = flags;
			Culture = row[Schema.Assembly.Culture].Culture;
			HashAlgorithm = hashAlgorithm;
			PublicKey = publicKey;
			PublicKeyToken = publicKeyToken;

			CustomAttributes = new CustomAttributes(_loader, this);
		}

		private static Version GetVersion(MetadataRow row, int i)
		{
			return new Version((int)row[i].Value,
							   (int)row[i + 1].Value,
							   (int)row[i + 2].Value,
							   (int)row[i + 3].Value);
		}

		public string Location
		{
			get { return _loader.Location; }
		}

		public int MetadataToken
		{
			get { return SimpleIndex.MakeToken(TableId.Assembly, 1); }
		}

		public string Name { get; private set; }

		public Version Version { get; private set; }

		public AssemblyFlags Flags { get; private set; }

		public CultureInfo Culture { get; private set; }

		public byte[] PublicKey { get; private set; }

		public byte[] PublicKeyToken { get; private set; }

		public byte[] HashValue
		{
			get { return null; }
		}

		public HashAlgorithmId HashAlgorithm { get; private set; }

		public string FullName
		{
			get { return this.BuildFullName(); }
		}
		
		public ICustomAttributeCollection CustomAttributes { get; private set; }

		public ITypeCollection Types
		{
			get
			{
				//TODO: support multimodule assemblies
				return _loader.Types;
			}
		}

		public IEnumerable<ICodeNode> ChildNodes
		{
			get { return Modules.Cast<ICodeNode>(); }
		}

		public object Data { get; set; }

		public IMethod EntryPoint
		{
			get { return _loader.ResolveEntryPoint(); }
		}

		public IModuleCollection Modules
		{
			get { return _loader.Modules; }
		}

		public IModule MainModule
		{
			get { return Modules.FirstOrDefault(x => x.IsMain); }
		}

		public IType FindType(string fullname)
		{
			if (string.IsNullOrEmpty(fullname))
				return null;

			//TODO: support multimodule assemblies
			return _loader.Types.FindType(fullname);
		}

		public IAssemblyLoader Loader
		{
			get { return _loader; }
		}

		public SystemTypes SystemTypes
		{
			get { return _systemTypes ?? (_systemTypes = ResolveSystemTypes()); }
		}

		private SystemTypes ResolveSystemTypes()
		{
			return this.IsCorlib() ? new SystemTypes(this) : this.Corlib().SystemTypes;
		}

		public TypeFactory TypeFactory
		{
			get { return _typeFactory ?? (_typeFactory = ResolveTypeFactory()); }
		}

		private TypeFactory ResolveTypeFactory()
		{
			return this.IsCorlib() ? new TypeFactory() : this.Corlib().TypeFactory;
		}

		public IReadOnlyList<IType> GetExposedTypes()
		{
			return _loader.GetExposedTypes();
		}

		public override string ToString()
		{
			return FullName;
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			return FullName;
		}

		public override bool Equals(object obj)
		{
			var other = obj as IAssembly;
			return other != null && this.EqualsTo(other);
		}

		public override int GetHashCode()
		{
			return this.EvalHashCode();
		}
	}
}
