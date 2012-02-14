using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DataDynamics.PageFX.CodeModel
{
    /// <summary>
    /// Implementation of <see cref="IAssembly"/>.
    /// </summary>
    [XmlElementName("Assembly")]
    public sealed class AssemblyImpl : AssemblyReference, IAssembly, ITypeCollection
    {
        public AssemblyImpl()
        {
            _modules = new ModuleCollection(this);
        }

        /// <summary>
        /// Gets the list of assembly manifest resources
        /// </summary>
        public ManifestResourceCollection ManifestResources
        {
            get { return _manifestResources; }
        }
        readonly ManifestResourceCollection _manifestResources = new ManifestResourceCollection();

        public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return new ICodeNode[] {_modules}; }
        }

        #region IAssembly Members
        public bool IsCorlib { get; set;  }

        /// <summary>
        /// Gets or sets type of this assembly
        /// </summary>
        public AssemblyType Type { get; set; }

        /// <summary>
        /// Gets or sets path to this assembly
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets hash algorithm calculated when assembly was being signed.
        /// </summary>
        public HashAlgorithmId HashAlgorithm { get; set; }

        /// <summary>
        /// Gets or sets auxiliary marker that can be used for some needs.
        /// </summary>
        public int Marker { get; set; }

        public IMethod EntryPoint { get; set; }

        /// <summary>
        /// Gets the list of assembly modules.
        /// </summary>
        public IModuleCollection Modules
        {
            get { return _modules; }
        }
        readonly ModuleCollection _modules;

        public IModule MainModule
        {
            get
            {
                if (_mainModule == null)
                {
                    foreach (Module module in _modules)
                    {
                        if (module.IsMain)
                        {
                            _mainModule = module;
                            break;
                        }
                    }
                }
                if (_mainModule == null)
                {
                    var mod = new Module {Name = "Main", IsMain = true};
                    _mainModule = mod;
                    Modules.Add(mod);
                }
                return _mainModule;
            }
        }
        IModule _mainModule;

        public IType FindType(string fullname)
        {
        	return _modules.Select(module => module.Types[fullname]).FirstOrDefault(type => type != null);
        }

    	#endregion

        #region ITypeContainer Members
        public ITypeCollection Types
        {
            get { return this; }
        }
        #endregion

        #region ITypeCollection Members
        int ITypeCollection.Count
        {
            get { return _modules.Sum(module => module.Types.Count); }
        }

        IType ITypeCollection.this[int index]
        {
            get
            {
                if (index < 0) return null;
                foreach (var module in _modules)
                {
                    int n = module.Types.Count;
                    if (index < n)
                        return module.Types[index];
                    index -= n;
                }
                return null;
            }
        }

        IType ITypeCollection.this[string fullname]
        {
            get
            {
                return FindType(fullname);
            }
        }

        bool ITypeCollection.Contains(IType type)
        {
            return FindType(type.FullName) != null;
        }

        void ITypeCollection.Add(IType type)
        {
            var mod = MainModule;
            if (mod == null)
                throw new InvalidOperationException();
            mod.Types.Add(type);
        }

        void ITypeCollection.Sort()
        {
            _modules.Sort();
        }
        #endregion

        #region IEnumerable<IType> Members
        IEnumerator<IType> IEnumerable<IType>.GetEnumerator()
        {
        	return _modules.SelectMany(module => module.Types).GetEnumerator();
        }

    	#endregion

        #region IEnumerable Members
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<IType>)this).GetEnumerator();
        }
        #endregion

        public override bool Equals(object obj)
        {
            var asm = obj as IAssembly;
            if (asm == null) return false;
            //if (asm.Location != _location) return false;
            if (!base.Equals(obj)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            int h = base.GetHashCode();
            //if (_location != null)
            //    h ^= _location.GetHashCode();
            return h;
        }
    }

    public sealed class AssemblyCollection : List<IAssembly>, IAssemblyCollection
    {
        #region IAssemblyCollection Members

        public IAssembly this[IAssemblyReference r]
        {
            get { return Find(r.Equals); }
        }

        #endregion
    }
}