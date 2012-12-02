using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.CodeModel.Syntax;

namespace DataDynamics.PageFX.CodeModel.TypeSystem
{
    public sealed class Module : CustomAttributeProvider, IModule
    {
		private readonly AssemblyCollection _refs = new AssemblyCollection();
		private bool _resolveRefs = true;
		private readonly ManifestFileCollection _files = new ManifestFileCollection();
		private IManifestResourceCollection _resources;
		private ITypeCollection _types;

	    public IAssembly Assembly { get; set; }

        public string Name { get; set; }

        public Guid Version { get; set; }

        public string Location { get; set; }

        public bool IsMain { get; set; }

        public IAssemblyLoader Loader { get; set; }

        public IAssemblyCollection References
        {
            get 
            {
                if (_resolveRefs && Loader != null)
                {
                    _resolveRefs = false;
                    Loader.ResolveAssemblyReferences();
                }
                return _refs;
            }
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
        
        public IUnmanagedResourceCollection UnmanagedResources
        {
            get { throw new NotSupportedException(); }
        }

        public object ResolveMetadataToken(IMethod method, int token)
        {
            if (Loader != null)
				return Loader.ResolveMetadataToken(method, token);
            return null;
        }

	    public ITypeCollection Types
        {
            get { return _types ?? (_types = new TypeCollection()); }
			set { _types = value; }
        }

	    #region ICodeNode Members

        public CodeNodeType NodeType
        {
            get { return CodeNodeType.Module; }
        }

        public IEnumerable<ICodeNode> ChildNodes
        {
            get { return new ICodeNode[] {Types}; }
        }

    	/// <summary>
    	/// Gets or sets user defined data assotiated with this object.
    	/// </summary>
    	public object Tag { get; set; }

    	#endregion

        #region IFormattable Members
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return SyntaxFormatter.Format(this, format, formatProvider);
        }
        #endregion

        #region Object Override Members
        public override string ToString()
        {
            return ToString(null, null);
        }
        #endregion

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

    /// <summary>
    /// List of <see cref="Module"/> objects.
    /// </summary>
    public sealed class ModuleCollection : List<IModule>, IModuleCollection
    {
		private readonly IAssembly _assembly;

        public ModuleCollection(IAssembly assembly)
        {
	        if (assembly == null)
				throw new ArgumentNullException("assembly");

	        _assembly = assembly;
        }

	    public new void Add(IModule module)
        {
            module.Assembly = _assembly;
            base.Add(module);
        }

        #region IModuleCollection Members

	    public IModule this[string name]
        {
            get { return Find(m => m.Name == name); }
        }

        #endregion

        #region ICodeNode Members

        public CodeNodeType NodeType
        {
            get { return CodeNodeType.Modules; }
        }

        public IEnumerable<ICodeNode> ChildNodes
        {
            get { return this.Cast<ICodeNode>(); }
        }

    	/// <summary>
    	/// Gets or sets user defined data assotiated with this object.
    	/// </summary>
    	public object Tag { get; set; }

    	#endregion

        #region IFormattable Members
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return SyntaxFormatter.Format(this, format, formatProvider);
        }
        #endregion

        #region Object Override Members
        public override string ToString()
        {
            return ToString(null, null);
        }
        #endregion
    }
}