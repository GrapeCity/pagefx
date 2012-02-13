using System.Collections.Generic;
using System.Linq;

namespace DataDynamics.PageFX.CodeModel
{
    public static class TypeFilters
    {
        static readonly string[] ExcludedTypes = 
            {
                "avm",
                "AvmErrors",
                "RootAttribute",
                "EmbedAttribute",
            };

        public static bool IsPageFX(IType type)
        {
            if (type == null) return false;

            string ns = type.Namespace;
            if (ns == "Avm" || ns.StartsWith("flash") || ns.StartsWith("adobe")
                || ns.StartsWith("__AS3__") || ns.StartsWith("NUnit")
                || ns == "PageFX" || ns == "DataDynamics.PageFX")
                return true;

            string fullname = type.FullName;
            if (ExcludedTypes.Contains(fullname))
                return true;

            return false;
        }

        const string System_Attribute = "System.Attribute";
        const string System_Exception = "System.Exception";

        public static bool IsAttribute(IType type)
        {
            if (type == null) return false;
            if (type.FullName == System_Attribute) return true;
            return TypeService.IsSubclassOf(type, System_Attribute);
        }

        public static bool IsException(IType type)
        {
            if (type == null) return false;
            if (type.FullName == System_Exception)
                return false;
            return TypeService.IsSubclassOf(type, System_Exception);
        }
    }
}