using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	public interface IAssemblyLoader : IMetadataTokenResolver
	{
		void ResolveAssemblyReferences();

		IMethod ResolveEntryPoint();
	}

    /// <summary>
    /// Implementation of <see cref="IAssembly"/>.
    /// </summary>
    public sealed class AssemblyImpl : AssemblyReference, IAssembly, ITypeCollection
    {
		private readonly ModuleCollection _modules;
	    private IMethod _entryPoint;

        public AssemblyImpl()
        {
            _modules = new ModuleCollection(this);
        }

	    public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return _modules.Cast<ICodeNode>(); }
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
        /// Gets or sets auxiliary marker that can be used for some needs.
        /// </summary>
        public int Marker { get; set; }

	    public IMethod EntryPoint
	    {
			get { return _entryPoint ?? (_entryPoint = ResolveEntryPoint()); }
			set { _entryPoint = value; }
	    }

		public IAssemblyLoader Loader { get; set; }

	    private IMethod ResolveEntryPoint()
	    {
		    return Loader != null ? Loader.ResolveEntryPoint() : null;
	    }

	    /// <summary>
        /// Gets the list of assembly modules.
        /// </summary>
        public IModuleCollection Modules
        {
            get { return _modules; }
        }
        
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
        private IModule _mainModule;

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

        int IReadOnlyList<IType>.Count
        {
            get { return _modules.Sum(module => module.Types.Count); }
        }

		IType IReadOnlyList<IType>.this[int index]
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
            get { return FindType(fullname); }
        }

        bool ITypeCollection.Contains(IType type)
        {
            return type != null && FindType(type.FullName) != null;
        }

        void ITypeCollection.Add(IType type)
        {
            var mod = MainModule;
            if (mod == null)
                throw new InvalidOperationException();
            mod.Types.Add(type);
        }

	    #endregion

	    IEnumerator<IType> IEnumerable<IType>.GetEnumerator()
        {
        	return _modules.SelectMany(module => module.Types).GetEnumerator();
        }

	    IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<IType>)this).GetEnumerator();
        }

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