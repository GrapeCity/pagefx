using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;

namespace DataDynamics.PageFX.FlashLand.Core
{
	internal static class AbcFileExtensions
    {
		public static AbcInstance ImportType(this AbcFile abc, IAssembly assembly, string fullname)
        {
            var type = AssemblyIndex.FindType(assembly, fullname);
            if (type == null)
                throw Errors.Type.UnableToFind.CreateException(fullname);

            var instance = type.Data as AbcInstance;
            if (instance == null)
                throw Errors.Type.NotLinked.CreateException(fullname);

            return abc.ImportInstance(instance);
        }
    }
}