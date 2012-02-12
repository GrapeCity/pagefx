using System;
using System.IO;
using DataDynamics.Compression.Zip;
using DataDynamics.PageFX.FLI.ABC;

namespace DataDynamics.PageFX.FLI.SWC
{
    public static class SwcHelper
    {
        internal static string ToSwcID(AbcMultiname mn)
        {
            string ns = mn.NamespaceString;
            string name = mn.NameString;
            if (string.IsNullOrEmpty(ns)) return name;
            return ns + ":" + name;
        }

        public static Stream ExtractLibrary(Stream input)
        {
            var zip = new ZipFile(input);
            return ExtractLibrary(zip);
        }

        public static Stream ExtractLibrary(string path)
        {
            using (var input = File.OpenRead(path))
                return ExtractLibrary(input);
        }

        public static Stream ExtractLibrary(ZipFile zip)
        {
            return Zip.Extract(zip, e => string.Compare(e.Name, SwcFile.LIBRARY_SWF, StringComparison.CurrentCultureIgnoreCase) == 0);
        }
    }
}