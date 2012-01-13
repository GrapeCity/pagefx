using System.Collections;
using System.Collections.Generic;
using System.Xml;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX
{
    internal static class Global
    {
        public static XmlDocument NUnitReport;
        public static IAssembly NUnitAssembly;
        public static List<IAssembly> API;

        public static ApiNode ApiRoot = new ApiNode();
        public static TestNode TestRoot = new TestNode();

        public static Hashtable TestCache = new Hashtable();
    }
}