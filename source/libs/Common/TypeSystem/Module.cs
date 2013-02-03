using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Syntax;

namespace DataDynamics.PageFX.Common.TypeSystem
{
    public sealed class Module : CustomAttributeProvider, IModule
    {
		private readonly AssemblyCollection _refs = new AssemblyCollection();
		private readonly ManifestFileCollection _files = new ManifestFileCollection();
		private IManifestResourceCollection _resources;
		private ITypeCollection _types;

	    public IAssembly Assembly { get; set; }

        public string Name { get; set; }

        public Guid Version { get; set; }

        public bool IsMain { get; set; }

        public IAssemblyCollection References
        {
            get { return _refs; }
        }

        public IManifestFileCollection Files
        {
            get { return _files; }
        }

        public IManifestResourceCollection Resources
        {
			get { return _resources ?? EmptyResourceCollection.Instance; }
			set { _resources = value; }
        }

	    public object ResolveMetadataToken(IMethod method, int token)
        {
            return null;
        }

	    public ITypeCollection Types
        {
            get { return _types ?? (_types = new TypeCollection()); }
			set { _types = value; }
        }

	    public IEnumerable<ICodeNode> ChildNodes
        {
            get { return new ICodeNode[] {Types}; }
        }

    	/// <summary>
    	/// Gets or sets user defined data assotiated with this object.
    	/// </summary>
    	public object Data { get; set; }

	    public string ToString(string format, IFormatProvider formatProvider)
        {
            return SyntaxFormatter.Format(this, format, formatProvider);
        }

	    public override string ToString()
        {
            return ToString(null, null);
        }

	    private sealed class EmptyResourceCollection : IManifestResourceCollection
		{
			public static readonly IManifestResourceCollection Instance = new EmptyResourceCollection();

			public IEnumerator<IManifestResource> GetEnumerator()
			{
				return Enumerable.Empty<IManifestResource>().GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			public int Count
			{
				get { return 0; }
			}

			public IManifestResource this[int index]
			{
				get { return null; }
			}

			public IManifestResource this[string name]
			{
				get { return null; }
			}
		}
    }
}