using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using DataDynamics.PageFX.CodeModel.Syntax;

namespace DataDynamics.PageFX.CodeModel
{
    public interface IAssemblyReferencesResolver
    {
        void ResolveAssemblyReferences();
    }

    public sealed class Module : CustomAttributeProvider, IModule, IXmlSerializationFeedback
    {
        #region IModule Members
        public IAssembly Assembly { get; set; }

        public string Name { get; set; }

        public Guid Version { get; set; }

        public string Location { get; set; }

        public bool IsMain { get; set; }

        public IAssemblyReferencesResolver RefResolver { get; set; }

        public IAssemblyCollection References
        {
            get 
            {
                if (_resolveRefs && RefResolver != null)
                {
                    _resolveRefs = false;
                    RefResolver.ResolveAssemblyReferences();
                }
                return _refs;
            }
        }
        readonly AssemblyCollection _refs = new AssemblyCollection();
        bool _resolveRefs = true;

        public IManifestFileCollection Files
        {
            get { return _files; }
        }
        readonly ManifestFileCollection _files = new ManifestFileCollection();

        public IManifestResourceCollection Resources
        {
			get { return _resources ?? EmptyResourceCollection.Instance; }
			set { _resources = value; }
        }
        private IManifestResourceCollection _resources;

        public IUnmanagedResourceCollection UnmanagedResources
        {
            get { throw new NotSupportedException(); }
        }

        public IMetadataTokenResolver MetadataTokenResolver { get; set; }

        public object ResolveMetadataToken(IMethod method, int token)
        {
            if (MetadataTokenResolver != null)
                return MetadataTokenResolver.ResolveMetadataToken(method, token);
            return null;
        }
        #endregion

        #region ITypeContainer Members
        public ITypeCollection Types
        {
            get { return _types; }
        }
        private readonly TypeCollection _types = new TypeCollection();
        #endregion

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

        #region IXmlSerializationFeedback Members
        string IXmlSerializationFeedback.XmlElementName
        {
            get { return null; }
        }

        void IXmlSerializationFeedback.WriteProperties(XmlWriter writer)
        {
            writer.WriteElementString("Name", Name);
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
				yield break;
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
    [XmlElementName("Modules")]
    public sealed class ModuleCollection : List<IModule>, IModuleCollection
    {
        public ModuleCollection(IAssembly assembly)
        {
            _assembly = assembly;
        }
        readonly IAssembly _assembly;

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