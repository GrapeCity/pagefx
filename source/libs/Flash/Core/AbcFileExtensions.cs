using DataDynamics.PageFX.Common.CompilerServices;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Flash.Abc;

namespace DataDynamics.PageFX.Flash.Core
{
	internal static class AbcFileExtensions
    {
		public static AbcInstance ImportType(this AbcFile abc, IAssembly assembly, string fullname)
        {
            var type = AssemblyIndex.FindType(assembly, fullname);
            if (type == null)
                throw Errors.Type.UnableToFind.CreateException(fullname);

            var instance = type.AbcInstance();
            if (instance == null)
                throw Errors.Type.NotLinked.CreateException(fullname);

            return abc.ImportInstance(instance);
        }
    }
}