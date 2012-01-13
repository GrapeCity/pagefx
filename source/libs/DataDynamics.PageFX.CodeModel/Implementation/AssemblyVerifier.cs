using System;
using System.Reflection;

namespace DataDynamics.PageFX.CodeModel
{
    public static class AssemblyVerifier
    {
        public static bool AreEquals(IModule mod, System.Reflection.Module rmod)
        {
            if (mod.Name != rmod.Name)
                throw new ApplicationException(string.Format("Module names are different."));
            foreach (var rtype in rmod.GetTypes())
            {
                
            }
            return true;
        }

        public static bool Verify(IAssembly asm)
        {
            var rasm = Assembly.LoadFrom(asm.Location);
            foreach (var rmod in rasm.GetModules())
            {
                var mod = asm.Modules[rmod.Name];
                if (mod == null)
                    throw new ApplicationException(string.Format("No module with name {0}", rmod.Name));
                if (!AreEquals(mod, rmod))
                    return false;
            }
            return true;
        }
    }
}