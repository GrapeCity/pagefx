using System;
using System.Collections.Generic;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;
using DataDynamics.PageFX.FlashLand.Swc;
using DataDynamics.PageFX.FlashLand.Swf;

namespace DataDynamics.PageFX.FlashLand.Core
{
    /// <summary>
    /// Shared compilers data assotiated with assembly.
    /// </summary>
    internal sealed class AssemblyCustomData
    {
		private AbcInstance _objectInstance;
		private AbcInstance _errorInstance;

        public static AssemblyCustomData GetInstance(IAssembly asm)
        {
			if (asm == null) return null;
            return asm.Tag as AssemblyCustomData ?? new AssemblyCustomData(asm);
        }

        private AssemblyCustomData(IAssembly asm)
        {
            Assembly = asm;
            asm.Tag = this;
        }

	    public IAssembly Assembly { get; private set; }

		public ISwcLinker Linker { get; set; }

		/// <summary>
		/// Gets or sets auxiliary marker that can be used for some needs.
		/// </summary>
		public InternalAssembyFlags Flags { get; set; }

	    public List<AbcFile> AbcFiles
        {
            get { return _abcFiles; }
        }
        private readonly List<AbcFile> _abcFiles = new List<AbcFile>();

        /// <summary>
        /// ABC file assotiated with assembly.
        /// </summary>
        public AbcFile ABC
        {
            get
            {
                if (_abcFiles.Count > 0)
                    return _abcFiles[0];
                return null;
            }
            set
            {
                AddAbc(value);
            }
        }

        public void AddAbc(AbcFile abc)
        {
            if (abc == null)
                throw new ArgumentNullException("abc");
            if (!_abcFiles.Contains(abc))
                _abcFiles.Add(abc);
        }

        /// <summary>
        /// SWC file assotiated with assembly.
        /// </summary>
        public SwcFile SWC;

        /// <summary>
        /// SWF file assotiated with assembly.
        /// </summary>
        public SwfMovie SWF;

        public RslItem RSL;

        /// <summary>
        /// Index assotiated with assembly.
        /// </summary>
		public AssemblyIndex Index { get; set; }

	    public bool IsSwcWrapper
        {
            get { return SWC != null; }
        }

	    public AbcInstance ObjectInstance
	    {
		    get { return _objectInstance ?? (_objectInstance = ResolveObjectInstance()); }
		    set { _objectInstance = value; }
	    }

	    public AbcInstance ErrorInstance
	    {
		    get { return _errorInstance ?? (_errorInstance = ResolveErrorInstance()); }
		    set { _errorInstance = value; }
	    }

	    public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var tag = obj as AssemblyCustomData;
            if (tag == null) return false;
            if (tag.Assembly != Assembly) return false;
            return true;
        }

        private static readonly int HashSalt = typeof(AssemblyCustomData).GetHashCode();

        public override int GetHashCode()
        {
            return Assembly.GetHashCode() ^ HashSalt;
        }

        public override string ToString()
        {
            return Assembly.ToString();
        }

		private AbcInstance ResolveObjectInstance()
		{
			if (!Assembly.IsCorlib)
				return Assembly.Corlib().CustomData().ObjectInstance;
			return ResolveInstance("Avm.Object");
		}

		private AbcInstance ResolveErrorInstance()
		{
			if (!Assembly.IsCorlib)
				return Assembly.Corlib().CustomData().ErrorInstance;
			return ResolveInstance("Avm.Error");
		}

		private AbcInstance ResolveInstance(string fullname)
		{
			var type = Assembly.FindType(fullname);
			if (type == null) return null;
			// type should be linked on type load
			return type.Tag as AbcInstance;
		}
    }

	[Flags]
	internal enum InternalAssembyFlags
	{
		PassedLinker = 0x400,
		HasAbcImports = 0x800,
	}
}