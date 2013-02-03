using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Collections;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	/// <summary>
    /// Implementation of <see cref="IAssembly"/>.
    /// </summary>
    public sealed class AssemblyImpl : AssemblyReference, IAssembly
    {
		private IModule _mainModule;
		private readonly ModuleCollection _modules = new ModuleCollection();
	    private IMethod _entryPoint;
		private SystemTypes _systemTypes;
		private TypeFactory _typeFactory;
		private ITypeCollection _types;

		/// <summary>
		/// Gets or sets user defined data assotiated with this object.
		/// </summary>
		public object Data { get; set; }

	    public IEnumerable<ICodeNode> ChildNodes
        {
            get { return _modules.Cast<ICodeNode>(); }
        }

		/// <summary>
        /// Gets or sets path to this assembly
        /// </summary>
        public string Location { get; set; }

        public IMethod EntryPoint
	    {
			get { return _entryPoint ?? (_entryPoint = ResolveEntryPoint()); }
	    }

		public IAssemblyLoader Loader { get; set; }

		public SystemTypes SystemTypes
		{
			get { return _systemTypes ?? (_systemTypes = ResolveSystemTypes()); }
		}

		public TypeFactory TypeFactory
		{
			get { return _typeFactory ?? (_typeFactory = ResolveTypeFactory()); }
		}

		public IReadOnlyList<IType> GetExposedTypes()
		{
			if (Loader != null)
			{
				return Loader.GetExposedTypes();
			}

			return Types.Where(x => x.IsExposed()).AsReadOnlyList();
		}

		private SystemTypes ResolveSystemTypes()
		{
			return this.IsCorlib() ? new SystemTypes(this) : this.Corlib().SystemTypes;
		}

		private TypeFactory ResolveTypeFactory()
		{
			return this.IsCorlib() ? new TypeFactory() : this.Corlib().TypeFactory;
		}

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
	                _mainModule = _modules.Cast<Module>().FirstOrDefault(x => x.IsMain);
                }

                if (_mainModule == null)
                {
	                var mod = new Module
		                {
			                Assembly = this,
			                Name = "Main",
			                IsMain = true
		                };

                    _mainModule = mod;

                    Modules.Add(mod);
                }

                return _mainModule;
            }
        }
        
		public IType FindType(string fullname)
        {
        	return Types.FindType(fullname);
        }

		public ITypeCollection Types
        {
            get { return _types ?? (_types = new AssemblyTypeCollection(this)); }
        }

		public override bool Equals(object obj)
        {
            var other = obj as IAssembly;
            return other != null && base.Equals(obj);
        }
    }

    public sealed class AssemblyCollection : List<IAssembly>, IAssemblyCollection
    {
    }
}