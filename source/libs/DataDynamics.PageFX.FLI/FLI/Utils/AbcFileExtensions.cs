using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FLI.ABC;

namespace DataDynamics.PageFX.FLI
{
	internal static class AbcFileExtensions
    {
		public static AbcInstance ImportType(this AbcFile abc, IAssembly assembly, string fullname)
        {
            var type = AssemblyIndex.FindType(assembly, fullname);
            if (type == null)
                throw Errors.Type.UnableToFind.CreateException(fullname);

            var instance = type.Tag as AbcInstance;
            if (instance == null)
                throw Errors.Type.NotLinked.CreateException(fullname);

            return abc.ImportInstance(instance);
        }
    }
}