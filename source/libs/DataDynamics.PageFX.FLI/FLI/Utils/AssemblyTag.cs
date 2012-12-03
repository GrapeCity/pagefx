using System;
using System.Collections.Generic;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FLI.ABC;
using DataDynamics.PageFX.FLI.SWC;
using DataDynamics.PageFX.FLI.SWF;

namespace DataDynamics.PageFX.FLI
{
    /// <summary>
    /// Shared compilers data assotiated with assembly.
    /// </summary>
    internal class AssemblyTag
    {
        public static AssemblyTag Instance(IAssembly asm)
        {
			if (asm == null) return null;
            return asm.Tag as AssemblyTag ?? new AssemblyTag(asm);
        }

        AssemblyTag(IAssembly asm)
        {
            _assembly = asm;
            asm.Tag = this;
        }

        public IAssembly Assembly
        {
            get { return _assembly; }
        }
        readonly IAssembly _assembly;

        public List<AbcFile> AbcFiles
        {
            get { return _abcFiles; }
        }
        readonly List<AbcFile> _abcFiles = new List<AbcFile>();

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
        public object Index;

        public bool IsSwcWrapper
        {
            get { return SWC != null; }
        }

        public AbcInstance InstanceObject { get; set; }

        public AbcInstance InstanceError { get; set; }

        public static AssemblyTag Corlib
        {
            get { return Instance(Common.Corlib.Assembly); }
        }

        public static class AvmGlobalTypes
        {
            public static AbcInstance Object
            {
                get { return Corlib.InstanceObject; }
            }

            public static AbcInstance Error
            {
                get { return Corlib.InstanceError; }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var tag = obj as AssemblyTag;
            if (tag == null) return false;
            if (tag._assembly != _assembly) return false;
            return true;
        }

        static readonly int hs = typeof(AssemblyTag).GetHashCode();

        public override int GetHashCode()
        {
            return _assembly.GetHashCode() ^ hs;
        }

        public override string ToString()
        {
            return _assembly.ToString();
        }
    }
}