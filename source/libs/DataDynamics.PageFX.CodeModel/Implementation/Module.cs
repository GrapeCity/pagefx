using System;
using System.Collections.Generic;
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
            get { return _resources; }
        }
        readonly ManifestResourceCollection _resources = new ManifestResourceCollection();

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
        public INamespaceCollection Namespaces
        {
            get { return _namespaces; }
        }
        readonly NamespaceCollection _namespaces = new NamespaceCollection();

        public ITypeCollection Types
        {
            get { return _types; }
        }
        readonly TypeCollection _types = new TypeCollection();
        #endregion

        #region ICodeNode Members

        public CodeNodeType NodeType
        {
            get { return CodeNodeType.Module; }
        }

        public IEnumerable<ICodeNode> ChildNodes
        {
            get { return CMHelper.Enumerate(_namespaces); }
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

        public new void Sort()
        {
            Sort((x, y) => x.Name.CompareTo(y.Name));

            foreach (var m in this)
            {
                m.Namespaces.Sort();
                m.Types.Sort();
            }
        }

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
            get { return CMHelper.Convert(this); }
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