using System;
using System.Collections.Generic;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Flash.Abc;
using DataDynamics.PageFX.Flash.Swc;
using DataDynamics.PageFX.Flash.Swf;

namespace DataDynamics.PageFX.Flash.Core
{
    /// <summary>
    /// Shared compilers data assotiated with assembly.
    /// </summary>
    internal sealed class AssemblyCustomData
    {
        public static AssemblyCustomData GetInstance(IAssembly assembly)
        {
			if (assembly == null) return null;
            return assembly.Data as AssemblyCustomData ?? new AssemblyCustomData(assembly);
        }

        private AssemblyCustomData(IAssembly assembly)
        {
            Assembly = assembly;
            assembly.Data = this;
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
        public AbcFile Abc
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
    }

	[Flags]
	internal enum InternalAssembyFlags
	{
		PassedLinker = 0x400,
		HasAbcImports = 0x800,
	}
}